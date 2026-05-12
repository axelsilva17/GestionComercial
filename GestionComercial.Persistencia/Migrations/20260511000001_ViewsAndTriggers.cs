using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    /// <inheritdoc />
    public partial class ViewsAndTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ═══════════════════════════════════════════════════════════════════════
            // VISTAS
            // ═══════════════════════════════════════════════════════════════════════

            // ── VistaVentasResumidas ──────────────────────────────────────────────
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS VistaVentasResumidas;
                CREATE VIEW VistaVentasResumidas AS
                SELECT
                    v.Id,
                    v.Fecha,
                    v.TotalFinal AS Total,
                    v.Estado,
                    c.Nombre AS ClienteNombre,
                    (u.Nombre || ' ' || u.Apellido) AS UsuarioNombre,
                    s.Nombre AS SucursalNombre,
                    mp.Nombre AS MetodoPagoNombre
                FROM Venta v
                LEFT JOIN Cliente c ON v.Id_cliente = c.Id
                LEFT JOIN Usuario u ON v.Id_usuario = u.Id
                LEFT JOIN Sucursal s ON v.Id_sucursal = s.Id
                LEFT JOIN Pago p ON p.Id_venta = v.Id
                LEFT JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id;
            ");

            // ── VistaProductosConStock ────────────────────────────────────────────
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS VistaProductosConStock;
                CREATE VIEW VistaProductosConStock AS
                SELECT
                    p.Id,
                    p.Nombre,
                    p.CodigoBarra,
                    p.PrecioVentaActual AS PrecioVenta,
                    p.StockActual AS Stock,
                    cat.Nombre AS CategoriaNombre,
                    um.Nombre AS UnidadMedidaNombre,
                    p.Activo
                FROM Producto p
                LEFT JOIN Categoria cat ON p.Id_categoria = cat.Id
                LEFT JOIN UnidadMedida um ON p.Id_unidadMedida = um.Id;
            ");

            // ── VistaMovimientosStock ─────────────────────────────────────────────
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS VistaMovimientosStock;
                CREATE VIEW VistaMovimientosStock AS
                SELECT
                    ms.Id,
                    ms.Fecha,
                    ms.Cantidad,
                    tms.Nombre AS TipoMovimientoNombre,
                    p.Nombre AS ProductoNombre,
                    ms.Observacion
                FROM MovimientoStock ms
                JOIN TipoMovimientoStock tms ON ms.TipoMovimiento = tms.Id
                JOIN Producto p ON ms.Id_producto = p.Id;
            ");

            // ═══════════════════════════════════════════════════════════════════════
            // TRIGGERS DE AUDITORÍA
            // ═══════════════════════════════════════════════════════════════════════

            // ── TRG_Producto_Insert_Audit ─────────────────────────────────────────
            // Las llaves JSON se escapan como {{ }} para evitar conflicto con C# interpolation
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS TRG_Producto_Insert_Audit;
                CREATE TRIGGER TRG_Producto_Insert_Audit
                AFTER INSERT ON Producto
                BEGIN
                    INSERT INTO AuditoriaLogs (
                        NombreTabla,
                        RegistroId,
                        TipoOperacion,
                        FechaOperacion,
                        ValoresNuevos
                    )
                    VALUES (
                        'Producto',
                        NEW.Id,
                        1,
                        datetime('now', 'localtime'),
                        '{' + char(34) + 'nombre' + char(34) + ':' + char(34) + COALESCE(NEW.Nombre, '') + char(34) + ',' +
                         char(34) + 'stock' + char(34) + ':' + COALESCE(NEW.StockActual, 0) + ',' +
                         char(34) + 'precio' + char(34) + ':' + COALESCE(NEW.PrecioVentaActual, 0) + '}'
                    );
                END;
            ");

            // ── TRG_Producto_Update_Audit ─────────────────────────────────────────
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS TRG_Producto_Update_Audit;
                CREATE TRIGGER TRG_Producto_Update_Audit
                AFTER UPDATE ON Producto
                WHEN OLD.Nombre != NEW.Nombre
                    OR OLD.StockActual != NEW.StockActual
                    OR OLD.PrecioVentaActual != NEW.PrecioVentaActual
                BEGIN
                    INSERT INTO AuditoriaLogs (
                        NombreTabla,
                        RegistroId,
                        TipoOperacion,
                        FechaOperacion,
                        ValoresAnteriores,
                        ValoresNuevos
                    )
                    VALUES (
                        'Producto',
                        NEW.Id,
                        2,
                        datetime('now', 'localtime'),
                        '{' + char(34) + 'nombre' + char(34) + ':' + char(34) + COALESCE(OLD.Nombre, '') + char(34) + ',' +
                         char(34) + 'stock' + char(34) + ':' + COALESCE(OLD.StockActual, 0) + ',' +
                         char(34) + 'precio' + char(34) + ':' + COALESCE(OLD.PrecioVentaActual, 0) + '}',
                        '{' + char(34) + 'nombre' + char(34) + ':' + char(34) + COALESCE(NEW.Nombre, '') + char(34) + ',' +
                         char(34) + 'stock' + char(34) + ':' + COALESCE(NEW.StockActual, 0) + ',' +
                         char(34) + 'precio' + char(34) + ':' + COALESCE(NEW.PrecioVentaActual, 0) + '}'
                    );
                END;
            ");

            // ── TRG_Stock_Validate ───────────────────────────────────────────────
            migrationBuilder.Sql(@"
                DROP TRIGGER IF EXISTS TRG_Stock_Validate;
                CREATE TRIGGER TRG_Stock_Validate
                BEFORE UPDATE ON Producto
                WHEN NEW.StockActual < 0
                BEGIN
                    SELECT RAISE(ABORT, 'Stock no puede ser negativo');
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS VistaVentasResumidas;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS VistaProductosConStock;");
            migrationBuilder.Sql("DROP VIEW IF EXISTS VistaMovimientosStock;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TRG_Producto_Insert_Audit;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TRG_Producto_Update_Audit;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TRG_Stock_Validate;");
        }
    }
}