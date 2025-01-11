using MagazynPro.Data;
using MagazynPro.Models;
using Microsoft.AspNetCore.Authorization;
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

        public ZamowieniaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Zamowienia
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zamowienia.ToListAsync());
        }

        // GET: Zamowienia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienia = await _context.Zamowienia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zamowienia == null)
            {
                return NotFound();
            }

            return View(zamowienia);
        }

        // GET: Zamowienia/Create
        public IActionResult Create()
        {
            // Pobierz listę nazw produktów z bazy danych
            ViewData["ProduktId"] = new SelectList(_context.Produkty, "Id", "NazwaProduktu");
            return View();
        }

        // POST: Zamowienia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProduktId,Ilosc")] Zamowienie zamowienie)
        {
            // Logowanie przychodzących danych
            Console.WriteLine($"ProduktId: {zamowienie.ProduktId}");
            Console.WriteLine($"Ilosc: {zamowienie.Ilosc}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState nie jest poprawny.");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Pole: {key}, Błąd: {error.ErrorMessage}");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState jest poprawny.");

                try
                {
                    // Przypisz zalogowanego użytkownika jako klienta
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Console.WriteLine($"Zalogowany użytkownik ID: {userId}");

                    var klient = await _context.Klienci.FirstOrDefaultAsync(k => k.UserId == userId);
                    if (klient == null)
                    {
                        Console.WriteLine("Nie znaleziono klienta powiązanego z użytkownikiem.");
                        ModelState.AddModelError(string.Empty, "Nie znaleziono klienta powiązanego z użytkownikiem.");
                        ViewData["ProduktId"] = new SelectList(_context.Produkty, "Id", "NazwaProduktu", zamowienie.ProduktId);
                        return View(zamowienie);
                    }
                    Console.WriteLine($"Znaleziono klienta: {klient.Id}");

                    var produkt = await _context.Produkty.FindAsync(zamowienie.ProduktId);
                    if (produkt == null)
                    {
                        Console.WriteLine($"Nie znaleziono produktu o ID {zamowienie.ProduktId}");
                        ModelState.AddModelError("ProduktId", "Wybrany produkt nie istnieje.");
                    }
                    else if (zamowienie.Ilosc > produkt.Ilosc)
                    {
                        Console.WriteLine($"Zamówiona ilość ({zamowienie.Ilosc}) przekracza dostępny stan magazynowy ({produkt.Ilosc}).");
                        ModelState.AddModelError("Ilosc", "Zamówiona ilość przekracza dostępny stan magazynowy.");
                    }
                    else
                    {
                        // Zmniejsz ilość produktu w magazynie
                        produkt.Ilosc -= zamowienie.Ilosc;
                        _context.Update(produkt);

                        // Przypisz klienta i datę zamówienia
                        zamowienie.KlientId = klient.Id;
                        zamowienie.DataZamowienia = DateTime.Now;

                        // Dodaj zamówienie do bazy danych
                        _context.Zamowienia.Add(zamowienie);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    // Logowanie błędów
                    Console.WriteLine($"Błąd podczas dodawania zamówienia: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Wystąpił błąd podczas dodawania zamówienia.");
                }

            }
            ViewData["ProduktId"] = new SelectList(_context.Produkty, "Id", "NazwaProduktu", zamowienie.ProduktId);
            return View(zamowienie);
        }

        // GET: Zamowienia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienia = await _context.Zamowienia.FindAsync(id);
            if (zamowienia == null)
            {
                return NotFound();
            }
            return View(zamowienia);
        }

        // POST: Zamowienia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NazwaProduktu,Ilosc,DataZamowienia,KlientId")] Zamowienie zamowienia)
        {
            if (id != zamowienia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zamowienia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowieniaExists(zamowienia.Id))
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
            return View(zamowienia);
        }

        // GET: Zamowienia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienia = await _context.Zamowienia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zamowienia == null)
            {
                return NotFound();
            }

            return View(zamowienia);
        }

        // POST: Zamowienia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienia = await _context.Zamowienia.FindAsync(id);
            if (zamowienia != null)
            {
                _context.Zamowienia.Remove(zamowienia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZamowieniaExists(int id)
        {
            return _context.Zamowienia.Any(e => e.Id == id);
        }
    }
}
