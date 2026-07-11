using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddUmbralStockCritico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UmbralStockCritico",
                table: "Empresa",
                type: "INTEGER",
                nullable: false,
                defaultValue: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UmbralStockCritico",
                table: "Empresa");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5357));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5402));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5407));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5413));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5418));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4996));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5001));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5003));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5009));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5020));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5163));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5166));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5169));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5171));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5174));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5176));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5982));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5986));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5998));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6005));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6012));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6015));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6018));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6021));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6024));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(6027));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4803));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5873));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5876));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5878));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5880));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5882));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5890));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5892));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5893));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5895));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5897));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5898));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5900));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5902));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5903));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5905));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5907));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5910));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5912));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5913));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5915));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5916));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5918));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5920));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5921));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5923));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5924));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5926));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4501));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4503));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4507));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4523));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4540));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4542));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4544));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4546));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4548));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4550));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4552));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4556));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4557));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5217));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5234));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5238));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5241));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5245));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5249));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5252));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5256));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5277));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5281));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5285));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5289));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5297));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5301));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5122));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5127));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5130));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4113));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4116));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4118));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4644));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4645));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4646));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4648));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4649));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4650));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4652));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4653));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4654));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4656));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4657));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4658));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4659));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4661));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4698));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4700));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4702));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4703));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4704));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4705));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4707));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4708));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4709));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4711));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4712));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4713));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4715));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4716));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4748));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4750));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4752));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4754));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4755));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4759));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4841));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4462));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4465));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4413));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4418));

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Activo", "Apellido", "BloqueadoHasta", "Email", "FechaAlta", "Id_rol", "Id_sucursal", "IntentosFallidos", "Nombre", "PasswordHash", "PreguntaSecreta", "RespuestaHash", "UltimoAcceso" },
                values: new object[,]
                {
                    { 1, true, "Sistema", null, "admin@miempresa.com", new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4879), 2, 1, 0, "Administrador", "$2a$12$1afFAY7Q1dY9UOpV5EboqOM9P1IO41RZz4F01zEqC918SeOU0qaRy", null, null, null },
                    { 2, true, "General", null, "gerente@miempresa.com", new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4906), 1, 1, 0, "Gerente", "$2a$12$WDfsTRGXXJUgkt/2ETwMb.IHj4pLKMzg.uthmji/O8u7QFzv.kpw2", null, null, null },
                    { 3, true, "Sistema", null, "vendedor@miempresa.com", new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(4914), 3, 1, 0, "Vendedor", "$2a$12$zSKdkIO6EG0FVBmfvc8L9uSkHz35E5ENyP7v/JSElqAAAXuSZtnLG", null, null, null }
                });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5483));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5492));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5496));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5505));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5509));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5513));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5518));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5522));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5530));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5535));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5539));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5543));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5547));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5552));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5556));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5560));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5564));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5568));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5578));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5582));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5587));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5591));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5595));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5599));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5604));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 6, 10, 23, 51, 15, 499, DateTimeKind.Local).AddTicks(5608));
        }
    }
}
