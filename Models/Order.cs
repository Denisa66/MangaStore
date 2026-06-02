using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MangaStoreWeb.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }  

        [Required]
        public string UserID { get; set; } = string.Empty;  

        [ForeignKey("UserID")]
        public IdentityUser? User { get; set; }  

        [Required]
        [Column(TypeName = "decimal(18,2)")] //  Asigura formatul pentru baze de date SQL
        public decimal TotalPrice { get; set; }  

        [Required]
        public string OrderStatus { get; set; } = "In Process"; 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // pentru consistenta

        public List<OrderDetails>? OrderDetails { get; set; } = new List<OrderDetails>(); 
    }
}
