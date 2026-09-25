using Mango.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.ShoppingCartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CartHeaderDto> CartHeaders { get; set; }
        public DbSet<CartDetailsDto> CartDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
        }
    }
}
