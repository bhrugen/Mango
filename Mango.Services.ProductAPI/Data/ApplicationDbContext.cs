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
                       new Product { ProductId = 1, Name = "Bruschetta", Price = 8.99, Description = "Grilled bread rubbed with garlic and topped with fresh tomato, basil, and olive oil.", CategoryName = "Appetizer" },
        new Product { ProductId = 2, Name = "Caprese Salad", Price = 9.99, Description = "Sliced tomatoes and fresh mozzarella layered with basil and a balsamic drizzle.", CategoryName = "Appetizer" },
        new Product { ProductId = 3, Name = "Focaccia Bread", Price = 6.99, Description = "Oven-baked Italian flatbread with rosemary, sea salt, and olive oil.", CategoryName = "Appetizer" },

        // Main Course
        new Product { ProductId = 4, Name = "Cacio e Pepe", Price = 15.99, Description = "Classic Roman pasta tossed with Pecorino Romano and cracked black pepper.", CategoryName = "Main Course" },
        new Product { ProductId = 5, Name = "Margherita Pizza", Price = 14.99, Description = "Wood-fired pizza with San Marzano tomatoes, fresh mozzarella, and basil.", CategoryName = "Main Course" },
        new Product { ProductId = 6, Name = "Pesto Pasta", Price = 15.49, Description = "Pasta tossed in a vibrant basil pesto with pine nuts and Parmesan.", CategoryName = "Main Course" },
        new Product { ProductId = 7, Name = "Pesto Pizza", Price = 16.49, Description = "Wood-fired pizza topped with basil pesto, mozzarella, and cherry tomatoes.", CategoryName = "Main Course" },
        new Product { ProductId = 8, Name = "Pomodoro Pasta", Price = 13.99, Description = "Pasta in a rich San Marzano tomato sauce with fresh basil and garlic.", CategoryName = "Main Course" },
        new Product { ProductId = 9, Name = "Spinach Ravioli", Price = 16.99, Description = "Handmade ravioli filled with ricotta and spinach in a sage butter sauce.", CategoryName = "Main Course" },

        // Desserts
        new Product { ProductId = 10, Name = "Cannolo", Price = 7.99, Description = "Crisp pastry shell filled with sweetened ricotta and chocolate chips.", CategoryName = "Dessert" },
        new Product { ProductId = 11, Name = "Panna Cotta", Price = 7.49, Description = "Silky vanilla bean panna cotta topped with a fresh berry compote.", CategoryName = "Dessert" },
        new Product { ProductId = 12, Name = "Tiramisu", Price = 8.49, Description = "Layers of espresso-soaked ladyfingers and mascarpone cream, dusted with cocoa.", CategoryName = "Dessert" }

            );
        }
    }
}
