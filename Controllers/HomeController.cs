using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MangaStoreWeb.Data;
using MangaStoreWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MangaStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Pagina principala (Home)
        public async Task<IActionResult> Index(string? query)
        {
            var mangas = string.IsNullOrWhiteSpace(query)
                ? await _context.Manga.ToListAsync()
                : await _context.Manga
                    .Where(m => (m.Title ?? "").Contains(query) || (m.Author ?? "").Contains(query))
                    .ToListAsync();

            return View(mangas); 
        }
    }
}
