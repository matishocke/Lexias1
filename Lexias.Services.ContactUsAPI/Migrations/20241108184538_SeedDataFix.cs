using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lexias.Services.ContactUsAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ContactUs",
                columns: new[] { "Id", "CreatedAt", "Email", "Message", "Name", "Subject" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 7, 18, 45, 38, 450, DateTimeKind.Utc).AddTicks(2359), "john.doe@example.com", "Hello, I would like more information about your services.", "John Doe", "Inquiry about services" },
                    { 2, new DateTime(2024, 11, 6, 18, 45, 38, 450, DateTimeKind.Utc).AddTicks(2365), "jane.smith@example.com", "I'm having an issue with my recent purchase.", "Jane Smith", "Support Request" },
                    { 3, new DateTime(2024, 11, 5, 18, 45, 38, 450, DateTimeKind.Utc).AddTicks(2367), "michael.johnson@example.com", "Could you provide me with the price list?", "Michael Johnson", "General Question" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactUs");
        }
    }
}
