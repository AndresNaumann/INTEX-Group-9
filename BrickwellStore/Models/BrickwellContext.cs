using Microsoft.EntityFrameworkCore;

namespace BrickwellStore.Models
{
    public class BrickwellContext : DbContext
    {
        public BrickwellContext(DbContextOptions<BrickwellContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        dasdasd
        // This is a comment and more stuff


    }
}
dasdaasdnaumann