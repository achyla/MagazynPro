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
                    UserName = "admin",
                    Imie = "Administrator",
                    Nazwisko = "Administrator"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    // Dodaj administratora do tabeli Klienci
                    var klient = new Klient
                    {
                        UserId = adminUser.Id,
                        Imie = adminUser.Imie,
                        Nazwisko = adminUser.Nazwisko
                    };

                    dbContext.Klienci.Add(klient);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
            else
            {
                // Jeśli admin istnieje, sprawdź, czy jest w tabeli Klienci
                if (!dbContext.Klienci.Any(k => k.UserId == adminUser.Id))
                {
                    var klient = new Klient
                    {
                        UserId = adminUser.Id,
                        Imie = adminUser.Imie,
                        Nazwisko = adminUser.Nazwisko
                    };

                    dbContext.Klienci.Add(klient);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
