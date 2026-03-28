using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddCajaPrimariaTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsPrimaria",
                table: "Caja",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Turno",
                table: "Caja",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EsPrimaria", "FechaAlta", "Turno" },
                values: new object[] { false, new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9550), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EsPrimaria", "FechaAlta", "Turno" },
                values: new object[] { false, new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9568), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EsPrimaria", "FechaAlta", "Turno" },
                values: new object[] { false, new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9572), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EsPrimaria", "FechaAlta", "Turno" },
                values: new object[] { false, new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9575), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EsPrimaria", "FechaAlta", "Turno" },
                values: new object[] { false, new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9579), null });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EsPrimaria", "FechaAlta", "Turno" },
                values: new object[] { false, new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9583), null });

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9174));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9176));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9177));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9178));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9180));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9181));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9183));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9370));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9372));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9382));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9386));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(115));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(118));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(130));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(133));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(135));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(138));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(140));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(142));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(145));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(147));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(149));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(152));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9001));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(11));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(13));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(15));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(16));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(18));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(19));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(22));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(23));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(24));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(27));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(28));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(30));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(31));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(32));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(34));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(35));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(36));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(37));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(39));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(40));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(41));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(42));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(44));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(45));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(46));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 562, DateTimeKind.Local).AddTicks(47));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8686));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8687));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8689));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8690));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8692));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8693));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8694));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8695));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8696));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8698));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8699));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8700));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8702));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8703));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9442));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9455));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9458));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9463));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9466));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9489));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9492));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9494));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9497));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9500));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9319));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9322));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9323));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9325));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8247));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8249));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8800));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8800));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8801));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8825));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8850));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8853));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8855));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8857));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8897));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8899));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8899));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8901));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8904));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8906));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8907));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8947));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8949));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8950));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8951));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8952));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8953));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8954));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9042));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8628));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8630));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8631));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8577));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8578));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8579));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(8580));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9078));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9081));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9083));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9677));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9680));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9684));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9687));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9691));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9694));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9697));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9719));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9743));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9749));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9755));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 28, 11, 34, 19, 561, DateTimeKind.Local).AddTicks(9759));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsPrimaria",
                table: "Caja");

            migrationBuilder.DropColumn(
                name: "Turno",
                table: "Caja");

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
    }
}
