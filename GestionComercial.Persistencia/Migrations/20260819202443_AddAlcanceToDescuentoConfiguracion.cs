using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddAlcanceToDescuentoConfiguracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Alcance",
                table: "DescuentoConfiguracion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            // Backfill: set Alcance based on existing FK columns
            migrationBuilder.Sql(@"
                UPDATE ""DescuentoConfiguracion"" SET ""Alcance"" = 1 WHERE ""Id_producto"" IS NOT NULL;
                UPDATE ""DescuentoConfiguracion"" SET ""Alcance"" = 2 WHERE ""Id_categoria"" IS NOT NULL AND ""Id_producto"" IS NULL;
            ");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alcance",
                table: "DescuentoConfiguracion");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2300));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2313));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2317));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2322));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2327));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2332));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1963));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1965));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1967));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1969));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1971));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1979));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1981));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1983));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1985));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2107));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2110));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2112));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2114));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2116));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2874));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2878));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2884));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2887));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2890));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2892));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2895));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2898));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2901));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2904));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2907));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2910));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1844));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2777));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2780));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2782));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2783));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2785));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2786));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2787));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2789));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2790));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2792));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2793));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2795));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2796));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2798));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2799));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2800));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2802));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2803));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2805));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2806));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2807));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2809));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2810));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2812));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2813));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2814));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2816));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2817));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1520));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1522));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1524));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1525));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1542));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1562));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1564));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1566));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1568));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1569));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1571));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1573));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1574));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1576));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1578));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1587));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2161));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2176));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2180));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2183));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2224));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2228));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2232));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2236));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2239));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2243));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2252));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2067));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2070));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2073));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2075));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1050));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1053));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1054));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1659));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1661));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1662));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1663));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1665));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1666));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1667));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1668));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1669));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1670));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1671));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1672));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1673));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1675));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1676));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1677));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1718));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1720));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1722));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1723));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1724));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1725));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1726));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1728));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1729));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1730));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1731));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1732));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1733));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1734));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1735));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1771));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1794));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1796));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1798));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1799));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1801));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1802));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1804));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1882));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1474));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1476));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1478));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1426));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1427));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(1429));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2397));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2401));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2410));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2414));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2418));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2422));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2426));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2430));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2434));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2438));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2442));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2458));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2472));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2480));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2483));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2487));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2491));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2495));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2499));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2503));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 18, 14, 51, 30, 682, DateTimeKind.Local).AddTicks(2507));
        }
    }
}
