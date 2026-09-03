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

        public async Task<IEnumerable<Pago>> ObtenerPagosPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                .Where(p => p.Venta.Fecha >= desde && p.Venta.Fecha <= hasta)
                .Include(p => p.MetodoPago)
                .Include(p => p.Venta)
                .ToListAsync(ct);

        // Tipo para SqlQueryRaw
        private record MetodoPagoTotalRaw(string Metodo, decimal Total, int Cantidad);
    }
}
