using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEfectivoRecibido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EfectivoRecibido",
                table: "Venta",
                type: "decimal(18,2)",
                nullable: true);

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
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4924) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4929) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4940) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4944) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4947) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4950) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4953) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4956) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4960) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4963) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4966) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4969) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4972) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4975) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4979) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4982) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4985) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4988) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4991) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4994) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(4997) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5000) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5004) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5007) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5010) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5013) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5016) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "EfectivoRecibido", "FechaAlta" },
                values: new object[] { null, new DateTime(2026, 3, 26, 22, 46, 42, 340, DateTimeKind.Local).AddTicks(5019) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EfectivoRecibido",
                table: "Venta");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3429));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3439));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3443));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3447));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3451));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3454));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3127));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3130));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3131));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3133));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3134));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3136));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3137));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3139));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3140));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3259));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3262));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3264));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3265));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3274));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3276));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3935));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3939));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3947));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3949));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3952));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3961));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3963));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3966));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3968));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3971));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3973));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3976));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2967));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3841));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3845));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3847));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3848));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3850));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3851));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3852));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3854));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3855));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3857));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3858));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3860));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3861));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3862));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3864));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3865));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3866));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3868));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3869));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3872));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3873));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3875));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3876));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3877));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3879));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3880));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3882));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2655));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2657));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2658));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2659));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2661));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2662));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2663));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2664));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2666));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2667));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2668));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2670));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2671));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2672));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3324));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3333));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3335));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3338));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3341));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3344));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3347));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3350));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3360));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3363));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3366));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3372));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3375));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3377));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3220));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3223));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3225));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3227));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2288));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2301));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2302));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2755));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2756));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2757));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2758));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2777));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2795));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2796));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2796));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2797));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2798));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2798));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2799));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2800));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2800));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2801));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2838));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2840));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2841));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2841));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2842));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2843));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2843));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2844));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2846));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2847));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2847));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2848));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2914));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2916));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2917));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2918));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2919));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2920));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2921));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3004));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2608));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2610));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2611));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2556));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2558));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2559));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(2561));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3041));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3517));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3523));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3532));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3536));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3539));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3543));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3546));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3549));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3553));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3556));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3559));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3563));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3566));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3569));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3576));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3579));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3583));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3586));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3589));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3593));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3596));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3603));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3606));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3609));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3613));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 20, 0, 0, 34, 676, DateTimeKind.Local).AddTicks(3616));
        }
    }
}
