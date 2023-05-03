//using Swashbuckle.AspNetCore.SwaggerGen;

using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.PortableExecutable;

namespace InventoryManagement.EntityFrameworkCore
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<InventoryItems> InventoryItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<InventoryItemPriceDetails> InventoryItemPriceDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*builder.Entity<Customer>()
                .HasIndex(u => u.contactNumber)
                .IsUnique(true);
                */

            builder.Entity<Customer>()
                .HasAlternateKey(c => c.contactNumber);

        }
    }
}