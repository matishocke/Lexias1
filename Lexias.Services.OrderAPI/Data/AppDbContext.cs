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
        public DbSet<OrderItem> OrderItems { get; set; } // Separate table for OrderItems

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


           


            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = "order1111",
                    OrderDate = DateTime.UtcNow.AddDays(-3),
                    OrderItemsList = new List<OrderItem>
                    {
                        new OrderItem { ProductId = "product1", ProductName = "T-Shirt", Quantity = 2, Price = 20.00M },
                        new OrderItem { ProductId = "product2", ProductName = "Jeans", Quantity = 1, Price = 40.00M }
                    },
                    Customer = new Customer
                    {
                        CustomerId = "customer1",
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        Address = "123 Street, City, Country",
                        PhoneNumber = "123456789"
                    },
                    TotalAmount = 80.00M,
                    OrderStatus = OrderStatus.Confirmed
                },
                new Order
                {
                    OrderId = "order2222",
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    OrderItemsList = new List<OrderItem>
                    {
                        new OrderItem { ProductId = "product3", ProductName = "Jacket", Quantity = 1, Price = 60.00M }
                    },
                    Customer = new Customer
                    {
                        CustomerId = "customer2",
                        Name = "Jane Smith",
                        Email = "jane.smith@example.com",
                        Address = "456 Avenue, City, Country",
                        PhoneNumber = "987654321"
                    },
                    TotalAmount = 60.00M,
                    OrderStatus = OrderStatus.Pending
                },
                new Order
                {
                    OrderId = "order3333",
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    OrderItemsList = new List<OrderItem>
                    {
                        new OrderItem { ProductId = "product4", ProductName = "Sneakers", Quantity = 1, Price = 50.00M },
                        new OrderItem { ProductId = "product5", ProductName = "Hat", Quantity = 3, Price = 10.00M }
                    },
                    Customer = new Customer
                    {
                        CustomerId = "customer3",
                        Name = "Michael Johnson",
                        Email = "michael.johnson@example.com",
                        Address = "789 Road, City, Country",
                        PhoneNumber = "192837465"
                    },
                    TotalAmount = 80.00M,
                    OrderStatus = OrderStatus.Shipped
                }
            );

        }
    }
}
