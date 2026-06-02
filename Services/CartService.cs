using MangaStoreWeb.Models;

namespace MangaStoreWeb.Services
{
    public class CartService
    {
        public decimal CalculateCartTotal(IEnumerable<Cart> cartItems)
        {
            if (cartItems == null)
            {
                throw new ArgumentNullException(nameof(cartItems));
            }

            return cartItems.Sum(item => item.Manga.Price * item.Quantity);
        }

        public decimal CalculateItemTotal(decimal price, int quantity)
        {
            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            if (quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }

            return price * quantity;
        }

        public bool IsMangaAvailable(Manga manga)
        {
            if (manga == null)
            {
                throw new ArgumentNullException(nameof(manga));
            }

            return manga.Stock > 0;
        }
    }
}