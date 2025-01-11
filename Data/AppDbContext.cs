using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MagazynPro.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagazynPro.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Zamowienia> Zamowienia { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<Produkt> Produkty { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Dodanie konfiguracji dla ApplicationUser
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }

        private class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)
            {
                builder.Property(x => x.Imie)
                    .HasMaxLength(255)
                    .IsRequired(false);

                builder.Property(x => x.Nazwisko)
                    .HasMaxLength(255)
                    .IsRequired(false);
            }
        }
    }
}