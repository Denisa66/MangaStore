using System;
using System.ComponentModel.DataAnnotations;

namespace MangaStoreWeb.Models
{
	public class Manga
{
    public int MangaID { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty; 

    [Required]
    public string Author { get; set; } = string.Empty; 

    public string Genre { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

}
