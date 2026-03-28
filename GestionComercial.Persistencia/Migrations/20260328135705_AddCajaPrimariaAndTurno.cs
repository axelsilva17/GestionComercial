using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddCajaPrimariaAndTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1821));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1831));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1835));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1839));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1843));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1847));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1518));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1520));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1522));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1523));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1524));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1526));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1527));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1529));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1531));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1656));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1658));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1660));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1661));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1663));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1664));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2338));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2345));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2353));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2355));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2357));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2360));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2362));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2364));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2367));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1335));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2239));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2242));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2243));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2245));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2246));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2248));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2249));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2250));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2251));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2253));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2254));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2255));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2257));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2258));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2259));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2261));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2262));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2263));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2265));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2266));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2267));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2268));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2270));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2271));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2272));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2274));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2275));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2276));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1035));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1037));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1038));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1040));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1041));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1042));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1044));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1045));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1046));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1048));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1049));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1050));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1051));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1052));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1054));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1714));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1720));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1723));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1726));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1728));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1731));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1734));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1737));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1755));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1758));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1761));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1764));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1766));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1769));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1772));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1612));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1615));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1616));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1618));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(693));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(707));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(708));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1157));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1161));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1170));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1184));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1185));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1186));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1187));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1187));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1188));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1189));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1189));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1190));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1190));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1237));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1239));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1240));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1241));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1241));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1242));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1244));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1245));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1245));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1246));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1247));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1247));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1282));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1283));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1285));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1286));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1287));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1288));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1289));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1383));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(951));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(952));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(953));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(895));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(897));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(898));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(899));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1419));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1422));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1903));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1909));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1912));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1916));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1919));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1925));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1928));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1932));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1935));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1938));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1941));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1950));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1954));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1957));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1961));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1964));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1967));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1970));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1973));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1976));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1982));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1985));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1988));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1991));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1995));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(1998));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 10, 57, 4, 847, DateTimeKind.Local).AddTicks(2001));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4836));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4847));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4851));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4854));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4858));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4862));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4543));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4546));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4548));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4549));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4551));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4552));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4555));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4557));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4677));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4679));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4681));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4688));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4689));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5361));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5364));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5373));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5377));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5385));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5390));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5393));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4368));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5257));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5260));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5261));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5263));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5264));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5265));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5267));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5268));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5269));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5271));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5272));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5274));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5275));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5276));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5278));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5279));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5280));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5281));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5283));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5284));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5285));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5286));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5288));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5289));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5290));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5292));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5294));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4100));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4102));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4103));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4104));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4106));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4107));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4108));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4109));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4111));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4112));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4113));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4114));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4115));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4117));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4118));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4740));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4746));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4749));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4752));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4754));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4759));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4762));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4774));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4777));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4780));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4783));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4786));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4788));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4635));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4638));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4640));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(3763));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(3776));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(3777));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4197));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4199));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4212));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4230));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4231));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4231));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4232));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4233));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4233));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4234));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4234));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4235));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4236));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4276));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4278));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4279));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4279));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4280));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4281));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4281));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4282));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4283));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4283));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4284));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4285));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4285));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4286));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4322));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4323));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4324));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4325));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4327));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4328));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4329));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4410));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4055));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4057));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4058));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4007));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4009));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4010));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4011));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4465));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4468));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4470));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4924));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4940));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4944));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4947));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4950));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4953));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4956));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4960));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4963));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4966));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4969));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4972));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4979));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4982));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4985));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4988));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4991));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4994));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4997));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5004));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5016));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5019));
        }
    }
}
