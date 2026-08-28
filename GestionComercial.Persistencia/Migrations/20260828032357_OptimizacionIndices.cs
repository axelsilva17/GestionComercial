using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <summary>
    /// Índices SQLite para optimizar las queries más pesadas del sistema.
    /// Basado en análisis de patrones de consulta:
    /// - Venta: filtrado por sucursal+fecha+estado (listados, reportes, caja)
    /// - VentaDetalle: lookup por venta, por producto (reportes, stock)
    /// - Producto: búsqueda por empresa+categoría (listados, búsquedas)
    /// - Cliente/Proveedor: búsqueda por empresa
    /// - Pago: lookup por venta
    /// - Caja: filtro por sucursal+estado
    /// - MovimientoStock: historial por producto
    /// - DescuentoConfiguracion: resolución por empresa+activo
    /// </summary>
    public partial class OptimizacionIndices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Venta: query principal del sistema (listado, caja, reportes) ──
            // Filtro más usado: Id_sucursal + Fecha + Estado
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Venta_SucursalFechaEstado 
                ON Venta(Id_sucursal, Fecha DESC, Estado);
            ");

            // Para dashboard: sumar totales por fecha
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Venta_FechaEstado 
                ON Venta(Estado, Fecha DESC);
            ");

            // Para búsquedas por cliente
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Venta_Cliente 
                ON Venta(Id_cliente);
            ");

            // ── VentaDetalle: reportes y cálculos de stock ──
            // El más pesado: JOIN con Venta para filtrar por sucursal+fecha
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_VentaDetalle_Venta 
                ON VentaDetalle(Id_venta);
            ");

            // Para reportes de top productos y rotación
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_VentaDetalle_Producto 
                ON VentaDetalle(Id_producto);
            ");

            // Compound: para queries que agrupan por producto con precio
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_VentaDetalle_ProductoPrecio 
                ON VentaDetalle(Id_producto, PrecioUnitario, Cantidad);
            ");

            // ── Producto: listados y búsquedas ──
            // Búsqueda por empresa (listado principal)
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Producto_Empresa 
                ON Producto(Id_empresa, Activo);
            ");

            // Búsqueda por categoría dentro de empresa
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Producto_Categoria 
                ON Producto(Id_categoria);
            ");

            // Búsqueda por código de barras (escáner)
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Producto_CodigoBarra 
                ON Producto(CodigoBarra);
            ");

            // Stock crítico: productos con stock bajo
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Producto_Stock 
                ON Producto(Id_empresa, StockActual, StockMinimo);
            ");

            // ── Cliente: búsquedas por empresa y documento ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Cliente_Empresa 
                ON Cliente(Id_empresa, Activo);
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Cliente_Documento 
                ON Cliente(Documento);
            ");

            // ── Proveedor ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Proveedor_Empresa 
                ON Proveedor(Id_empresa, Activo);
            ");

            // ── Pago: lookup por venta ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Pago_Venta 
                ON Pago(Id_venta);
            ");

            // ── Caja: filtros por sucursal ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Caja_SucursalEstado 
                ON Caja(Id_sucursal, Estado);
            ");

            // ── MovimientoCaja: historial por caja ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_MovimientoCaja_Caja 
                ON MovimientoCaja(Id_caja);
            ");

            // ── MovimientoStock: historial por producto ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_MovimientoStock_Producto 
                ON MovimientoStock(Id_producto, Fecha DESC);
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_MovimientoStock_Sucursal 
                ON MovimientoStock(Id_sucursal, Fecha DESC);
            ");

            // ── Compra ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Compra_SucursalFecha 
                ON Compra(Id_sucursal, Fecha DESC);
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_CompraDetalle_Compra 
                ON CompraDetalle(Id_compra);
            ");

            // ── DescuentoConfiguracion: resolución por empresa ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_Descuento_EmpresaActivo 
                ON DescuentoConfiguracion(Id_empresa, Activo);
            ");

            // ── VentaDetalleDescuento ──
            migrationBuilder.Sql(@"
                CREATE INDEX IF NOT EXISTS IX_VentaDetalleDescuento_Detalle 
                ON VentaDetalleDescuento(Id_ventaDetalle);
            ");

            // ── SQLite: optimizaciones de rendimiento ──
            // Temp store en memoria (tablas temporales)
            migrationBuilder.Sql("PRAGMA temp_store = MEMORY;");
            // Cache de 64MB para queries complejas
            migrationBuilder.Sql("PRAGMA cache_size = -64000;");
            // Memory-mapped I/O: 256MB (mejora lecturas frecuentes)
            migrationBuilder.Sql("PRAGMA mmap_size = 268435456;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Venta_SucursalFechaEstado;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Venta_FechaEstado;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Venta_Cliente;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_VentaDetalle_Venta;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_VentaDetalle_Producto;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_VentaDetalle_ProductoPrecio;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Producto_Empresa;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Producto_Categoria;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Producto_CodigoBarra;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Producto_Stock;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Cliente_Empresa;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Cliente_Documento;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Proveedor_Empresa;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Pago_Venta;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Caja_SucursalEstado;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_MovimientoCaja_Caja;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_MovimientoStock_Producto;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_MovimientoStock_Sucursal;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Compra_SucursalFecha;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_CompraDetalle_Compra;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_Descuento_EmpresaActivo;");
            migrationBuilder.Sql("DROP INDEX IF EXISTS IX_VentaDetalleDescuento_Detalle;");
        }
    }
}
