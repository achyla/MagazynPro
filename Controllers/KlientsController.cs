using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagazynPro.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MagazynPro.Models;

namespace MagazynPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KlientsController : Controller
    {
        private readonly AppDbContext _context;

        public KlientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Klients
        public async Task<IActionResult> Index()
        {
            var klienci = await _context.Klienci.ToListAsync();
            return View(klienci);
        }


        // GET: Klients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klient = await _context.Klienci
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (klient == null)
            {
                return NotFound();
            }

            return View(klient);
        }


        // GET: Klients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Klients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Imie,Nazwisko")] Klient klient)
        {
            // Pobierz UserId zalogowanego użytkownika
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Nie można przypisać użytkownika do klienta.");
                return View(klient);
            }

            // Przypisz UserId do klienta
            klient.UserId = userId;

            // Walidacja i zapis do bazy
            if (ModelState.IsValid)
            {
                _context.Klienci.Add(klient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Przekierowanie do listy klientów
            }

            return View(klient);
        }

        // GET: Klients/Edit

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klient = await _context.Klienci.FindAsync(id);
            if (klient == null)
            {
                return NotFound();
            }

            return View(klient);
        }

        // POST: Klients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,Imie,Nazwisko")] Klient klient)
        {
            if (id != klient.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KlientExists(klient.UserId))
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
            return View(klient);
        }

        private bool KlientExists(string id)
        {
            return _context.Klienci.Any(e => e.UserId == id);
        }



        // GET: Klients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klient = await _context.Klienci
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (klient == null)
            {
                return NotFound();
            }

            return View(klient);
        }


        // POST: Klients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var klient = await _context.Klienci.FindAsync(id);
            if (klient != null)
            {
                _context.Klienci.Remove(klient);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
