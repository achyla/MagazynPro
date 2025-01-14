using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using MagazynPro.Models;

namespace MagazynPro.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            // Tworzenie ról
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Tworzenie użytkownika Administratora
            var adminUsername = "admin";
            var adminUser = await userManager.FindByNameAsync(adminUsername);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUsername,
                    Imie = "Administrator",
                    Nazwisko = "Administrator"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                    return; // Zatrzymaj inicjalizację, jeśli utworzenie admina się nie powiodło
                }

                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Upewnij się, że administrator istnieje w tabeli Klienci
            var adminKlient = dbContext.Klienci.FirstOrDefault(k => k.UserId == adminUser.Id);
            if (adminKlient == null)
            {
                adminKlient = new Klient
                {
                    UserId = adminUser.Id,
                    Imie = adminUser.Imie ?? "Administrator",
                    Nazwisko = adminUser.Nazwisko ?? "Administrator"
                };

                dbContext.Klienci.Add(adminKlient);
                await dbContext.SaveChangesAsync();
            }

            Console.WriteLine("Administrator został poprawnie zainicjalizowany.");
        }
    }
}