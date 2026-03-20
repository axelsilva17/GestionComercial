using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class PagoRepositorio : RepositorioBase<Pago>, IPagoRepositorio
    {
        public PagoRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<(string Metodo, decimal Total)>> ObtenerTotalesPorMetodoAsync(
            int idSucursal, DateTime desde, DateTime hasta)
        {
            var resultado = await _dbSet
                .Where(p => p.Venta.Id_sucursal == idSucursal
                         && p.Venta.Fecha >= desde
                         && p.Venta.Fecha <= hasta
                         && p.Venta.Estado == 2) // solo ventas pagadas
                .Include(p => p.MetodoPago)
                .GroupBy(p => p.MetodoPago.Nombre)
                .Select(g => new
                {
                    Metodo = g.Key,
                    Total  = g.Sum(p => p.Monto),
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();

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
