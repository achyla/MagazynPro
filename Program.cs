using MagazynPro.Data;
using MagazynPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja AppDbContext z Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(); // Dodaj mechanizm uwierzytelniania

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";  // Domyślna strona logowania
    options.LogoutPath = "/Identity/Account/Logout"; // Domyślna strona wylogowania
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Strona dostępu zabronionego
});


// Add services to the container.
builder.Services.AddControllersWithViews();
// Rejestracja Razor Pages
builder.Services.AddRazorPages();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Inicjalizacja ról i użytkownika Administratora
        await DbInitializer.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        // Obsłuż wyjątki podczas inicjalizacji
        Console.WriteLine($"Błąd podczas inicjalizacji bazy danych: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();// Middleware uwierzytelniania
app.UseAuthorization(); // Middleware autoryzacji


// Dodanie tras do stron Identity
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Zamowienia}/{action=Index}/{id?}");

app.Run();