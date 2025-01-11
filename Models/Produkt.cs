using MagazynPro.Models;
using System.ComponentModel.DataAnnotations;

public class Produkt
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
    [StringLength(100, ErrorMessage = "Nazwa produktu nie może być dłuższa niż 100 znaków.")]
    public required string Nazwa { get; set; }

    [Required(ErrorMessage = "Cena jest wymagana.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0.")]
    public decimal Cena { get; set; }

    // Relacja z zamówieniami (opcjonalne)
    public required ICollection<Zamowienie> Zamowienia { get; set; }
}
