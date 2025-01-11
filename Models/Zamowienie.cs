namespace MagazynPro.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Zamowienie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Display(Name = "Ilość")]
        [Range(1, int.MaxValue)]
        public required int Ilosc { get; set; }

        [Required(ErrorMessage = "Data zamówienia jest wymagana.")]
        [Display(Name = "Data Zamówienia")]
        public DateTime DataZamowienia { get; set; }

        [Required(ErrorMessage = "Klient jest wymagany.")]
        [ForeignKey("Klient")]
        [Display(Name = "Klient")]
        public string KlientId { get; set; }
        public Klient Klient { get; set; }

        [Required(ErrorMessage = "Produkt jest wymagany.")]
        [ForeignKey("Produkt")]
        public int ProduktId { get; set; } // Klucz obcy do modelu Produkt
        public Produkt Produkt { get; set; } // Relacja do modelu Produkt
    }
}
