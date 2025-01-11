using MagazynPro.Models;
using System.ComponentModel.DataAnnotations;

public class Klient
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Imię jest wymagane.")]
    [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
    public required string Imie { get; set; }

    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    public required string Nazwisko { get; set; }

    // Relacja z zamówieniami
    public ICollection<Zamowienie> Zamowienia { get; set; }
    public Klient()
    {
        Zamowienia = new List<Zamowienie>();
    }
}
