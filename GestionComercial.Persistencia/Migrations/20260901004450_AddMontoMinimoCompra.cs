using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddMontoMinimoCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Venta_Id_usuario",
                table: "Venta");

            migrationBuilder.RenameIndex(
                name: "IX_Venta_Id_sucursal_Fecha",
                table: "Venta",
                newName: "IX_Venta_Sucursal_Fecha");

            migrationBuilder.AddColumn<decimal>(
                name: "MontoMinimoCompra",
                table: "DescuentoConfiguracion",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MantenimientoLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Resultado = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Detalles = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientoLog", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Venta_Usuario_Fecha",
                table: "Venta",
                columns: new[] { "Id_usuario", "Fecha" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MantenimientoLog");

            migrationBuilder.DropIndex(
                name: "IX_Venta_Usuario_Fecha",
                table: "Venta");

            migrationBuilder.DropColumn(
                name: "MontoMinimoCompra",
                table: "DescuentoConfiguracion");

            migrationBuilder.RenameIndex(
                name: "IX_Venta_Sucursal_Fecha",
                table: "Venta",
                newName: "IX_Venta_Id_sucursal_Fecha");

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

            migrationBuilder.CreateIndex(
                name: "IX_Venta_Id_usuario",
                table: "Venta",
                column: "Id_usuario");
        }
    }
}
