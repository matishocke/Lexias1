using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lexias.Services.PaymentAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedIncluded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "OrderId", "PaymentDate", "Status" },
                values: new object[,]
                {
                    { "payment1", 50.00m, "order1111", new DateTime(2024, 11, 9, 13, 45, 47, 439, DateTimeKind.Utc).AddTicks(7235), 1 },
                    { "payment2", 100.00m, "order2222", new DateTime(2024, 11, 14, 13, 45, 47, 439, DateTimeKind.Utc).AddTicks(8051), 0 },
                    { "payment3", 75.00m, "order3333", new DateTime(2024, 11, 17, 13, 45, 47, 439, DateTimeKind.Utc).AddTicks(8057), 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
