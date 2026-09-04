using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIndicesCompuestosProductoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cliente_Nombre",
                table: "Cliente");

            migrationBuilder.RenameIndex(
                name: "IX_Producto_IdEmpresa",
                table: "Producto",
                newName: "IX_Producto_Id_empresa");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_IdEmpresa",
                table: "Cliente",
                newName: "IX_Cliente_Id_empresa");

            // ── Composite indexes via raw SQL (single source of truth: migrations) ──
            // Covers ObtenerPorEmpresaPaginadoAsync: Where(Id_empresa) + Where(Activo) + OrderBy(Nombre)
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Producto_Empresa_Activo_Nombre
                ON Producto(Id_empresa, Activo, Nombre);
            ");

            // Same pattern for Cliente: Where(Id_empresa) + Where(Activo) + OrderBy(Nombre)
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Cliente_Empresa_Activo_Nombre
                ON Cliente(Id_empresa, Activo, Nombre);
            ");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5187));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5199));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5204));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5208));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5213));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5217));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4895));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4896));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4898));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4902));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4904));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4906));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5034));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5037));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5039));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5041));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5043));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5718));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5722));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5725));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5728));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5730));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5733));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5736));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5739));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5741));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5744));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5747));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5750));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4769));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5625));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5628));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5629));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5630));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5632));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5633));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5634));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5635));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5637));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5638));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5639));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5640));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5642));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5643));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5644));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5646));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5647));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5648));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5650));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5651));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5652));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5654));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5655));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5656));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5658));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5659));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5660));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5662));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4484));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4486));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4487));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4489));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4490));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4492));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4494));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4495));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4497));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4499));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4500));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4502));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4503));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4505));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4506));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4508));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4510));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5083));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5098));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5101));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5105));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5108));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5112));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5119));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5122));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5129));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5132));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5136));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5143));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4987));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4990));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4993));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4995));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(3995));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(3997));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(3999));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4591));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4593));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4594));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4595));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4597));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4598));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4599));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4600));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4601));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4603));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4604));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4605));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4606));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4607));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4609));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4648));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4650));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4651));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4652));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4653));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4654));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4655));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4656));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4658));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4659));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4660));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4661));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4662));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4663));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4664));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4693));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4713));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4720));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4722));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4724));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4725));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4726));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4728));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4812));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4441));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4443));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4444));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4392));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4394));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4396));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(4397));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5274));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5281));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5285));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5289));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5298));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5302));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5306));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5309));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5313));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5318));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5322));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5329));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5333));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5337));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5340));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5344));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5348));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5352));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5355));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5359));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5363));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5366));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5374));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5378));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5381));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 4, 1, 3, 33, 628, DateTimeKind.Local).AddTicks(5385));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the raw SQL composite indexes
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Producto_Empresa_Activo_Nombre;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Cliente_Empresa_Activo_Nombre;");

            // Re-create IX_Cliente_Nombre (dropped by Up)
            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Nombre",
                table: "Cliente",
                column: "Nombre");

            migrationBuilder.RenameIndex(
                name: "IX_Producto_Id_empresa",
                table: "Producto",
                newName: "IX_Producto_IdEmpresa");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_Id_empresa",
                table: "Cliente",
                newName: "IX_Cliente_IdEmpresa");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3606));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3624));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3629));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3635));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3639));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3293));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3296));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3298));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3300));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3301));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3303));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3305));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3308));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3310));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3426));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3429));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3431));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3433));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3435));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3437));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4178));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4182));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4193));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4196));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4199));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4202));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4206));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4209));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4212));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4215));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4218));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4221));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3165));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4082));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4086));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4088));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4089));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4091));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4092));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4094));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4095));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4097));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4099));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4100));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4102));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4103));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4105));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4106));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4108));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4109));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4111));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4112));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4114));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4115));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4117));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4118));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4120));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4121));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4123));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4124));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(4126));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2855));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2857));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2859));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2860));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2870));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2882));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2884));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2885));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2887));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2897));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2899));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2901));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2903));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2904));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2906));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2908));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2910));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3475));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3488));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3492));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3495));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3499));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3503));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3506));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3510));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3531));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3535));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3539));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3549));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3553));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3557));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3560));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3388));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3391));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3393));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3395));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2404));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2408));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2984));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2986));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2988));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2989));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2990));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2991));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2992));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2993));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2994));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2996));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2997));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2998));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2999));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3000));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3001));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3002));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3043));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3045));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3047));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3048));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3049));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3050));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3051));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3052));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3053));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3054));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3055));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3057));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3058));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3059));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3091));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3114));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3116));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3117));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3119));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3121));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3122));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3124));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3205));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2813));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2815));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2816));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2769));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2771));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2772));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2774));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3696));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3708));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3712));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3716));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3727));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3731));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3735));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3739));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3743));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3747));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3751));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3755));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3767));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3771));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3775));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3779));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3783));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3787));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3791));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3795));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3799));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3803));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3807));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3811));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3815));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(3819));

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Nombre",
                table: "Cliente",
                column: "Nombre");
        }
    }
}
