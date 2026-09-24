using Mango.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.ProductAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Mango Chicken Tikka", Price = 14.99, Description = "Tender chicken tikka glazed with mango chutney.", CategoryName = "Main Course" },
                new Product { ProductId = 2, Name = "Mango Lassi", Price = 4.99, Description = "Chilled yogurt drink blended with fresh mango.", CategoryName = "Beverage" }
            );
        }
    }
}
