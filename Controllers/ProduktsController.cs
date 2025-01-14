  using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazynPro.Data;
using Microsoft.AspNetCore.Authorization;
using MagazynPro.Models;

namespace MagazynPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProduktsController : Controller
    {
        private readonly AppDbContext _context;

        public ProduktsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Produkts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produkty.ToListAsync());
        }

        // GET: Produkts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // GET: Produkts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produkts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produkt produkt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(produkt);
                    await _context.SaveChangesAsync(); // Próba zapisania produktu w bazie danych
                    return RedirectToAction(nameof(Index)); // Przekierowanie do listy produktów
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas dodawania produktu: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Wystąpił błąd podczas dodawania produktu.");
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Błąd walidacji: {error.ErrorMessage}");
                }
            }
            ViewBag.Produkty = _context.Produkty
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.NazwaProduktu} - {p.Cena:C}"
                })
                .ToList();

            // Jeśli coś poszło nie tak, zwróć widok z produktem
            return View(produkt);
        }

        // GET: Produkts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }
            return View(produkt);
        }

        // POST: Produkts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produkt produkt)
        {
            if (id != produkt.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(produkt);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (FormatException ex)
            {
                ModelState.AddModelError(nameof(produkt.CenaInput), ex.Message); // Dodaj błąd do ModelState
            }
            ViewBag.Produkty = _context.Produkty
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.NazwaProduktu} - {p.Cena:C}"
                })
                .ToList();
            return View(produkt); // Powróć do widoku z błędami
        }

        // GET: Produkts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkt = await _context.Produkty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null)
            {
                return NotFound();
            }

            return View(produkt);
        }

        // POST: Produkts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt != null)
            {
                _context.Produkty.Remove(produkt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktExists(int id)
        {
            return _context.Produkty.Any(e => e.Id == id);
        }
    }
}
