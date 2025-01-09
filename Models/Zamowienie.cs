namespace MagazynPro.Models
{
    public class Zamowienia
    {
        public int Id { get; set; }
        public string NazwaProduktu { get; set; }
        public int Ilosc { get; set; }
        public DateTime DataZamowienia { get; set; }
    }
}
