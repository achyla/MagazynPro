namespace MagazynPro.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Zamowienie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
        [Display(Name = "Nazwa Produktu")]
        public required string NazwaProduktu { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Display(Name = "Ilość")]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż 0.")]
        public int Ilosc { get; set; }

        [Required(ErrorMessage = "Data zamówienia jest wymagana.")]
        [Display(Name = "Data Zamówienia")]
        public DateTime DataZamowienia { get; set; }

        [Required]
        [ForeignKey("Klient")]
        [Display(Name = "Klient")]

        public int KlientId { get; set; }
        public Klient Klient { get; set; }
    }
}
