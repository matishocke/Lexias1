using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexias.Services.WarehouseAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataDecimalFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(24,4)",
                precision: 24,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,4)",
                oldPrecision: 24,
                oldScale: 4,
                oldNullable: true);
        }
    }
}
