namespace MagazynPro.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Klient
    {
        [Key] // Ustawiamy UserId jako klucz główny
        public required string UserId { get; set; } // GUID użytkownika z tabeli AspNetUsers

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
        public required string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public required string Nazwisko { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        // Relacja z zamówieniami
        public ICollection<Zamowienie> Zamowienia { get; set; } = new List<Zamowienie>();
    }
}