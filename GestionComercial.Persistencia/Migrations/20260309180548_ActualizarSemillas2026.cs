using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarSemillas2026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaAlta", "FechaApertura", "FechaCierre" },
                values: new object[] { new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9344), new DateTime(2025, 10, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaAlta", "FechaApertura", "FechaCierre" },
                values: new object[] { new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9355), new DateTime(2025, 11, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaAlta", "FechaApertura", "FechaCierre" },
                values: new object[] { new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9358), new DateTime(2025, 12, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Caja",
                columns: new[] { "Id", "Activo", "Estado", "FechaAlta", "FechaApertura", "FechaCierre", "Id_sucursal", "MontoFinal", "MontoInicial", "Observacion", "UsuarioApertura_id", "UsuarioCierre_id" },
                values: new object[,]
                {
                    { 4, true, 2, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9361), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 380000m, 50000m, null, 1, 1 },
                    { 5, true, 2, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9365), new DateTime(2026, 2, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 450000m, 50000m, null, 1, 1 },
                    { 6, true, 2, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9368), new DateTime(2026, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 520000m, 50000m, null, 1, 1 }
                });

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
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9780) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9783) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9785) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9787) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9789) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9792) });

            migrationBuilder.InsertData(
                table: "Compra",
                columns: new[] { "Id", "Activo", "Estado", "Fecha", "FechaAlta", "Id_proveedor", "Id_sucursal", "Id_usuario", "Observacion", "Total" },
                values: new object[,]
                {
                    { 7, true, 2, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9794), 3, 1, 1, null, 132000m },
                    { 8, true, 2, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9796), 4, 1, 1, null, 78000m },
                    { 9, true, 2, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9798), 1, 1, 1, null, 158000m },
                    { 10, true, 2, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9801), 2, 1, 1, null, 91000m },
                    { 11, true, 2, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9803), 1, 1, 1, null, 165000m },
                    { 12, true, 2, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9805), 3, 1, 1, null, 88000m }
                });

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
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9410) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9415) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9418) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9421) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9424) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9426) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9429) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9432) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9435) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9445) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9449) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9452) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9455) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9457) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9460) });

            migrationBuilder.InsertData(
                table: "CompraDetalle",
                columns: new[] { "Id", "Cantidad", "Id_compra", "Id_producto", "PrecioCosto", "Subtotal" },
                values: new object[,]
                {
                    { 18, 8m, 7, 1, 5500m, 44000m },
                    { 19, 10m, 7, 3, 4200m, 42000m },
                    { 20, 6m, 7, 4, 8000m, 48000m },
                    { 21, 12m, 8, 11, 3500m, 42000m },
                    { 22, 12m, 8, 12, 2000m, 24000m },
                    { 23, 1m, 9, 5, 58000m, 58000m },
                    { 24, 1m, 9, 6, 50000m, 50000m },
                    { 25, 4m, 9, 7, 12000m, 48000m },
                    { 26, 2m, 10, 8, 28000m, 56000m },
                    { 27, 5m, 10, 9, 6400m, 32000m },
                    { 28, 15m, 11, 1, 5500m, 82500m },
                    { 29, 20m, 11, 2, 2600m, 52000m },
                    { 30, 8m, 11, 3, 4200m, 33600m },
                    { 31, 1m, 12, 5, 58000m, 58000m },
                    { 32, 1m, 12, 10, 21000m, 21000m }
                });

            migrationBuilder.InsertData(
                table: "Venta",
                columns: new[] { "Id", "Activo", "Estado", "Fecha", "FechaAlta", "Id_caja", "Id_cliente", "Id_sucursal", "Id_usuario", "Observacion", "TotalBruto", "TotalDescuento", "TotalFinal" },
                values: new object[,]
                {
                    { 16, true, 2, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9463), 4, 1, 1, 1, null, 38500m, 0m, 38500m },
                    { 17, true, 2, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9466), 4, 6, 1, 1, null, 92000m, 4600m, 87400m },
                    { 18, true, 2, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9470), 4, 3, 1, 1, null, 27300m, 0m, 27300m },
                    { 19, true, 2, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9473), 4, 4, 1, 1, null, 54000m, 0m, 54000m },
                    { 20, true, 2, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9477), 4, 2, 1, 1, null, 18500m, 925m, 17575m },
                    { 21, true, 2, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9480), 5, 6, 1, 1, null, 85000m, 0m, 85000m },
                    { 22, true, 2, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9483), 5, 1, 1, 1, null, 42000m, 2100m, 39900m },
                    { 23, true, 2, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9486), 5, 5, 1, 1, null, 32000m, 0m, 32000m },
                    { 24, true, 2, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9488), 5, 6, 1, 1, null, 72000m, 3600m, 68400m },
                    { 25, true, 2, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9491), 5, 3, 1, 1, null, 16800m, 0m, 16800m },
                    { 26, true, 2, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9494), 6, 4, 1, 1, null, 54800m, 0m, 54800m },
                    { 27, true, 2, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9497), 6, 6, 1, 1, null, 93600m, 4680m, 88920m },
                    { 28, true, 2, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9501), 6, 2, 1, 1, null, 25700m, 0m, 25700m }
                });

            migrationBuilder.InsertData(
                table: "Pago",
                columns: new[] { "Id", "Fecha", "Id_metodoPago", "Id_venta", "Monto" },
                values: new object[,]
                {
                    { 16, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9710), 1, 16, 38500m },
                    { 17, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9712), 3, 17, 87400m },
                    { 18, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9713), 2, 18, 27300m },
                    { 19, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9714), 4, 19, 54000m },
                    { 20, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9715), 1, 20, 17575m },
                    { 21, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9716), 3, 21, 85000m },
                    { 22, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9718), 1, 22, 39900m },
                    { 23, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9719), 2, 23, 32000m },
                    { 24, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9720), 4, 24, 68400m },
                    { 25, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9721), 1, 25, 16800m },
                    { 26, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9722), 5, 26, 54800m },
                    { 27, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9724), 3, 27, 88920m },
                    { 28, new DateTime(2026, 3, 9, 15, 5, 47, 344, DateTimeKind.Local).AddTicks(9725), 1, 28, 25700m }
                });

            migrationBuilder.InsertData(
                table: "VentaDetalle",
                columns: new[] { "Id", "Cantidad", "CostoUnitario", "Descuento", "Id_producto", "Id_venta", "MargenUnitario", "PrecioUnitario", "Subtotal" },
                values: new object[,]
                {
                    { 28, 2m, 5500m, 0m, 1, 16, 3000m, 8500m, 17000m },
                    { 29, 3m, 2600m, 0m, 2, 16, 1600m, 4200m, 12600m },
                    { 30, 1m, 4200m, 0m, 3, 16, 2600m, 6800m, 6800m },
                    { 31, 1m, 58000m, 4600m, 5, 17, 22400m, 85000m, 80400m },
                    { 32, 1m, 12000m, 0m, 7, 18, 6500m, 18500m, 18500m },
                    { 33, 2m, 3500m, 0m, 11, 18, 2000m, 5500m, 11000m },
                    { 34, 1m, 28000m, 0m, 8, 19, 14000m, 42000m, 42000m },
                    { 35, 1m, 50000m, 0m, 6, 19, 22000m, 72000m, 72000m },
                    { 36, 1m, 12000m, 925m, 7, 20, 5575m, 18500m, 17575m },
                    { 37, 1m, 58000m, 0m, 5, 21, 27000m, 85000m, 85000m },
                    { 38, 1m, 28000m, 2100m, 8, 22, 11900m, 42000m, 39900m },
                    { 39, 1m, 21000m, 0m, 10, 23, 11000m, 32000m, 32000m },
                    { 40, 1m, 50000m, 3600m, 6, 24, 18400m, 72000m, 68400m },
                    { 41, 1m, 6400m, 0m, 9, 25, 3400m, 9800m, 9800m },
                    { 42, 1m, 4200m, 0m, 3, 25, 2600m, 6800m, 6800m },
                    { 43, 1m, 28000m, 0m, 8, 26, 14000m, 42000m, 42000m },
                    { 44, 1m, 6400m, 0m, 9, 26, 3400m, 9800m, 9800m },
                    { 45, 1m, 58000m, 4680m, 5, 27, 22320m, 85000m, 80320m },
                    { 46, 2m, 2600m, 0m, 2, 27, 1600m, 4200m, 8400m },
                    { 47, 1m, 5500m, 0m, 1, 28, 3000m, 8500m, 8500m },
                    { 48, 3m, 2000m, 0m, 12, 28, 1200m, 3200m, 9600m },
                    { 49, 1m, 5100m, 0m, 13, 28, 2700m, 7800m, 7800m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaAlta", "FechaApertura", "FechaCierre" },
                values: new object[] { new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(437), new DateTime(2025, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaAlta", "FechaApertura", "FechaCierre" },
                values: new object[] { new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(447), new DateTime(2025, 2, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaAlta", "FechaApertura", "FechaCierre" },
                values: new object[] { new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(452), new DateTime(2025, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9975));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9979));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9981));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9983));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9985));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9987));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9989));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9991));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9993));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(226));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(229));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(232));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(234));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(236));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(238));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(878) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(883) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(886) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(889) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(892) });

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(895) });

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9745));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(793));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(796));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(798));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(799));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(801));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(803));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(804));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(806));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(808));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(809));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(811));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(813));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(814));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(816));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                column: "Fecha",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(817));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9362));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9365));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9366));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9368));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9370));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9371));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9373));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9374));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9376));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9377));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9379));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9381));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9383));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(309));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(319));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(322));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(326));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(329));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(333));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(336));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(340));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(349));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(353));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(360));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(363));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(371));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(139));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(143));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(152));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(154));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 398, DateTimeKind.Local).AddTicks(9269));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(8896));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9502));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9504));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9505));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9507));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9519));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9531));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9533));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9534));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9535));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9536));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9536));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9537));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9538));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9539));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9540));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9608));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9610));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9611));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9612));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9613));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9614));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9615));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9616));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9616));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9617));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9618));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9619));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9678));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9679));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9681));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9682));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9289));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9291));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9293));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9221));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9223));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9225));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9226));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9868));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(514) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(521) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(532) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(536) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(540) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(544) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(548) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(552) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(556) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(560) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(564) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(568) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(572) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(576) });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Fecha", "FechaAlta" },
                values: new object[] { new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(580) });
        }
    }
}
