namespace MagazynPro.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Zamowienie
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa Produktu")]
        public  string NazwaProduktu { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Display(Name = "Ilość")]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż 0.")]
        public int Ilosc { get; set; }

        [Display(Name = "Data Zamówienia")]
        public DateTime DataZamowienia { get; set; }

        [ForeignKey("Klient")]
        [Display(Name = "Klient")]
        public int KlientId { get; set; }
        public Klient Klient { get; set; }

        [ForeignKey("Produkt")]
        public int ProduktId { get; set; } // Klucz obcy do modelu Produkt
        public Produkt Produkt { get; set; } // Relacja do modelu Produkt
    }
}
