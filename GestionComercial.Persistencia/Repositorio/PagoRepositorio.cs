using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class PagoRepositorio : RepositorioBase<Pago>, IPagoRepositorio
    {
        public PagoRepositorio(GestionComercialContext context) : base(context) { }

        /// <summary>
        /// Obtiene totales por método de pago filtrando por CAJA específica.
        /// IMPORTANTE: Filtra por Venta.Id_caja para solo incluir pagos de esta caja.
        /// </summary>
        public async Task<IEnumerable<(string Metodo, decimal Total)>> ObtenerTotalesPorMetodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, int? idCaja = null)
        {
            // Traer todos los pagos y agrupar en memoria (SQLite no soporta Sum en decimal)
            var query = _dbSet
                .Where(p => p.Venta.Id_sucursal == idSucursal
                         && p.Venta.Fecha >= desde
                         && p.Venta.Fecha <= hasta
                         && p.Venta.Estado == 2); // solo ventas pagadas

            // Agregar filtro de caja si se especifica
            if (idCaja.HasValue)
            {
                query = query.Where(p => p.Venta.Id_caja == idCaja.Value);
            }

            var pagos = await query
                .Include(p => p.MetodoPago)
                .ToListAsync();

            // Agrupar en memoria (funciona con decimal)
            var resultado = pagos
                .GroupBy(p => p.MetodoPago?.Nombre ?? "Sin método")
                .Select(g => new
                {
                    Metodo = g.Key,
                    Total = g.Sum(p => p.Monto)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            return resultado.Select(x => (x.Metodo, x.Total));
        }

        public async Task<IEnumerable<Pago>> ObtenerPagosPorPeriodoAsync(DateTime desde, DateTime hasta)
            => await _dbSet
                .Where(p => p.Venta.Fecha >= desde && p.Venta.Fecha <= hasta)
                .Include(p => p.MetodoPago)
                .Include(p => p.Venta)
                .ToListAsync();
    }
}
