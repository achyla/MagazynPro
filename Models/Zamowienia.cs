namespace MagazynPro.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Zamowienia
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
        public required string NazwaProduktu { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż 0.")]
        public int Ilosc { get; set; }

        [Required(ErrorMessage = "Data zamówienia jest wymagana.")]
        public DateTime DataZamowienia { get; set; }

        // Relacja do Klient
        [ForeignKey("Klient")]
        public int KlientId { get; set; }
        public required Klient Klient { get; set; }
    }
}
