using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class RedisenoDescuentoCombinado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Limpieza defensiva de datos ──────────────────────────────
            // El modelo nuevo exige scope (producto O categoría). Las filas antiguas
            // de tipo "Método de Pago" (sin producto ni categoría) quedan sin sentido.
            // En la BD de referencia la tabla está vacía; esto protege BIN y copias.
            migrationBuilder.Sql(
                "DELETE FROM \"DescuentoConfiguracion\" " +
                "WHERE \"Id_producto\" IS NULL AND \"Id_categoria\" IS NULL;");

            // ── Venta: se conserva la columna Id_metodoPagoDescuento ────
            // NOTA: no se emiten DropForeignKey porque SQLite las implementa como
            // table-rebuild (choca con VistaVentasResumidas) y estas FK nunca
            // existieron físicamente (la migración previa las creó por SQL crudo).

            // ── Índices obsoletos (nombres reales en BD) ─────────────────
            migrationBuilder.DropIndex(
                name: "IX_DescuentoConfiguracion_Id_empresa_Tipo_Activo",
                table: "DescuentoConfiguracion");

            migrationBuilder.DropIndex(
                name: "IX_DescuentoConfiguracion_Empresa_Tipo_MetodoPago_Activo",
                table: "DescuentoConfiguracion");

            migrationBuilder.DropIndex(
                name: "IX_DescuentoConfiguracion_Id_metodoPago",
                table: "DescuentoConfiguracion");

            // ── Columnas que desaparecen del modelo ─────────────────────
            migrationBuilder.DropColumn(
                name: "Id_metodoPago",
                table: "DescuentoConfiguracion");

            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "DescuentoConfiguracion");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "DescuentoConfiguracion");

            // ── Nueva columna de condición de pago ──────────────────────
            // Las filas existentes eran de producto/categoría sin restricción
            // de método → pasan a "cualquier método" (default 1).
            migrationBuilder.AddColumn<bool>(
                name: "AplicaCualquierMetodoPago",
                table: "DescuentoConfiguracion",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            // ── Tabla N:M DescuentoMetodoPago ───────────────────────────
            migrationBuilder.CreateTable(
                name: "DescuentoMetodoPago",
                columns: table => new
                {
                    Id_descuentoConfiguracion = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_metodoPago = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescuentoMetodoPago", x => new { x.Id_descuentoConfiguracion, x.Id_metodoPago });
                    table.ForeignKey(
                        name: "FK_DescuentoMetodoPago_DescuentoConfiguracion_Id_descuentoConfiguracion",
                        column: x => x.Id_descuentoConfiguracion,
                        principalTable: "DescuentoConfiguracion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DescuentoMetodoPago_MetodoPago_Id_metodoPago",
                        column: x => x.Id_metodoPago,
                        principalTable: "MetodoPago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoMetodoPago_Id_metodoPago",
                table: "DescuentoMetodoPago",
                column: "Id_metodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoMetodoPago_Id_descuentoConfiguracion",
                table: "DescuentoMetodoPago",
                column: "Id_descuentoConfiguracion");

            // ── Índice nuevo del modelo (empresa + activo) ───────────────
            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Id_empresa_Activo",
                table: "DescuentoConfiguracion",
                columns: new[] { "Id_empresa", "Activo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescuentoMetodoPago");

            migrationBuilder.DropIndex(
                name: "IX_DescuentoConfiguracion_Id_empresa_Activo",
                table: "DescuentoConfiguracion");

            migrationBuilder.DropColumn(
                name: "AplicaCualquierMetodoPago",
                table: "DescuentoConfiguracion");

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "DescuentoConfiguracion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Prioridad",
                table: "DescuentoConfiguracion",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_metodoPago",
                table: "DescuentoConfiguracion",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Id_empresa_Tipo_Activo",
                table: "DescuentoConfiguracion",
                columns: new[] { "Id_empresa", "Tipo", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Empresa_Tipo_MetodoPago_Activo",
                table: "DescuentoConfiguracion",
                columns: new[] { "Id_empresa", "Tipo", "Id_metodoPago", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoConfiguracion_Id_metodoPago",
                table: "DescuentoConfiguracion",
                column: "Id_metodoPago");
        }
    }
}