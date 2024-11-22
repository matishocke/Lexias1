using Lexias.Services.OrderAPI.Data.Configurations;
using Lexias.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Apply the configurations
            modelBuilder.ApplyConfiguration(new OrderConfiguration());



            //modelBuilder.Entity<Order>().HasData(
            //    new Order
            //    {
            //        OrderId = "order1111",
            //        OrderDate = DateTime.UtcNow.AddDays(-3),
            //        CustomerId = "customer1",
            //        OrderStatus = OrderStatus.Confirmed, // Using enum value directly
            //        TotalAmount = 80.00M
            //    },
            //    new Order
            //    {
            //        OrderId = "order2222",
            //        OrderDate = DateTime.UtcNow.AddDays(-1),
            //        CustomerId = "customer2",
            //        OrderStatus = OrderStatus.Pending, // Using enum value directly
            //        TotalAmount = 60.00M
            //    },
            //    new Order
            //    {
            //        OrderId = "order3333",
            //        OrderDate = DateTime.UtcNow.AddDays(-2),
            //        CustomerId = "customer3",
            //        OrderStatus = OrderStatus.Shipped, // Using enum value directly
            //        TotalAmount = 80.00M
            //    }
            //);

            //// Seed OrderItems - Note: Must reference existing OrderIds
            //modelBuilder.Entity<OrderItem>().HasData(
            //    new OrderItem
            //    {
            //        OrderItemId = "orderitem1",
            //        OrderId = "order1111", // Ensure this matches an OrderId in the Orders seed data
            //        ProductId = "product1",
            //        ProductName = "T-Shirt",
            //        Quantity = 2,
            //        Price = 20.00M
            //    },
            //    new OrderItem
            //    {
            //        OrderItemId = "orderitem2",
            //        OrderId = "order1111",
            //        ProductId = "product2",
            //        ProductName = "Jeans",
            //        Quantity = 1,
            //        Price = 40.00M
            //    },
            //    new OrderItem
            //    {
            //        OrderItemId = "orderitem3",
            //        OrderId = "order2222",
            //        ProductId = "product3",
            //        ProductName = "Jacket",
            //        Quantity = 1,
            //        Price = 60.00M
            //    },
            //    new OrderItem
            //    {
            //        OrderItemId = "orderitem4",
            //        OrderId = "order3333",
            //        ProductId = "product4",
            //        ProductName = "Sneakers",
            //        Quantity = 1,
            //        Price = 50.00M
            //    },
            //    new OrderItem
            //    {
            //        OrderItemId = "orderitem5",
            //        OrderId = "order3333",
            //        ProductId = "product5",
            //        ProductName = "Hat",
            //        Quantity = 3,
            //        Price = 10.00M
            //    }
            //);
        }
    }
}

