using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddDescuentoConfiguracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.CreateTable(
                name: "DescuentoConfiguracion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    ModoDescuento = table.Column<int>(type: "INTEGER", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id_producto = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_categoria = table.Column<int>(type: "INTEGER", nullable: true),
                    Id_empresa = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FechaHasta = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Prioridad = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescuentoConfiguracion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DescuentoConfiguracion_Categoria_Id_categoria",
                        column: x => x.Id_categoria,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DescuentoConfiguracion_Empresa_Id_empresa",
                        column: x => x.Id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DescuentoConfiguracion_Producto_Id_producto",
                        column: x => x.Id_producto,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6533));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6546));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6553));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6559));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6565));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6572));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6142));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6145));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6147));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6150));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6153));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6155));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6158));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6160));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6163));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6316));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6320));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6325));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6328));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6331));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7222));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7237));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7244));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7248));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7252));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7255));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7259));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7263));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7266));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5999));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7092));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7096));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7098));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7100));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7102));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7103));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7105));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7107));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7109));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7111));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7112));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7114));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7116));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7125));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7140));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7142));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(7144));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5611));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5613));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5615));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5618));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5631));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5651));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5654));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5656));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5659));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5661));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5663));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5665));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5667));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5669));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5672));

            migrationBuilder.InsertData(
                table: "Permiso",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[] { 16, true, "Ver descuentos", new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5674), "Descuentos.Ver" });

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6387));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6400));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6405));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6410));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6414));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6423));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6428));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6447));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6452));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6456));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6461));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6465));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6470));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6474));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6267));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6271));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6274));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6277));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5252));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5257));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5259));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5758));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5760));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5762));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5763));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5764));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5766));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5767));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5769));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5770));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5773));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5774));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5776));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5777));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5778));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5780), 16, 1 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5832), 1 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5834), 2 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5836), 3 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5837), 4 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5839), 5 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5840), 6 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5842), 7 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5843), 8 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5844), 9 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5846), 10 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5847), 11 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5848), 12 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5850), 13 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5851), 14, 2 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5853), 15, 2 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5905), 16, 2 });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "Id", "Activo", "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[,]
                {
                    { 37, true, new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5937), 1, 3 },
                    { 38, true, new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5939), 2, 3 },
                    { 39, true, new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5941), 6, 3 },
                    { 40, true, new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5943), 9, 3 },
                    { 41, true, new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5945), 10, 3 },
                    { 42, true, new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5948), 12, 3 },
                    { 43, true, new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5950), 13, 3 }
                });

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6046));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5554));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5557));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5559));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5497));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5499));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5501));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(5503));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6643));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6652));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6658));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6669));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6675));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6680));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6684));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6689));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6694));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6699));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6704));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6709));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6714));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6719));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6723));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6728));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6733));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6738));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6743));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6747));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6752));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6757));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6762));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6767));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6781));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 13, 20, 56, 39, 479, DateTimeKind.Local).AddTicks(6786));

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Activo",
                table: "DescuentoConfiguracion",
                column: "Activo");

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Id_categoria",
                table: "DescuentoConfiguracion",
                column: "Id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Id_empresa",
                table: "DescuentoConfiguracion",
                column: "Id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Id_empresa_Tipo_Activo",
                table: "DescuentoConfiguracion",
                columns: new[] { "Id_empresa", "Tipo", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Id_producto",
                table: "DescuentoConfiguracion",
                column: "Id_producto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescuentoConfiguracion");

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7123));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7140));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7150));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7154));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6763));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6765));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6767));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6769));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6771));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6773));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6775));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6777));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6931));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6934));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6938));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6940));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6943));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7693));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7697));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7709));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7712));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7721));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7724));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7727));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7730));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7733));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7736));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7742));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6644));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7600));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7605));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7606));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7608));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7610));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7611));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7613));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7614));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7616));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7617));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7619));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7620));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7621));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7623));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7624));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7626));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7627));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7629));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7630));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7632));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7633));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7635));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7636));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7638));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7639));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7640));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7642));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6368));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6370));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6372));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6385));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6399));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6401));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6403));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6404));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6406));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6408));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6409));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6411));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6413));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6414));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6984));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7012));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7015));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7019));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7022));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7026));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7029));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7033));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7067));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7070));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7074));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6855));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6858));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6086));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6089));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6091));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6487));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6490));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6491));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6492));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6493));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6496));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6497));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6498));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6499));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6500));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6501));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6502));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6503));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6505));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6542), 1, 2 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6544), 2 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6545), 3 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6547), 4 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6548), 5 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6549), 6 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6550), 7 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6551), 8 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6552), 9 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6553), 10 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6554), 11 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6555), 12 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6557), 13 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "FechaAlta", "Id_permiso" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6558), 14 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6587), 1, 3 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6588), 2, 3 });

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[] { new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6590), 6, 3 });

            migrationBuilder.InsertData(
                table: "RolPermiso",
                columns: new[] { "Id", "Activo", "FechaAlta", "Id_permiso", "Id_rol" },
                values: new object[,]
                {
                    { 33, true, new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6591), 9, 3 },
                    { 34, true, new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6593), 10, 3 },
                    { 35, true, new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6602), 12, 3 },
                    { 36, true, new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6604), 13, 3 }
                });

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6683));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6329));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6331));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6333));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6282));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6284));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6285));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(6287));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7213));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7222));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7226));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7230));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7234));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7238));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7248));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7253));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7257));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7261));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7265));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7269));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7273));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7277));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7282));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7294));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7302));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7314));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7319));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7323));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7327));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7331));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 7, 11, 17, 43, 1, 538, DateTimeKind.Local).AddTicks(7335));
        }
    }
}
