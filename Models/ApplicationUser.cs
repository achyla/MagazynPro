using Microsoft.AspNetCore.Identity;

namespace MagazynPro.Models
{
        public class ApplicationUser : IdentityUser
        {
            public string Imie { get; set; }
            public string Nazwisko { get; set; }
        }
}