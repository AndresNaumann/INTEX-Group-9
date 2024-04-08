using Microsoft.EntityFrameworkCore;

namespace BrickwellStore.Data
{
    public class BrickwellContext : DbContext
    {
        public BrickwellContext(DbContextOptions<BrickwellContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
    }
}