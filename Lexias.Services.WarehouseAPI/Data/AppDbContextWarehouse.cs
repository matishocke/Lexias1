using Lexias.Services.WarehouseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Lexias.Services.WarehouseAPI.Data
{
    public class AppDbContextWarehouse : DbContext
    {
        public AppDbContextWarehouse(DbContextOptions<AppDbContextWarehouse> options) : base(options)
        {
        }
        public DbSet<Product> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = "product1",
                    Name = "T-Shirt",
                    Description = "Comfortable cotton T-shirt",
                    Price = 20.00M,
                    StockQuantity = 500,
                    Category = "Men",
                    Sizes = new List<string> { "S", "M", "L", "XL" },
                    Color = "Red",
                    Images = new List<string> { "tshirt1.jpg", "tshirt2.jpg" }
                },
                new Product
                {
                    ProductId = "product2",
                    Name = "Jeans",
                    Description = "Slim fit blue jeans",
                    Price = 40.00M,
                    StockQuantity = 300,
                    Category = "Men",
                    Sizes = new List<string> { "M", "L", "XL" },
                    Color = "Blue",
                    Images = new List<string> { "jeans1.jpg", "jeans2.jpg" }
                },
                new Product
                {
                    ProductId = "product3",
                    Name = "Jacket",
                    Description = "Warm winter jacket",
                    Price = 60.00M,
                    StockQuantity = 600,
                    Category = "Women",
                    Sizes = new List<string> { "S", "M", "L" },
                    Color = "Black",
                    Images = new List<string> { "jacket1.jpg", "jacket2.jpg" }
                }
            );

        }
    }
}
