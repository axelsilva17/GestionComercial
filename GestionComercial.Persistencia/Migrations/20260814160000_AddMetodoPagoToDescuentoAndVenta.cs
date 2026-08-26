using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class AddMetodoPagoToDescuentoAndVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Venta: columnas de descuento por método de pago ──────────
            // Usar Sql directo para evitar table-rebuild de EF Core (conflicto con VistaVentasResumidas)
            // SQLite no admite ALTER condicional; EF aplica la migración una sola vez (historial)
            migrationBuilder.Sql("ALTER TABLE \"Venta\" ADD COLUMN \"DescuentoMetodoPago\" DECIMAL(18,2) NOT NULL DEFAULT 0;");
            migrationBuilder.Sql("ALTER TABLE \"Venta\" ADD COLUMN \"Id_metodoPagoDescuento\" INTEGER NULL;");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS \"IX_Venta_Id_metodoPagoDescuento\" ON \"Venta\" (\"Id_metodoPagoDescuento\");");

            // ── DescuentoConfiguracion: columna FK a MetodoPago ─────────
            migrationBuilder.Sql("ALTER TABLE \"DescuentoConfiguracion\" ADD COLUMN \"Id_metodoPago\" INTEGER NULL;");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS \"IX_DescuentoConfiguracion_Id_metodoPago\" ON \"DescuentoConfiguracion\" (\"Id_metodoPago\");");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_DescuentoConfiguracion_Empresa_Tipo_MetodoPago_Activo\" " +
                "ON \"DescuentoConfiguracion\" (\"Id_empresa\", \"Tipo\", \"Id_metodoPago\", \"Activo\");");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Venta_Id_metodoPagoDescuento\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_DescuentoConfiguracion_Id_metodoPago\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_DescuentoConfiguracion_Empresa_Tipo_MetodoPago_Activo\";");
            migrationBuilder.Sql("ALTER TABLE \"Venta\" DROP COLUMN \"Id_metodoPagoDescuento\";");
            migrationBuilder.Sql("ALTER TABLE \"Venta\" DROP COLUMN \"DescuentoMetodoPago\";");
            migrationBuilder.Sql("ALTER TABLE \"DescuentoConfiguracion\" DROP COLUMN \"Id_metodoPago\";");
        }
    }
}
