-- ═══════════════════════════════════════════════════════════════════════
-- Script: 003_Triggers.sql
-- Proyecto: GestionComercial
-- Descripción: Triggers para auditoría automática y validación de stock
-- ───────────────────────────────────────────────────────────────────────
-- Uso: Ejecutar en SQLite directamente o incluir en migración EF Core
-- Idempotente: usa DROP TRIGGER IF EXISTS + CREATE TRIGGER
-- ═══════════════════════════════════════════════════════════════════════

-- ═══════════════════════════════════════════════════════════════════════
-- TRIGGERS DE AUDITORÍA
-- ═══════════════════════════════════════════════════════════════════════

-- ── TRG_Producto_Insert_Audit ─────────────────────────────────────────
-- Registra en AuditoriaLogs cada inserción de producto.
-- TipoOperacion = 1 (INSERT)
-- Usa char(34) para representar comillas dobles en SQLite
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

-- ── TRG_Producto_Update_Audit ─────────────────────────────────────────
-- Registra cambios en productos (solo si cambia Nombre, StockActual o PrecioVentaActual).
-- TipoOperacion = 2 (UPDATE)
-- Condición WHEN evita registros innecesarios cuando solo cambia Activo u otros campos.
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

-- ═══════════════════════════════════════════════════════════════════════
-- TRIGGERS DE VALIDACIÓN
-- ═══════════════════════════════════════════════════════════════════════

-- ── TRG_Stock_Validate ───────────────────────────────────────────────
-- Previene stock negativo en Producto.
-- BEFORE UPDATE para interceptar ANTES de que se aplique el cambio.
-- Si StockActual < 0, aborta la transacción con mensaje de error.
DROP TRIGGER IF EXISTS TRG_Stock_Validate;
CREATE TRIGGER TRG_Stock_Validate
BEFORE UPDATE ON Producto
WHEN NEW.StockActual < 0
BEGIN
    SELECT RAISE(ABORT, 'Stock no puede ser negativo');
END;