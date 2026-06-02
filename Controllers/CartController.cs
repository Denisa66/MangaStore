using MangaStoreWeb.Data;
using MangaStoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MangaStoreWeb.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Afiseaza cosul 
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _context.Cart
                .Include(c => c.Manga)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        // Adauga in cos
        public async Task<IActionResult> AddToCart(int mangaId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var manga = await _context.Manga.FindAsync(mangaId);
            if (manga == null)
            {
                return NotFound();
            }

            var cartItem = await _context.Cart
                .FirstOrDefaultAsync(c => c.UserId == userId && c.MangaID == mangaId);

            if (cartItem == null)
            {
                var newCartItem = new Cart
                {
                    UserId = userId,
                    MangaID = mangaId,
                    Manga = manga,
                    Quantity = 1
                };

                _context.Cart.Add(newCartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //  Update quantity 
        public async Task<IActionResult> UpdateQuantity(int cartId, int quantity)
        {
            var cartItem = await _context.Cart.FindAsync(cartId);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Sterge din cos
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            var cartItem = await _context.Cart.FindAsync(cartId);
            if (cartItem != null)
            {
                _context.Cart.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // sterge tot din cos
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.Cart.Where(c => c.UserId == userId);
            if (cartItems.Any())
            {
                _context.Cart.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index"); 
        }
    }
}
