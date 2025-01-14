using MagazynPro.Data;
using MagazynPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MagazynPro.Controllers
{
    [Authorize]
    public class ZamowieniaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ZamowieniaController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Zamowienia
        public async Task<IActionResult> Index()
        {
            // Pobierz zalogowanego użytkownika
            var currentUser = await _userManager.GetUserAsync(User);

            // Pobierz zamówienia
            IQueryable<Zamowienie> zamowienia;

            // Sprawdź, czy użytkownik ma rolę "Admin"
            if (User.IsInRole("Admin"))
            {
                // Admin widzi wszystkie zamówienia
                zamowienia = _context.Zamowienia
                    .Include(z => z.Produkt)
                    .Include(z => z.Klient)
                    .ThenInclude(k => k.ApplicationUser);
            }
            else
            {
                // Zwykły użytkownik widzi tylko swoje zamówienia
                zamowienia = _context.Zamowienia
                    .Include(z => z.Produkt)
                    .Include(z => z.Klient)
                    .ThenInclude(k => k.ApplicationUser)
                    .Where(z => z.KlientId == currentUser.Id);
            }

            // Pobierz dane jako listę
            var zamowieniaList = await zamowienia.ToListAsync();

            return View(zamowieniaList);
        }

        // GET: Zamowienia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienia
                .Include(z => z.Produkt)               // Załaduj dane produktu
                .Include(z => z.Klient)                // Załaduj dane klienta
                .ThenInclude(k => k.ApplicationUser)   // Załaduj dane użytkownika
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        // GET: Zamowienia/Create
        public IActionResult Create()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Znajdź klienta w bazie danych
            var klient = _context.Klienci.FirstOrDefault(k => k.UserId == userId);


            if (klient == null)
            {
                // Obsłuż sytuację, gdy klient nie istnieje
                return RedirectToAction("Error", "Home");
            }

            // Utwórz obiekt Zamowienie z ustawionym KlientId
            var zamowienie = new Zamowienie
            {
                KlientId = klient.UserId // Przypisz KlientId
            };

            // Wypełnij ViewBag.Produkty dla listy rozwijanej
            ViewBag.Produkty = _context.Produkty
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.NazwaProduktu} - {p.Cena:C} (Dostępne: {p.Ilosc})",
                    Disabled = p.Ilosc == 0, // Zablokuj produkty o zerowej dostępności

                })
                .ToList();

            // Przekaż model Zamowienie do widoku

            return View(zamowienie);
        }

        // POST: Zamowienia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zamowienie zamowienie)
        {
            Console.WriteLine($"ProduktId: {zamowienie.ProduktId}");
            Console.WriteLine($"Ilosc: {zamowienie.Ilosc}");
            Console.WriteLine($"UserId: {User.FindFirstValue(ClaimTypes.NameIdentifier)}");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Pobierz UserId zalogowanego użytkownika
            var klient = await _context.Klienci.FirstOrDefaultAsync(k => k.UserId == userId);

            if (klient == null)
            {
                Console.WriteLine("Klient nie został znaleziony.");
                ModelState.AddModelError(string.Empty, "Nie znaleziono klienta powiązanego z użytkownikiem.");
                ViewBag.Produkty = _context.Produkty
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.NazwaProduktu} - {p.Cena:C} (Dostępne: {p.Ilosc})",
                        Disabled = p.Ilosc == 0, // Zablokuj produkty o zerowej dostępności

                    })
                    .ToList();
                return View(zamowienie);
            }

            zamowienie.KlientId = klient.UserId; // Przypisz KlientId jako UserId klienta
            zamowienie.DataZamowienia = DateTime.Now;
            Console.WriteLine($"KlientId: {zamowienie.KlientId}");


            var produkt = await _context.Produkty.FindAsync(zamowienie.ProduktId);
            if (produkt == null)
            {
                Console.WriteLine("Produkt nie został znaleziony.");
                ModelState.AddModelError("ProduktId", "Wybrany produkt nie istnieje.");
                ViewBag.Produkty = _context.Produkty
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.NazwaProduktu} - {p.Cena:C} (Dostępne: {p.Ilosc})",
                        Disabled = p.Ilosc == 0, // Zablokuj produkty o zerowej dostępności

                    })
                    .ToList();
                return View(zamowienie);
            }

            if (zamowienie.Ilosc > produkt.Ilosc)
            {
                ModelState.AddModelError("Ilosc", $"Maksymalna dostępna ilość to {produkt.Ilosc}.");
                ViewBag.Produkty = _context.Produkty
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.NazwaProduktu} - {p.Cena:C} (Dostępne: {p.Ilosc})",
                        Disabled = p.Ilosc == 0, // Zablokuj produkty o zerowej dostępności

                    })
                    .ToList();
                return View(zamowienie);
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model error: {error.ErrorMessage}");
                }
                ViewBag.Produkty = _context.Produkty
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.NazwaProduktu} - {p.Cena:C} (Dostępne: {p.Ilosc})",
                        Disabled = p.Ilosc == 0, // Zablokuj produkty o zerowej dostępności

                    })
                    .ToList();
                return View(zamowienie);
            }

            try
            {
                produkt.Ilosc -= zamowienie.Ilosc;
                _context.Update(produkt);

                _context.Zamowienia.Add(zamowienie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania zamówienia: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Wystąpił błąd podczas dodawania zamówienia.");
                return View(zamowienie);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Zamowienia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Pobierz zamówienie z bazy danych, w tym powiązane dane klienta i produktu
            var zamowienie = await _context.Zamowienia
                .Include(z => z.Produkt) // Załaduj szczegóły produktu
                .Include(z => z.Klient)  // Załaduj szczegóły klienta
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            // Sprawdź, czy użytkownik jest administratorem
            if (!User.IsInRole("Admin"))
            {

                // Pobierz ID zalogowanego użytkownika
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Znajdź klienta powiązanego z zalogowanym użytkownikiem
                var klient = await _context.Klienci.FirstOrDefaultAsync(k => k.UserId == userId);

                if (klient == null || zamowienie.KlientId != klient.UserId)
                {
                    // Jeśli klient nie istnieje lub nie jest właścicielem zamówienia, zwróć błąd
                    return RedirectToAction("Error", "Home");
                }
            }

            // Wypełnij ViewBag.Produkty dla potencjalnych edycji produktu (jeśli konieczne)
            ViewBag.Produkty = _context.Produkty
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.NazwaProduktu} - {p.Cena:C} (Dostępne: {p.Ilosc})",
                    Disabled = p.Ilosc == 0, // Zablokuj produkty o zerowej dostępności

                })
                .ToList();

            return View(zamowienie);
        }


        // POST: Zamowienia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Zamowienie zamowienie)
        {
            // Pobierz istniejące zamówienie z bazy danych
            var existingZamowienie = await _context.Zamowienia
                .Include(z => z.Produkt) // Ładujemy produkt
                .Include(z => z.Klient)  // Ładujemy klienta
                .FirstOrDefaultAsync(z => z.Id == id);

            if (existingZamowienie == null)
            {
                return NotFound();
            }
            zamowienie.Produkt = existingZamowienie.Produkt;
            zamowienie.Klient = existingZamowienie.Klient;

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(zamowienie);
            }
            // Oblicz różnicę między nową ilością a istniejącą ilością
            int roznicaIlosci = zamowienie.Ilosc - existingZamowienie.Ilosc;

            // Sprawdź, czy nowa ilość jest możliwa do zrealizowania
            if (roznicaIlosci > 0 && roznicaIlosci > existingZamowienie.Produkt.Ilosc)
            {
                ModelState.AddModelError("Ilosc", "Zamówiona ilość przekracza dostępny stan magazynowy.");
                zamowienie.Produkt = existingZamowienie.Produkt;
                zamowienie.Klient = existingZamowienie.Klient;
                return View(zamowienie);
            }

            try
            {
                if (roznicaIlosci > 0)
                {
                    // Zmniejszamy ilość w magazynie, jeśli zamówiona ilość wzrosła
                    existingZamowienie.Produkt.Ilosc -= roznicaIlosci;
                }
                else if (roznicaIlosci < 0)
                {
                    // Zwiększamy ilość w magazynie, jeśli zamówiona ilość została zmniejszona
                    existingZamowienie.Produkt.Ilosc += Math.Abs(roznicaIlosci);
                }

                // Aktualizuj dane zamówienia
                existingZamowienie.Ilosc = zamowienie.Ilosc;
                existingZamowienie.DataZamowienia = DateTime.Now;

                _context.Update(existingZamowienie.Produkt); // Zaktualizuj produkt
                _context.Update(existingZamowienie); // Zaktualizuj zamówienie
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZamowieniaExists(zamowienie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Zamowienia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienie = await _context.Zamowienia
                .Include(z => z.Produkt)
                .Include(z => z.Klient)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        // POST: Zamowienia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Pobierz zamówienie z bazy danych
            var zamowienie = await _context.Zamowienia
                .Include(z => z.Produkt) // Załaduj powiązany produkt
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            // Przywróć ilość produktu na magazyn
            if (zamowienie.Produkt != null)
            {
                zamowienie.Produkt.Ilosc += zamowienie.Ilosc; // Dodaj zamówioną ilość z powrotem do magazynu
                _context.Produkty.Update(zamowienie.Produkt); // Zaktualizuj produkt w bazie danych
            }

            // Usuń zamówienie z bazy danych
            _context.Zamowienia.Remove(zamowienie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZamowieniaExists(int id)
        {
            return _context.Zamowienia.Any(e => e.Id == id);
        }
    }
}
