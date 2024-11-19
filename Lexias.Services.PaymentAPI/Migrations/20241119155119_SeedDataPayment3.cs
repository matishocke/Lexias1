using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexias.Services.PaymentAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataPayment3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(24,6)",
                precision: 24,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,4)",
                oldPrecision: 24,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "payment1",
                column: "PaymentDate",
                value: new DateTime(2024, 11, 9, 15, 51, 18, 690, DateTimeKind.Utc).AddTicks(485));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "payment2",
                column: "PaymentDate",
                value: new DateTime(2024, 11, 14, 15, 51, 18, 690, DateTimeKind.Utc).AddTicks(1318));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "payment3",
                column: "PaymentDate",
                value: new DateTime(2024, 11, 17, 15, 51, 18, 690, DateTimeKind.Utc).AddTicks(1322));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(24,4)",
                precision: 24,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,6)",
                oldPrecision: 24,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "payment1",
                column: "PaymentDate",
                value: new DateTime(2024, 11, 9, 15, 48, 8, 484, DateTimeKind.Utc).AddTicks(6292));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "payment2",
                column: "PaymentDate",
                value: new DateTime(2024, 11, 14, 15, 48, 8, 484, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: "payment3",
                column: "PaymentDate",
                value: new DateTime(2024, 11, 17, 15, 48, 8, 484, DateTimeKind.Utc).AddTicks(7059));
        }
    }
}
