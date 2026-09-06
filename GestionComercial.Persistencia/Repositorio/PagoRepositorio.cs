using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class PagoRepositorio : RepositorioBase<Pago>, IPagoRepositorio
    {
        public PagoRepositorio(GestionComercialContext context) : base(context) { }

        ///         /// Obtiene totales por método de pago filtrando por CAJA específica.
        /// IMPORTANTE: Filtra por Venta.Id_caja para solo incluir pagos de esta caja.
        /// Usa SqlQueryRaw con CAST a REAL para trabajar alrededor de la limitación de SQLite con SUM en decimal.
        public async Task<IEnumerable<(string Metodo, decimal Total, int Cantidad)>> ObtenerTotalesPorMetodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, int? idCaja = null, CancellationToken ct = default)
        {
            string sql;
            object[] parameters;

            if (idCaja.HasValue)
            {
                sql = @"
                    SELECT mp.Nombre AS Metodo,
                           SUM(CAST(p.Monto AS REAL)) AS Total,
                           COUNT(*) AS Cantidad
                    FROM Pago p
                    INNER JOIN Venta v ON p.Id_venta = v.Id
                    INNER JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id
                    WHERE v.Id_sucursal = {0}
                      AND v.Fecha >= {1}
                      AND v.Fecha <= {2}
                      AND v.Estado = 2
                      AND v.Id_caja = {3}
                    GROUP BY mp.Nombre
                    ORDER BY Total DESC";
                parameters = new object[] { idSucursal, desde, hasta, idCaja.Value };
            }
            else
            {
                sql = @"
                    SELECT mp.Nombre AS Metodo,
                           SUM(CAST(p.Monto AS REAL)) AS Total,
                           COUNT(*) AS Cantidad
                    FROM Pago p
                    INNER JOIN Venta v ON p.Id_venta = v.Id
                    INNER JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id
                    WHERE v.Id_sucursal = {0}
                      AND v.Fecha >= {1}
                      AND v.Fecha <= {2}
                      AND v.Estado = 2
                    GROUP BY mp.Nombre
                    ORDER BY Total DESC";
                parameters = new object[] { idSucursal, desde, hasta };
            }

            var rows = await _context.Database
                .SqlQueryRaw<MetodoPagoTotalRaw>(sql, parameters)
                .ToListAsync(ct);

            return rows.Select(r => (r.Metodo, r.Total, r.Cantidad));
        }

        /// <summary>
        /// Totales por método de pago agrupados por mes (año-mes) en el período.
        /// Una sola consulta SQL agrupada; reemplaza el N+1 mensual de la exportación de gerencia.
        /// </summary>
        public async Task<List<PagoMesMetodoRow>> ObtenerTotalesPorMetodoMensualAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<PagoMesMetodoRaw>(
                    @"SELECT CAST(strftime('%Y', v.Fecha) AS INTEGER) * 100 + CAST(strftime('%m', v.Fecha) AS INTEGER) AS AnioMes,
                             mp.Nombre AS Metodo,
                             SUM(CAST(p.Monto AS REAL)) AS Total,
                             COUNT(*) AS Cantidad
                      FROM Pago p
                      INNER JOIN Venta v ON p.Id_venta = v.Id
                      INNER JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id
                      WHERE v.Id_sucursal = {0}
                        AND v.Fecha >= {1}
                        AND v.Fecha <= {2}
                        AND v.Estado = 2
                      GROUP BY strftime('%Y-%m', v.Fecha), mp.Nombre
                      ORDER BY AnioMes ASC, Total DESC",
                    idSucursal, desde, hasta)
                .ToListAsync(ct);

            return rows.Select(r => new PagoMesMetodoRow(r.AnioMes, r.Metodo, r.Total, r.Cantidad)).ToList();
        }

        public async Task<IEnumerable<Pago>> ObtenerPagosPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(p => p.Venta.Fecha >= desde && p.Venta.Fecha <= hasta)
                .Include(p => p.MetodoPago)
                .Include(p => p.Venta)
                .ToListAsync(ct);

        /// <summary>
        /// Totales por método de pago agrupados por caja para todas las cajas activas de la
        /// sucursal abiertas en el período. Ventas del turno de cada caja (Estado = 2) entre
        /// la apertura de la caja y el momento actual — replica la semántica de
        /// ObtenerTotalesPorMetodoAsync(idCaja) sin el N+1.
        /// </summary>
        public async Task<List<PagoCajaMetodoRow>> ObtenerTotalesPorMetodoPorCajaAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default)
        {
            var rows = await _context.Database
                .SqlQueryRaw<PagoCajaMetodoRaw>(
                    @"SELECT v.Id_caja AS IdCaja,
                             mp.Nombre AS Metodo,
                             SUM(CAST(p.Monto AS REAL)) AS Total,
                             COUNT(*) AS Cantidad
                      FROM Pago p
                      INNER JOIN Venta v ON p.Id_venta = v.Id
                      INNER JOIN Caja c ON v.Id_caja = c.Id
                      INNER JOIN MetodoPago mp ON p.Id_metodoPago = mp.Id
                      WHERE c.Id_sucursal = {0}
                        AND c.FechaApertura >= {1}
                        AND c.FechaApertura <= {2}
                        AND c.Activo = 1
                        AND v.Id_sucursal = c.Id_sucursal
                        AND v.Estado = 2
                        AND v.Fecha >= c.FechaApertura
                        AND v.Fecha <= {3}
                      GROUP BY v.Id_caja, mp.Nombre
                      ORDER BY v.Id_caja ASC, Total DESC",
                    idSucursal, desde, hasta, DateTime.Now)
                .ToListAsync(ct);

            return rows.Select(r => new PagoCajaMetodoRow(r.IdCaja, r.Metodo, r.Total, r.Cantidad)).ToList();
        }

        // Tipo para SqlQueryRaw
        private record MetodoPagoTotalRaw(string Metodo, decimal Total, int Cantidad);

        // Tipo para SqlQueryRaw (agrupado por mes)
        private record PagoMesMetodoRaw(int AnioMes, string Metodo, decimal Total, int Cantidad);

        // Tipo para SqlQueryRaw (agrupado por caja)
        private record PagoCajaMetodoRaw(int IdCaja, string Metodo, decimal Total, int Cantidad);
    }
}
