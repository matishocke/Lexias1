using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lexias.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class WithourSeeding2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: "orderitem1");

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: "orderitem2");

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: "orderitem3");

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: "orderitem4");

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: "orderitem5");

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: "order1111");

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: "order2222");

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: "order3333");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "OrderDate", "OrderStatus", "TotalAmount" },
                values: new object[,]
                {
                    { "order1111", "customer1", new DateTime(2024, 11, 16, 13, 14, 48, 280, DateTimeKind.Utc).AddTicks(2066), 5, 80.00m },
                    { "order2222", "customer2", new DateTime(2024, 11, 18, 13, 14, 48, 280, DateTimeKind.Utc).AddTicks(3154), 0, 60.00m },
                    { "order3333", "customer3", new DateTime(2024, 11, 17, 13, 14, 48, 280, DateTimeKind.Utc).AddTicks(3161), 6, 80.00m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "Price", "ProductId", "ProductName", "Quantity" },
                values: new object[,]
                {
                    { "orderitem1", "order1111", 20.00m, "product1", "T-Shirt", 2 },
                    { "orderitem2", "order1111", 40.00m, "product2", "Jeans", 1 },
                    { "orderitem3", "order2222", 60.00m, "product3", "Jacket", 1 },
                    { "orderitem4", "order3333", 50.00m, "product4", "Sneakers", 1 },
                    { "orderitem5", "order3333", 10.00m, "product5", "Hat", 3 }
                });
        }
    }
}
