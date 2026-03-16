using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class DatosPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Documento",
                table: "Cliente",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "Caja",
                columns: new[] { "Id", "Activo", "Estado", "FechaAlta", "FechaApertura", "FechaCierre", "Id_sucursal", "MontoFinal", "MontoInicial", "Observacion", "UsuarioApertura_id", "UsuarioCierre_id" },
                values: new object[,]
                {
                    { 1, true, 2, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(437), new DateTime(2025, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 285000m, 50000m, null, 1, 1 },
                    { 2, true, 2, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(447), new DateTime(2025, 2, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 320000m, 50000m, null, 1, 1 },
                    { 3, true, 2, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(452), new DateTime(2025, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1, 410000m, 50000m, null, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Activo", "CategoriaPadre_id", "FechaAlta", "Id_empresa", "Nombre" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9975), 1, "Herramientas" },
                    { 2, true, null, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9979), 1, "Materiales" },
                    { 3, true, null, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9981), 1, "Electricidad" },
                    { 4, true, null, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9983), 1, "Pintura" },
                    { 5, true, null, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9985), 1, "Plomería" }
                });

            migrationBuilder.InsertData(
                table: "Cliente",
                columns: new[] { "Id", "Activo", "Documento", "Email", "FechaAlta", "Id_empresa", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, true, 0, null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(226), 1, "Consumidor Final", null },
                    { 2, true, 30111111, "juan@gmail.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(229), 1, "Juan Pérez", "3794555001" },
                    { 3, true, 32222222, "maria@gmail.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(232), 1, "María González", "3794555002" },
                    { 4, true, 28333333, "carlos@empresa.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(234), 1, "Carlos Rodríguez", "3794555003" },
                    { 5, true, 35444444, "laura@gmail.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(236), 1, "Laura Martínez", "3794555004" },
                    { 6, true, 30555555, "compras@constructora.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(238), 1, "Constructora ABC", "3794555005" }
                });

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9745));

            migrationBuilder.InsertData(
                table: "MetodoPago",
                columns: new[] { "Id", "Activo", "EsEfectivo", "Id_empresa", "Nombre" },
                values: new object[,]
                {
                    { 1, true, true, 1, "Efectivo" },
                    { 2, true, false, 1, "Débito" },
                    { 3, true, false, 1, "Crédito" },
                    { 4, true, false, 1, "Transferencia" },
                    { 5, true, false, 1, "Mercado Pago" }
                });

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

            migrationBuilder.InsertData(
                table: "Proveedor",
                columns: new[] { "Id", "Activo", "CUIT", "Email", "FechaAlta", "Id_empresa", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, true, "30-11111111-1", "norte@proveedor.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(139), 1, "Distribuidora Norte", "3794111111" },
                    { 2, true, "30-22222222-2", "sur@pinturerias.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(143), 1, "Pinturerias del Sur", "3794222222" },
                    { 3, true, "30-33333333-3", "ventas@electro.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(152), 1, "Electro Mayorista", "3794333333" },
                    { 4, true, "30-44444444-4", "info@constructor.com", new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(154), 1, "Materiales El Constructor", "3794444444" }
                });

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

            migrationBuilder.InsertData(
                table: "UnidadMedida",
                columns: new[] { "Id", "Abreviatura", "Nombre" },
                values: new object[,]
                {
                    { 1, "UN", "Unidad" },
                    { 2, "KG", "Kilogramo" },
                    { 3, "MT", "Metro" },
                    { 4, "LT", "Litro" },
                    { 5, "CJA", "Caja" }
                });

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9868));

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Activo", "CategoriaPadre_id", "FechaAlta", "Id_empresa", "Nombre" },
                values: new object[,]
                {
                    { 6, true, 1, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9987), 1, "Manuales" },
                    { 7, true, 1, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9989), 1, "Eléctricas" },
                    { 8, true, 4, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9991), 1, "Látex" },
                    { 9, true, 4, new DateTime(2026, 3, 9, 0, 38, 3, 399, DateTimeKind.Local).AddTicks(9993), 1, "Esmalte" }
                });

            migrationBuilder.InsertData(
                table: "Compra",
                columns: new[] { "Id", "Activo", "Estado", "Fecha", "FechaAlta", "Id_proveedor", "Id_sucursal", "Id_usuario", "Observacion", "Total" },
                values: new object[,]
                {
                    { 1, true, 2, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(878), 1, 1, 1, null, 145000m },
                    { 2, true, 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(883), 2, 1, 1, null, 84000m },
                    { 3, true, 2, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(886), 3, 1, 1, null, 108000m },
                    { 4, true, 2, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(889), 4, 1, 1, null, 62000m },
                    { 5, true, 2, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(892), 1, 1, 1, null, 174000m },
                    { 6, true, 2, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(895), 2, 1, 1, null, 96000m }
                });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "Activo", "CodigoBarra", "Descripcion", "FechaAlta", "Id_categoria", "Id_empresa", "Id_unidadMedida", "Nombre", "PrecioCostoActual", "PrecioVentaActual", "StockActual", "StockMinimo" },
                values: new object[,]
                {
                    { 11, true, "7790001000011", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(356), 3, 1, 3, "Cable Unipolar 1.5mm x5m", 3500m, 5500m, 50m, 10m },
                    { 12, true, "7790001000012", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(360), 3, 1, 1, "Tomacorriente Doble", 2000m, 3200m, 35m, 8m },
                    { 13, true, "7790001000013", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(363), 3, 1, 1, "Disyuntor 16A", 5100m, 7800m, 22m, 5m },
                    { 14, true, "7790001000014", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(367), 5, 1, 3, "Caño PVC 1\" x3m", 3100m, 4800m, 40m, 8m },
                    { 15, true, "7790001000015", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(371), 5, 1, 1, "Codo PVC 90° 1\"", 520m, 850m, 3m, 10m }
                });

            migrationBuilder.InsertData(
                table: "Venta",
                columns: new[] { "Id", "Activo", "Estado", "Fecha", "FechaAlta", "Id_caja", "Id_cliente", "Id_sucursal", "Id_usuario", "Observacion", "TotalBruto", "TotalDescuento", "TotalFinal" },
                values: new object[,]
                {
                    { 1, true, 2, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(514), 1, 2, 1, 1, null, 25700m, 0m, 25700m },
                    { 2, true, 2, new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(521), 1, 1, 1, 1, null, 85000m, 0m, 85000m },
                    { 3, true, 2, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(532), 1, 3, 1, 1, null, 18500m, 925m, 17575m },
                    { 4, true, 2, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(536), 1, 6, 1, 1, null, 54800m, 0m, 54800m },
                    { 5, true, 2, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(540), 1, 4, 1, 1, null, 32000m, 1600m, 30400m },
                    { 6, true, 2, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(544), 2, 1, 1, 1, null, 42000m, 0m, 42000m },
                    { 7, true, 2, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(548), 2, 2, 1, 1, null, 12500m, 0m, 12500m },
                    { 8, true, 2, new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(552), 2, 6, 1, 1, null, 93600m, 4680m, 88920m },
                    { 9, true, 2, new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(556), 2, 5, 1, 1, null, 27300m, 0m, 27300m },
                    { 10, true, 2, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(560), 2, 3, 1, 1, null, 16800m, 0m, 16800m },
                    { 11, true, 2, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(564), 3, 6, 1, 1, null, 72000m, 3600m, 68400m },
                    { 12, true, 2, new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(568), 3, 1, 1, 1, null, 9800m, 0m, 9800m },
                    { 13, true, 2, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(572), 3, 4, 1, 1, null, 47500m, 0m, 47500m },
                    { 14, true, 2, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(576), 3, 6, 1, 1, null, 156000m, 7800m, 148200m },
                    { 15, true, 2, new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(580), 3, 2, 1, 1, null, 22400m, 0m, 22400m }
                });

            migrationBuilder.InsertData(
                table: "CompraDetalle",
                columns: new[] { "Id", "Cantidad", "Id_compra", "Id_producto", "PrecioCosto", "Subtotal" },
                values: new object[,]
                {
                    { 8, 10m, 3, 11, 3500m, 35000m },
                    { 9, 10m, 3, 12, 2000m, 20000m },
                    { 10, 10m, 4, 14, 3100m, 31000m },
                    { 11, 20m, 4, 15, 520m, 10400m },
                    { 12, 8m, 4, 13, 5100m, 40800m }
                });

            migrationBuilder.InsertData(
                table: "Pago",
                columns: new[] { "Id", "Fecha", "Id_metodoPago", "Id_venta", "Monto" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(793), 1, 1, 25700m },
                    { 2, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(796), 3, 2, 85000m },
                    { 3, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(798), 1, 3, 17575m },
                    { 4, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(799), 4, 4, 54800m },
                    { 5, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(801), 2, 5, 30400m },
                    { 6, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(803), 1, 6, 42000m },
                    { 7, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(804), 5, 7, 12500m },
                    { 8, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(806), 4, 8, 88920m },
                    { 9, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(808), 1, 9, 27300m },
                    { 10, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(809), 2, 10, 16800m },
                    { 11, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(811), 3, 11, 68400m },
                    { 12, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(813), 1, 12, 9800m },
                    { 13, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(814), 4, 13, 47500m },
                    { 14, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(816), 3, 14, 148200m },
                    { 15, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(817), 1, 15, 22400m }
                });

            migrationBuilder.InsertData(
                table: "Producto",
                columns: new[] { "Id", "Activo", "CodigoBarra", "Descripcion", "FechaAlta", "Id_categoria", "Id_empresa", "Id_unidadMedida", "Nombre", "PrecioCostoActual", "PrecioVentaActual", "StockActual", "StockMinimo" },
                values: new object[,]
                {
                    { 1, true, "7790001000001", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(309), 6, 1, 1, "Martillo 500g", 5500m, 8500m, 25m, 5m },
                    { 2, true, "7790001000002", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(319), 6, 1, 1, "Destornillador Philips", 2600m, 4200m, 40m, 8m },
                    { 3, true, "7790001000003", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(322), 6, 1, 1, "Alicate Universal", 4200m, 6800m, 18m, 5m },
                    { 4, true, "7790001000004", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(326), 6, 1, 1, "Sierra Arco", 8000m, 12500m, 12m, 3m },
                    { 5, true, "7790001000005", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(329), 7, 1, 1, "Taladro 650W", 58000m, 85000m, 8m, 2m },
                    { 6, true, "7790001000006", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(333), 7, 1, 1, "Amoladora 115mm", 50000m, 72000m, 6m, 2m },
                    { 7, true, "7790001000007", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(336), 8, 1, 4, "Látex Interior Blanco 4L", 12000m, 18500m, 30m, 6m },
                    { 8, true, "7790001000008", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(340), 8, 1, 4, "Látex Exterior 10L", 28000m, 42000m, 15m, 4m },
                    { 9, true, "7790001000009", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(349), 9, 1, 4, "Esmalte Sintético 1L", 6400m, 9800m, 20m, 5m },
                    { 10, true, "7790001000010", null, new DateTime(2026, 3, 9, 0, 38, 3, 400, DateTimeKind.Local).AddTicks(353), 9, 1, 4, "Esmalte Negro 4L", 21000m, 32000m, 10m, 3m }
                });

            migrationBuilder.InsertData(
                table: "VentaDetalle",
                columns: new[] { "Id", "Cantidad", "CostoUnitario", "Descuento", "Id_producto", "Id_venta", "MargenUnitario", "PrecioUnitario", "Subtotal" },
                values: new object[,]
                {
                    { 3, 2m, 2000m, 0m, 12, 1, 1200m, 3200m, 6400m },
                    { 4, 1m, 5100m, 0m, 13, 1, 2700m, 7800m, 7800m },
                    { 16, 1m, 3500m, 0m, 11, 9, 2000m, 5500m, 5500m },
                    { 27, 1m, 3100m, 0m, 14, 15, 1700m, 4800m, 4800m }
                });

            migrationBuilder.InsertData(
                table: "CompraDetalle",
                columns: new[] { "Id", "Cantidad", "Id_compra", "Id_producto", "PrecioCosto", "Subtotal" },
                values: new object[,]
                {
                    { 1, 10m, 1, 1, 5500m, 55000m },
                    { 2, 15m, 1, 2, 2600m, 39000m },
                    { 3, 8m, 1, 3, 4200m, 33600m },
                    { 4, 4m, 1, 4, 8000m, 32000m },
                    { 5, 10m, 2, 7, 12000m, 120000m },
                    { 6, 8m, 2, 9, 6400m, 51200m },
                    { 7, 1m, 3, 5, 58000m, 58000m },
                    { 13, 1m, 5, 5, 58000m, 58000m },
                    { 14, 1m, 5, 6, 50000m, 50000m },
                    { 15, 12m, 5, 1, 5500m, 66000m },
                    { 16, 2m, 6, 8, 28000m, 56000m },
                    { 17, 2m, 6, 10, 21000m, 42000m }
                });

            migrationBuilder.InsertData(
                table: "VentaDetalle",
                columns: new[] { "Id", "Cantidad", "CostoUnitario", "Descuento", "Id_producto", "Id_venta", "MargenUnitario", "PrecioUnitario", "Subtotal" },
                values: new object[,]
                {
                    { 1, 1m, 5500m, 0m, 1, 1, 3000m, 8500m, 8500m },
                    { 2, 2m, 2600m, 0m, 2, 1, 1600m, 4200m, 8400m },
                    { 5, 1m, 58000m, 0m, 5, 2, 27000m, 85000m, 85000m },
                    { 6, 1m, 12000m, 925m, 7, 3, 5575m, 18500m, 17575m },
                    { 7, 1m, 28000m, 0m, 8, 4, 14000m, 42000m, 42000m },
                    { 8, 1m, 6400m, 0m, 9, 4, 3400m, 9800m, 9800m },
                    { 9, 1m, 4200m, 0m, 3, 4, 2600m, 6800m, 6800m },
                    { 10, 1m, 21000m, 1600m, 10, 5, 9400m, 32000m, 30400m },
                    { 11, 1m, 28000m, 0m, 8, 6, 14000m, 42000m, 42000m },
                    { 12, 1m, 8000m, 0m, 4, 7, 4500m, 12500m, 12500m },
                    { 13, 1m, 58000m, 4680m, 5, 8, 22320m, 85000m, 80320m },
                    { 14, 1m, 50000m, 0m, 6, 8, 22000m, 72000m, 72000m },
                    { 15, 1m, 12000m, 0m, 7, 9, 6500m, 18500m, 18500m },
                    { 17, 1m, 2600m, 0m, 2, 9, 1600m, 4200m, 4200m },
                    { 18, 1m, 6400m, 0m, 9, 10, 3400m, 9800m, 9800m },
                    { 19, 1m, 4200m, 0m, 3, 10, 2600m, 6800m, 6800m },
                    { 20, 1m, 50000m, 3600m, 6, 11, 18400m, 72000m, 68400m },
                    { 21, 1m, 6400m, 0m, 9, 12, 3400m, 9800m, 9800m },
                    { 22, 1m, 28000m, 0m, 8, 13, 14000m, 42000m, 42000m },
                    { 23, 1m, 5500m, 0m, 1, 13, 3000m, 8500m, 8500m },
                    { 24, 1m, 58000m, 7800m, 5, 14, 19200m, 85000m, 77200m },
                    { 25, 1m, 50000m, 0m, 6, 14, 22000m, 72000m, 72000m },
                    { 26, 1m, 12000m, 0m, 7, 15, 6500m, 18500m, 18500m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CompraDetalle",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "UnidadMedida",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnidadMedida",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "VentaDetalle",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MetodoPago",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UnidadMedida",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnidadMedida",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UnidadMedida",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Documento",
                table: "Cliente",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9257));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8895));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8898));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8904));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8906));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8908));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8910));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8912));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8914));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8916));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8918));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8920));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8922));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8924));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8414));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8429));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8432));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9041));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9044));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9045));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9047));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9048));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9049));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9050));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9052));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9053));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9054));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9055));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9056));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9058));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9123));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9125));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9126));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9127));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9128));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9130));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9131));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9132));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9134));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9135));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9186));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9188));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9190));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9192));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9195));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9314));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8828));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8830));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8832));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8763));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8766));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8767));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(8769));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 6, 9, 21, 46, 195, DateTimeKind.Local).AddTicks(9366));
        }
    }
}
