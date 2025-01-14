namespace MagazynPro.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;

    public class Produkt
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa produktu nie może być dłuższa niż 100 znaków.")]
        [Column(TypeName = "nvarchar(100)")] // Określenie typu danych w bazie
        public required string NazwaProduktu { get; set; }
        [Required(ErrorMessage = "Cena jest wymagana.")]
        [Range(0.01, 999999.99, ErrorMessage = "Cena musi być większa niż 0.")]
        [Column(TypeName = "decimal(8, 2)")] // Precyzja i skala dla wartości dziesiętnej
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Range(0, int.MaxValue, ErrorMessage = "Ilość nie może być ujemna.")]
        [Column(TypeName = "int")] // Określenie typu danych
        public int Ilosc { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Cena jest wymagana.")]
        [RegularExpression(@"^\d+([\.,]\d{1,2})?$", ErrorMessage = "Cena musi być liczbą z maksymalnie dwoma miejscami dziesiętnymi.")]
        public string CenaInput
        {
            get => Cena.ToString(CultureInfo.InvariantCulture);
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Replace(",", ".");
                    if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                    {
                        Cena = parsedValue;
                    }
                    else
                    {
                        throw new FormatException("Niepoprawny format liczby. Cena musi być liczbą z maksymalnie dwoma miejscami dziesiętnymi.");
                    }
                }
            }
        }

        // Relacja z zamówieniami (opcjonalne)
        public ICollection<Zamowienie>? Zamowienia { get; set; }
    }
}