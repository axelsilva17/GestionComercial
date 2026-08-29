using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddBackupConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackupConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Frecuencia = table.Column<int>(type: "INTEGER", nullable: false),
                    DiaSemana = table.Column<int>(type: "INTEGER", nullable: true),
                    HoraProgramada = table.Column<string>(type: "TEXT", nullable: true),
                    MaxBackups = table.Column<int>(type: "INTEGER", nullable: false),
                    CarpetaDestino = table.Column<string>(type: "TEXT", nullable: false),
                    UltimoBackup = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupConfig", x => x.Id);
                });

            // Seed default config row
            migrationBuilder.InsertData(
                table: "BackupConfig",
                columns: new[] { "FechaAlta", "Activo", "Frecuencia", "MaxBackups", "CarpetaDestino" },
                values: new object[] { new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Local), true, 0, 10, "" });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1944));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1957));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1962));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1967));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1972));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1976));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1619));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1621));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1623));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1625));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1627));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1629));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1631));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1633));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1636));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1753));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1756));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1758));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1760));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1762));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1764));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2517));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2521));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2529));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2533));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2535));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2546));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2549));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2551));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2554));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2557));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2560));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2563));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1465));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2422));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2426));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2427));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2429));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2430));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2432));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2433));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2435));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2437));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2439));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2440));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2442));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2443));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2444));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2447));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2449));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2452));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2453));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2456));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2457));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2459));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2462));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1158));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1162));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1173));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1195));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1197));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1199));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1201));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1202));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1204));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1206));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1208));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1209));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1213));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1807));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1822));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1826));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1829));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1844));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1847));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1851));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1875));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1879));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1883));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1886));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1890));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1894));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1898));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1714));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1717));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1719));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1721));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(790));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(805));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(807));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1284));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1286));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1287));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1288));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1289));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1290));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1291));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1293));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1294));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1295));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1296));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1297));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1298));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1299));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1337));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1339));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1340));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1342));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1343));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1344));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1345));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1346));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1347));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1348));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1349));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1352));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1353));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1354));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1382));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1403));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1405));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1406));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1408));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1409));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1411));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1412));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1504));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1113));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1115));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1117));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1062));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1064));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1066));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(1067));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "admin@miempresa.com");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "vendedor@miempresa.com");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "gerente@miempresa.com");

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2041));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2049));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2053));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2058));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2062));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2066));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2069));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2080));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2084));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2088));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2092));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2096));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2100));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2104));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2107));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2111));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2115));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2123));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2127));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2135));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2139));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2143));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2147));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2151));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2155));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 23, 41, 4, 157, DateTimeKind.Local).AddTicks(2159));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackupConfig");

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9401));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9409));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9414));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9419));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9423));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9063));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9065));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9067));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9069));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9072));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9073));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9076));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9078));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9080));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9204));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9206));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9209));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9211));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9213));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9988));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9993));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(4));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(7));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(10));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(14));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(17));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(23));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 420, DateTimeKind.Local).AddTicks(29));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9897));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9899));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9902));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9903));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9905));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9907));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9908));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9909));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9911));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9913));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9914));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9916));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9917));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9919));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9920));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9922));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9923));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9924));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9926));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9927));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9929));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8589));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8591));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8593));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8595));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8613));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8633));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8635));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8637));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8639));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8641));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8643));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8645));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8646));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8648));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8650));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8651));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9260));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9279));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9286));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9300));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9316));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9321));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9325));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9329));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9337));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9341));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9161));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9164));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8059));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8062));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8064));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8734));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8737));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8738));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8739));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8741));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8742));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8743));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8745));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8746));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8747));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8749));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8750));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8751));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8752));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8754));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8755));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8797));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8801));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8802));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8805));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8806));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8807));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8809));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8810));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8811));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8813));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8814));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8815));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8817));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8850));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8873));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8875));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8877));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8879));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8880));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8882));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8884));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8972));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8540));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8542));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8544));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8480));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8483));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8484));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(8486));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "admin@demo.com");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "vendedor@demo.com");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "gerente@demo.com");

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9493));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9498));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9502));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9506));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9511));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9515));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9519));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9523));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9527));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9531));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9540));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9545));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9581));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9585));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9589));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9593));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9597));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9601));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 28, 0, 23, 56, 419, DateTimeKind.Local).AddTicks(9605));
        }
    }
}
