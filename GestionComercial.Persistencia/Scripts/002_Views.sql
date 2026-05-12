-- ═══════════════════════════════════════════════════════════════════════
-- Script: 002_Views.sql
-- Proyecto: GestionComercial
-- Descripción: Definiciones de Vistas para consultas optimizadas
-- ───────────────────────────────────────────────────────────────────────
-- Uso: Ejecutar en SQLite directamente o incluir en migración EF Core
-- Idempotente: usa DROP VIEW IF EXISTS + CREATE VIEW
-- ═══════════════════════════════════════════════════════════════════════

-- ── VistaVentasResumidas ─────────────────────────────────────────────
-- Combina datos de Venta con Cliente, Usuario, Sucursal y Método de Pago.
-- LEFT JOINs para no perder ventas sin pago o sin cliente.
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

-- ── VistaProductosConStock ────────────────────────────────────────────
-- Lista productos con su categoría y unidad de medida.
-- LEFT JOIN para productos sin categoría.
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

-- ── VistaMovimientosStock ─────────────────────────────────────────────
-- Detalle de movimientos de stock con nombre de producto y tipo.
-- Usa JOIN (no LEFT) porque MovimientoStock siempre tiene producto y tipo.
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