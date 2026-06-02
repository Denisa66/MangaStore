using MangaStoreWeb.Data;
using MangaStoreWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MangaStoreWeb.Controllers
{
    [Authorize] 
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

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

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewData["Total"] = cartItems.Sum(i => i.Manga.Price * i.Quantity);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
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

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                UserID = userId,
                CreatedAt = DateTime.Now,  
                TotalPrice = cartItems.Sum(i => i.Manga.Price * i.Quantity),
                OrderStatus = "In Process"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    OrderID = order.OrderID,
                    MangaID = item.MangaID,
                    Quantity = item.Quantity,
                    Price = item.Manga.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }

            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
