using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaStoreWeb.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public required string UserId { get; set; }  // leaga cosul de utilizator

        [ForeignKey("Manga")]
        public int MangaID { get; set; }
        public required Manga Manga { get; set; }  

        public int Quantity { get; set; }  

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } 

        public decimal TotalPrice => Quantity * Price;  
    }
}
