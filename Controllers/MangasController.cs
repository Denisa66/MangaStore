using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangaStoreWeb.Data;
using MangaStoreWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace MangaStoreWeb.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class MangasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MangasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metoda pentru căutare
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index", "Home"); 
            }

            var manga = await _context.Manga
                .FirstOrDefaultAsync(m => m.Title.Contains(query) || m.Author.Contains(query));

            if (manga == null)
            {
                return NotFound(); 
            }

            return RedirectToAction("Details", new { id = manga.MangaID });
        }

        //  detaliilor unei manga 
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var manga = await _context.Manga
                .FirstOrDefaultAsync(m => m.MangaID == id);

            if (manga == null)
            {
                return NotFound();
            }

            return View(manga); 
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Admin Panel"; 
            return View(await _context.Manga.ToListAsync());
        }

        //  Doar adminii pot adauga manga
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MangaID,Title,Author,Genre,Price,Stock,Description,ImageURL,CreatedAt")] Manga manga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manga);
        }

        // Doar adminii pot edita
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manga = await _context.Manga.FindAsync(id);
            if (manga == null)
            {
                return NotFound();
            }
            return View(manga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MangaID,Title,Author,Genre,Price,Stock,Description,ImageURL,CreatedAt")] Manga manga)
        {
            if (id != manga.MangaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MangaExists(manga.MangaID))
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
            return View(manga);
        }

        // Doar adminii pot sterge manga
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manga = await _context.Manga
                .FirstOrDefaultAsync(m => m.MangaID == id);
            if (manga == null)
            {
                return NotFound();
            }

            return View(manga);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manga = await _context.Manga.FindAsync(id);
            if (manga != null)
            {
                _context.Manga.Remove(manga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MangaExists(int id)
        {
            return _context.Manga.Any(e => e.MangaID == id);
        }
    }
}
