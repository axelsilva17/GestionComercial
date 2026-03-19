using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditoriaLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditoriaLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTabla = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegistroId = table.Column<int>(type: "int", nullable: false),
                    TipoOperacion = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    NombreUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaOperacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValoresAnteriores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValoresNuevos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Workstation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdEmpresa = table.Column<int>(type: "int", nullable: true),
                    IdSucursal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriaLogs_Empresa_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditoriaLogs_Sucursal_IdSucursal",
                        column: x => x.IdSucursal,
                        principalTable: "Sucursal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditoriaLogs_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TablasAuditadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTabla = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Habilitada = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CamposExcluidos = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TablasAuditadas", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1609));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1620));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1624));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1627));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1631));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1634));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1309));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1311));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1313));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1314));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1316));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1319));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1321));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1322));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1462));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1465));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1466));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1468));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1469));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1471));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2096));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2099));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2111));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2113));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2116));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2118));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2123));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2125));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2128));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2130));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2132));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1138));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2005));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2008));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2009));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2011));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2012));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2013));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2015));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2016));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2017));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2019));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2020));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2021));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2023));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2024));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2025));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2027));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2028));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2029));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2030));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2032));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2033));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2034));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2036));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2037));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2038));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2040));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2041));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(2043));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(830));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(832));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(833));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(835));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(836));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(837));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(838));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(840));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(841));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(843));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(844));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(845));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(846));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(848));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(849));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1520));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1525));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1528));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1531));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1534));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1537));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1539));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1542));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1549));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1552));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1555));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1558));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1560));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1563));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1566));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1409));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1426));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(386));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(398));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(400));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(933));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(935));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(936));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(936));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(945));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(968));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(969));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(969));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(970));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(971));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(971));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(972));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(973));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(973));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(974));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1022));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1024));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1024));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1025));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1026));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1026));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1027));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1028));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1028));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1029));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1030));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1030));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1031));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1032));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1078));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1080));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1081));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1083));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1084));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1085));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1086));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1182));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(766));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(768));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(705));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(707));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(709));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(711));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1220));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1696));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1703));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1706));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1710));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1713));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1716));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1719));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1722));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1725));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1728));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1731));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1734));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1738));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1741));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1744));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1747));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1750));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1753));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1756));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1759));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1762));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1765));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1768));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1771));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1774));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1777));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1780));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 1, 18, 0, 281, DateTimeKind.Local).AddTicks(1783));

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_FechaOperacion",
                table: "AuditoriaLogs",
                column: "FechaOperacion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_IdEmpresa",
                table: "AuditoriaLogs",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_IdSucursal",
                table: "AuditoriaLogs",
                column: "IdSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_IdUsuario",
                table: "AuditoriaLogs",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLogs_NombreTabla_RegistroId",
                table: "AuditoriaLogs",
                columns: new[] { "NombreTabla", "RegistroId" });

            migrationBuilder.CreateIndex(
                name: "IX_TablasAuditadas_NombreTabla",
                table: "TablasAuditadas",
                column: "NombreTabla",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaLogs");

            migrationBuilder.DropTable(
                name: "TablasAuditadas");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9344));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9355));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9358));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9361));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9365));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9368));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9075));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9078));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9080));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9081));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9082));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9084));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9085));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9087));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9088));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9204));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9206));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9208));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9209));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9211));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9212));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9780));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9785));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9787));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9792));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8944));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9690));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9693));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9694));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9697));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9698));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9699));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9702));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9704));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9719));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9720));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8720));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8722));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8723));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8724));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8725));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8727));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8728));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8729));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8730));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8731));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8733));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8734));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8735));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8736));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8737));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9251));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9257));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9266));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9269));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9272));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9274));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9282));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9285));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9292));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9295));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9298));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9174));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9175));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8455));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8470));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8472));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8802));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8804));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8804));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8805));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8806));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8806));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8807));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8807));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8808));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8809));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8809));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8810));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8811));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8811));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8812));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8848));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8850));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8853));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8855));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8857));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8886));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8887));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8888));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8889));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8890));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8892));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8978));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8681));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8682));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8641));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8643));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8644));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(8645));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9007));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9410));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9415));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9418));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9421));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9424));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9426));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9429));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9432));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9435));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9449));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9455));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9463));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9466));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9470));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9473));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9477));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9480));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9488));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9491));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9494));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9497));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9501));
        }
    }
}
