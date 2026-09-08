using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class MovimientoCajaRepositorio : RepositorioBase<TipoMovimientoCaja>, IMovimientoCajaRepositorio
    {
        public MovimientoCajaRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorCajaAsync(int idCaja, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(m => m.Id_caja == idCaja)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync(ct);

        public async Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync(ct);

        // Uses SqlQueryRaw with CAST(m.Monto AS REAL) to avoid SQLite's inability to handle
        // decimal SUM on large groups (same pattern as PagoRepositorio / VentaRepositorio).
        // Semantics preserved: movements with Tipo == 1 go to Ingresos; all others (Apertura,
        // Cierre, Egreso) go to Egresos, for cajas that belong to the sucursal and were opened
        // in the period.
        public async Task<(decimal Ingresos, decimal Egresos)> ObtenerResumenPorSucursalAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<MovimientoCajaResumenRaw>(
                    @"SELECT COALESCE(SUM(CASE WHEN mc.Tipo = 1 THEN CAST(mc.Monto AS REAL) ELSE 0 END), 0) AS Ingresos,
                             COALESCE(SUM(CASE WHEN mc.Tipo != 1 THEN CAST(mc.Monto AS REAL) ELSE 0 END), 0) AS Egresos
                      FROM MovimientoCaja mc
                      INNER JOIN Caja c ON mc.Id_caja = c.Id
                      WHERE c.Id_sucursal = {0}
                        AND c.FechaApertura >= {1}
                        AND c.FechaApertura <= {2}
                        AND c.Activo = 1",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);

            var r = rows.FirstOrDefault() ?? new MovimientoCajaResumenRaw(0, 0);
            return (r.Ingresos, r.Egresos);
        }

        /// <summary>
        /// Ingresos manuales (Tipo == 1 sin venta) y egresos (Tipo == 2) por caja en su turno.
        /// Replica la semántica de ObtenerPorCajaAsync para el resumen de auditoría: los
        /// movimientos de Ingreso por venta (con Id_venta) ya están contados en los pagos, y
        /// Apertura/Cierre son operativos y no afectan el saldo físico.
        /// </summary>
        public async Task<List<MovimientoCajaResumenPorCajaRow>> ObtenerResumenPorCajaEnPeriodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<MovimientoCajaResumenPorCajaRaw>(
                    @"SELECT mc.Id_caja AS IdCaja,
                             COALESCE(SUM(CASE WHEN mc.Tipo = 1 AND mc.Id_venta IS NULL THEN CAST(mc.Monto AS REAL) ELSE 0 END), 0) AS Ingresos,
                             COALESCE(SUM(CASE WHEN mc.Tipo = 2 THEN CAST(mc.Monto AS REAL) ELSE 0 END), 0) AS Egresos
                      FROM MovimientoCaja mc
                      INNER JOIN Caja c ON mc.Id_caja = c.Id
                      WHERE c.Id_sucursal = {0}
                        AND c.FechaApertura >= {1}
                        AND c.FechaApertura <= {2}
                        AND c.Activo = 1
                      GROUP BY mc.Id_caja",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);

            return rows.Select(r => new MovimientoCajaResumenPorCajaRow(r.IdCaja, r.Ingresos, r.Egresos)).ToList();
        }

        // Proyección ligera para exportar movimientos: el Select se traduce a un JOIN en SQL
        // (Caja + Usuario por nombre), sin materializar el grafo completo ni cargar Venta.
        // AsNoTracking + ordenado por Fecha descendente (mismo criterio que la vista del módulo).
        public async Task<List<MovimientoCajaExportRow>> ObtenerMovimientosExportAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(m => m.Caja.Id_sucursal == idSucursal
                            && m.Fecha >= desde && m.Fecha <= hasta)
                .OrderByDescending(m => m.Fecha)
                .Select(m => new MovimientoCajaExportRow(
                    m.Id,
                    m.Fecha,
                    m.Tipo,
                    m.Monto,
                    m.Concepto ?? "-",
                    m.Usuario != null ? m.Usuario.Nombre : "Sistema",
                    m.Id_caja))
                .ToListAsync(ct);
    }

    // ── Tipo para SqlQueryRaw (EF Core 8) ─────────────────────────
    public record MovimientoCajaResumenRaw(decimal Ingresos, decimal Egresos);

    // ── Tipo para SqlQueryRaw (resumen por caja) ─────────────────────
    public record MovimientoCajaResumenPorCajaRaw(int IdCaja, decimal Ingresos, decimal Egresos);
}