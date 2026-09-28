using Mango.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.EmailAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLogger> EmailLogger { get; set; }

    }
}
