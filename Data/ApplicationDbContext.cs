using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using MangaStoreWeb.Models;


namespace MangaStoreWeb.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }

		public DbSet<Manga> Manga { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<Cart> Cart { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// precizia pentru toate coloanele `decimal`
			modelBuilder.Entity<Manga>()
				.Property(m => m.Price)
				.HasPrecision(18, 2);

			modelBuilder.Entity<Order>()
				.Property(o => o.TotalPrice)
				.HasPrecision(18, 2);

			modelBuilder.Entity<OrderDetails>()
				.Property(od => od.Price)
				.HasPrecision(18, 2);
		}
	}
}

