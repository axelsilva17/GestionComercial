using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9306));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9318));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9323));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9327));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9332));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9337));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8969));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8971));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8973));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8975));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8977));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8979));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8981));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8983));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8985));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9096));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9099));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9101));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9103));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9105));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9108));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9903));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9914));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9922));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9925));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9931));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9934));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9938));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9940));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9944));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9950));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8853));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9829));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9831));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9832));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9833));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9835));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9836));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9838));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9839));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9841));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9842));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9843));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9845));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9846));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8545));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8547));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8549));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8550));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8561));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8577));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8579));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8581));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8583));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8585));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8586));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8590));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8591));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8593));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8595));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9184));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9197));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9204));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9208));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9211));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9235));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9238));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9242));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9245));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9249));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9253));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9257));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9062));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9065));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9067));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8032));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8035));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8674));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8677));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8678));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8679));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8680));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8681));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8683));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8685));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8686));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8687));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8689));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8690));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8692));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8693));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8730));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8732));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8733));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8734));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8735));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8736));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8737));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8739));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8740));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8741));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8742));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8743));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8744));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8745));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8746));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8777));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8798));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8801));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8804));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8806));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8807));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8500));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8502));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8504));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8420));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8422));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8423));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(8425));

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Activo", "Apellido", "BloqueadoHasta", "Email", "FechaAlta", "Id_rol", "Id_sucursal", "IntentosFallidos", "Nombre", "PasswordHash", "PreguntaSecreta", "RespuestaHash", "UltimoAcceso" },
                values: new object[,]
                {
                    { 1, true, "Sistema", null, "admin@demo.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, 0, "Admin", "$2a$10$vZSSeTQhuOMqZQnUDpuO2.cMfnBcqwuzKGUR4jeq5v96n6rD0e13C", null, null, null },
                    { 2, true, "Demo", null, "vendedor@demo.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 1, 0, "Vendedor", "$2a$10$LUIblGp1Yji4FbTt64k3e.e8I5nNfDu4WznoJ1P3DwB6WIU9g246a", null, null, null },
                    { 3, true, "Demo", null, "gerente@demo.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 0, "Gerente", "$2a$10$zd3k2FDRQhwROhoa3URlg.kHvSFnim0Hhi/zukq3hAOth4P/EL83y", null, null, null }
                });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9394));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9402));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9406));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9411));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9414));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9418));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9432));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9436));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9444));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9448));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9464));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9468));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9472));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9475));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9487));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9491));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9495));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9499));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9503));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9508));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9512));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 24, 9, 32, 31, 932, DateTimeKind.Local).AddTicks(9516));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6545));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6560));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6566));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6572));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6579));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6584));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6232));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6234));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6236));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6238));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6240));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6242));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6246));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6248));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6365));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6368));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6370));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6372));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7364));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7399));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7403));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7406));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7409));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7412));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7415));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7418));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7421));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7424));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7427));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6113));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7239));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7244));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7246));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7248));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7249));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7251));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7253));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7255));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7257));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7258));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7260));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7262));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7264));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7266));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7267));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7269));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7271));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7273));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7274));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7276));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7278));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7280));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7281));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7283));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7285));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7288));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5769));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5773));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5775));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5791));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5808));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5810));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5815));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5816));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5818));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5819));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5821));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5823));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5824));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6431));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6435));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6438));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6445));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6449));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6452));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6472));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6475));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6479));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6483));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6487));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6490));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6328));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6331));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6333));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6335));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5344));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5347));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5349));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5904));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5906));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5907));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5909));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5910));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5913));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5914));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5915));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5916));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5918));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5919));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5921));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5922));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5923));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5968));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5971));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5972));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5973));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5975));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5976));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5977));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5979));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5980));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5982));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5983));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5984));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5986));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5988));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6024));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6052));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6054));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6056));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6058));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6060));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6062));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6064));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6151));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5724));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5726));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5728));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5671));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5673));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5674));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(5676));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6661));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6743));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6748));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6753));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6758));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6763));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6768));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6773));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6783));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6788));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6793));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6799));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6806));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6811));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6816));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6821));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6826));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6830));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6845));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6851));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6856));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6865));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6870));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6875));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6880));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 19, 17, 24, 42, 339, DateTimeKind.Local).AddTicks(6885));
        }
    }
}
