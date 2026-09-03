using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class OptimizacionIndicesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Permiso",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaAlta", "Nombre" },
                values: new object[] { 17, true, "Auditoría de caja", new DateTime(2026, 9, 3, 20, 13, 59, 879, DateTimeKind.Local).AddTicks(2910), "Caja.Auditoria" });

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
                name: "IX_Proveedor_Nombre",
                table: "Proveedor",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Nombre",
                table: "Producto",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Nombre",
                table: "Cliente",
                column: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proveedor_Nombre",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Producto_Nombre",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_Nombre",
                table: "Cliente");

            migrationBuilder.DeleteData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6675));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6690));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6695));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6699));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6704));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6709));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6399));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6401));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6403));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6405));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6409));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6411));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6413));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6415));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6534));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6537));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6539));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6541));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6543));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6545));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7190));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7196));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7199));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7202));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7204));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7207));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7210));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7213));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7215));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7218));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7221));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6287));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7097));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7101));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7102));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7104));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7105));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7106));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7108));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7109));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7110));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7112));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7113));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7114));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7116));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7117));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7119));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7121));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7125));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7134));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                column: "Fecha",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(7136));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6016));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6018));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6020));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6022));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6024));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6025));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6027));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6029));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6030));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6032));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6033));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6035));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6037));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6038));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6040));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6041));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6581));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6592));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6595));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6599));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6602));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6606));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6609));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6612));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6616));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6619));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6622));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6626));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6629));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6633));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6636));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6498));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6500));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6502));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6504));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5752));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5754));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5756));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6112));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6113));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6114));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6116));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6117));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6118));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6119));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6120));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6121));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6123));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6124));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6125));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6126));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6127));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6129));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6167));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6169));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6170));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6171));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6172));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6174));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6175));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6176));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6177));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6178));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6179));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6180));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6181));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6182));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6183));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 37,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6240));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 38,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6242));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 39,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 40,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6245));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 41,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6247));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 42,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6248));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 43,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6250));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6326));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5974));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5976));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5977));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5933));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5935));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5937));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(5938));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6753));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6760));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6764));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6767));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6780));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6784));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6787));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6791));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6795));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6799));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6803));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6806));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6814));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6818));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6821));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6825));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6829));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6833));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6836));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6840));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6844));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6847));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6851));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6855));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6858));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 8, 31, 21, 44, 49, 105, DateTimeKind.Local).AddTicks(6862));
        }
    }
}
