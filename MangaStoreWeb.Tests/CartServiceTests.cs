using MangaStoreWeb.Models;
using MangaStoreWeb.Services;
using Xunit;

namespace MangaStoreWeb.Tests
{
    public class CartServiceTests
    {
        [Fact]
        public void CalculateItemTotal_ReturnsCorrectTotal()
        {
            var service = new CartService();

            var result = service.CalculateItemTotal(50m, 2);

            Assert.Equal(100m, result);
        }

        [Fact]
        public void CalculateCartTotal_ReturnsCorrectTotal()
        {
            var service = new CartService();

            var cartItems = new List<Cart>
            {
                new Cart
                {
                    UserId = "test-user",
                    Quantity = 2,
                    Manga = new Manga
                    {
                        Price = 50m
                    }
                },
                new Cart
                {
                    UserId = "test-user",
                    Quantity = 1,
                    Manga = new Manga
                    {
                        Price = 30m
                    }
                }
            };

            var result = service.CalculateCartTotal(cartItems);

            Assert.Equal(130m, result);
        }

        [Fact]
        public void IsMangaAvailable_ReturnsTrue_WhenStockIsGreaterThanZero()
        {
            var service = new CartService();

            var manga = new Manga
            {
                Stock = 5
            };

            var result = service.IsMangaAvailable(manga);

            Assert.True(result);
        }

        [Fact]
        public void IsMangaAvailable_ReturnsFalse_WhenStockIsZero()
        {
            var service = new CartService();

            var manga = new Manga
            {
                Stock = 0
            };

            var result = service.IsMangaAvailable(manga);

            Assert.False(result);
        }

        [Fact]
        public void CalculateItemTotal_ThrowsException_WhenPriceIsNegative()
        {
            var service = new CartService();

            Assert.Throws<ArgumentException>(() => service.CalculateItemTotal(-10m, 2));
        }
    }
}