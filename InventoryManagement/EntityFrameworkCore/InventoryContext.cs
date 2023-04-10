﻿//using Swashbuckle.AspNetCore.SwaggerGen;

namespace InventoryManagement.EntityFrameworkCore
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {



        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}