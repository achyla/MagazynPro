using MagazynPro.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Produkt
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
    [StringLength(100, ErrorMessage = "Nazwa produktu nie może być dłuższa niż 100 znaków.")]
    [Column(TypeName = "nvarchar(100)")] // Określenie typu danych w bazie
    public required string NazwaProduktu { get; set; }
    [Required(ErrorMessage = "Cena jest wymagana.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0.")]
    [Column(TypeName = "decimal(18, 2)")] // Precyzja i skala dla wartości dziesiętnej
    public decimal Cena { get; set; }

    [Required(ErrorMessage = "Ilość jest wymagana.")]
    [Range(0, int.MaxValue, ErrorMessage = "Ilość nie może być ujemna.")]
    [Column(TypeName = "int")] // Określenie typu danych
    public int Ilosc { get; set; }

    // Relacja z zamówieniami (opcjonalne)
    public ICollection<Zamowienie>? Zamowienia { get; set; }
}
