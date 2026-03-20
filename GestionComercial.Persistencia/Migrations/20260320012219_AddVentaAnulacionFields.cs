using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddVentaAnulacionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAnulacion",
                table: "Venta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacion",
                table: "Venta",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioAnulacionId",
                table: "Venta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_movimientoCaja",
                table: "Pago",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id_venta",
                table: "MovimientoCaja",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VentaDetalleDescuento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_detalle = table.Column<int>(type: "int", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDetalleDescuento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaDetalleDescuento_VentaDetalle_Id_detalle",
                        column: x => x.Id_detalle,
                        principalTable: "VentaDetalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VentaDetalleImpuesto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_detalle = table.Column<int>(type: "int", nullable: false),
                    Id_tipoImpuesto = table.Column<int>(type: "int", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDetalleImpuesto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaDetalleImpuesto_VentaDetalle_Id_detalle",
                        column: x => x.Id_detalle,
                        principalTable: "VentaDetalle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "Caja",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9409));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9411));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9413));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9414));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9416));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9417));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9419));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9422));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9546));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9552));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9554));

            migrationBuilder.UpdateData(
                table: "Cliente",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(234));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(238));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(245));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(247));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(252));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(254));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(257));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(259));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(261));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(264));

            migrationBuilder.UpdateData(
                table: "Compra",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(266));

            migrationBuilder.UpdateData(
                table: "Empresa",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9264));

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(136), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(139), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(140), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(142), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(143), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(144), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(145), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(147), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(148), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(150), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(151), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(152), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(154), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(155), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(156), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(157), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(159), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(160), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(162), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(163), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(164), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(166), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(167), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(169), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(170), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(171), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(172), null });

            migrationBuilder.UpdateData(
                table: "Pago",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Fecha", "Id_movimientoCaja" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 510, DateTimeKind.Local).AddTicks(174), null });

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8988));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8990));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8992));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8993));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8994));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8995));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8998));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8999));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9001));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9002));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9003));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9004));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9005));

            migrationBuilder.UpdateData(
                table: "Permiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9007));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9609));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9615));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9623));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9658));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9661));

            migrationBuilder.UpdateData(
                table: "Producto",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9664));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9506));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9509));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9510));

            migrationBuilder.UpdateData(
                table: "Proveedor",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9512));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8631));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8645));

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8646));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9091));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9093));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9093));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9094));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9105));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 7,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9119));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 8,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9120));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 9,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9120));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 10,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 11,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 12,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9122));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 13,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9123));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 14,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9123));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 15,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 16,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9163));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 17,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9165));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 18,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 19,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 20,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9167));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 21,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9167));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 22,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 23,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 24,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 25,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9170));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 26,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9171));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 27,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9171));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 28,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 29,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 30,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9204));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 31,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9205));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 32,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9207));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 33,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9208));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 34,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9209));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 35,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9210));

            migrationBuilder.UpdateData(
                table: "RolPermiso",
                keyColumn: "Id",
                keyValue: 36,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9212));

            migrationBuilder.UpdateData(
                table: "Sucursal",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9300));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8904));

            migrationBuilder.UpdateData(
                table: "TipoDocumento",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8858));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8859));

            migrationBuilder.UpdateData(
                table: "TipoMovimientoStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(8860));

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9334));

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9796), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9802), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9806), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9809), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9812), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9815), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9818), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9822), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9831), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9835), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9838), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9841), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9844), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9847), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9851), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9854), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9857), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9860), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9863), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9866), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9870), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9873), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9876), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9879), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9883), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9886), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9889), null, null, null });

            migrationBuilder.UpdateData(
                table: "Venta",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "FechaAlta", "FechaAnulacion", "MotivoAnulacion", "UsuarioAnulacionId" },
                values: new object[] { new DateTime(2026, 3, 19, 22, 22, 17, 509, DateTimeKind.Local).AddTicks(9892), null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Pago_Id_movimientoCaja",
                table: "Pago",
                column: "Id_movimientoCaja");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoCaja_Id_venta",
                table: "MovimientoCaja",
                column: "Id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalleDescuento_Id_detalle",
                table: "VentaDetalleDescuento",
                column: "Id_detalle");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalleImpuesto_Id_detalle",
                table: "VentaDetalleImpuesto",
                column: "Id_detalle");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoCaja_Venta_Id_venta",
                table: "MovimientoCaja",
                column: "Id_venta",
                principalTable: "Venta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pago_MovimientoCaja_Id_movimientoCaja",
                table: "Pago",
                column: "Id_movimientoCaja",
                principalTable: "MovimientoCaja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientoCaja_Venta_Id_venta",
                table: "MovimientoCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_Pago_MovimientoCaja_Id_movimientoCaja",
                table: "Pago");

            migrationBuilder.DropTable(
                name: "VentaDetalleDescuento");

            migrationBuilder.DropTable(
                name: "VentaDetalleImpuesto");

            migrationBuilder.DropIndex(
                name: "IX_Pago_Id_movimientoCaja",
                table: "Pago");

            migrationBuilder.DropIndex(
                name: "IX_MovimientoCaja_Id_venta",
                table: "MovimientoCaja");

            migrationBuilder.DropColumn(
                name: "FechaAnulacion",
                table: "Venta");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacion",
                table: "Venta");

            migrationBuilder.DropColumn(
                name: "UsuarioAnulacionId",
                table: "Venta");

            migrationBuilder.DropColumn(
                name: "Id_movimientoCaja",
                table: "Pago");

            migrationBuilder.DropColumn(
                name: "Id_venta",
                table: "MovimientoCaja");

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
        }
    }
}
