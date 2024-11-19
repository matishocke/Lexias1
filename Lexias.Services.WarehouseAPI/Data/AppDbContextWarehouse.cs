using Lexias.Services.WarehouseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;

namespace Lexias.Services.WarehouseAPI.Data
{
    public class AppDbContextWarehouse : DbContext
    {
        public AppDbContextWarehouse(DbContextOptions<AppDbContextWarehouse> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set precision for Price to avoid issues with decimals
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(24, 4); // 18 digits in total, 2 after the decimal point



            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = "product1",
                    Name = "T-Shirt",
                    Description = "Comfortable cotton T-shirt",
                    Price = 20.00M,
                    StockQuantity = 500,
                    Category = "Men",
                    Color = "Red",
                    ItemType = ItemType.Tops
                },
                new Product
                {
                    ProductId = "product2",
                    Name = "Jeans",
                    Description = "Slim fit blue jeans",
                    Price = 40.00M,
                    StockQuantity = 300,
                    Category = "Men",
                    Color = "Blue",
                    ItemType = ItemType.Bottoms
                },
                new Product
                {
                    ProductId = "product3",
                    Name = "Jacket",
                    Description = "Warm winter jacket",
                    Price = 60.00M,
                    StockQuantity = 600,
                    Category = "Women",
                    Color = "Black",
                    ItemType = ItemType.Outerwear
                }
            );
        }
    }
}
