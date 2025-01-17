﻿namespace MagazynPro.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Zamowienie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Display(Name = "Ilość")]
        [Range(1, int.MaxValue)]
        public int Ilosc { get; set; }

        [Display(Name = "Data Zamówienia")]
        public DateTime DataZamowienia { get; set; }


        [Required]
        [ForeignKey("Klient")]
        [Display(Name = "Klient")]
        public string KlientId { get; set; } // Klucz obcy do modelu Klient
        public Klient? Klient { get; set; } // Relacja do modelu Klient

        [Required]
        [ForeignKey("Produkt")]
        public int ProduktId { get; set; } // Klucz obcy do modelu Produkt
        public Produkt? Produkt { get; set; } // Relacja do modelu Produkt
    }
}