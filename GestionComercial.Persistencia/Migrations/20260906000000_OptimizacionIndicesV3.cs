using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(GestionComercialContext))]
    [Migration("20260906000000_OptimizacionIndicesV3")]
    public partial class OptimizacionIndicesV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Covering indexes ──
            // Covers the KPI aggregation and report queries:
            //   WHERE Id_sucursal = ? AND Fecha BETWEEN ? AND ? AND Estado = ? GROUP BY Id_usuario
            // plus VentaDetalle joins/aggregations on Id_venta + Id_producto with amount columns.
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Venta_Sucursal_Fecha_Estado_Usuario_TotalFinal
                ON Venta(Id_sucursal, Fecha, Estado, Id_usuario, TotalFinal);
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_VentaDetalle_Venta_Producto_Montos
                ON VentaDetalle(Id_venta, Id_producto, Cantidad, CostoUnitario, Subtotal);
            ");

            // ── Dead index clutter (duplicates/subsumed by the covering indexes above) ──
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Venta_FechaEstado;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Venta_Fecha;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Venta_Sucursal_Fecha;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Pago_Id_venta;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_VentaDetalle_Id_venta;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the dropped indexes with their original definitions
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS IX_Venta_FechaEstado ON Venta(Estado, Fecha DESC);");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS IX_Venta_Fecha ON Venta(Fecha);");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS IX_Venta_Sucursal_Fecha ON Venta(Id_sucursal, Fecha);");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS IX_Pago_Id_venta ON Pago(Id_venta);");
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS IX_VentaDetalle_Id_venta ON VentaDetalle(Id_venta);");

            // Remove the covering indexes
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Venta_Sucursal_Fecha_Estado_Usuario_TotalFinal;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_VentaDetalle_Venta_Producto_Montos;");
        }
    }
}