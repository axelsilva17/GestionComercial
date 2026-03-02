using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class VentaRepositorio : RepositorioBase<Venta>, IVentaRepostorio
    {
        public VentaRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Venta?> ObtenerConDetallesAsync(int idVenta)
            => await _dbSet
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(v => v.Pagos)
                    .ThenInclude(p => p.MetodoPago)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(v => v.Id == idVenta);

        public async Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal)
            => await _dbSet
                .Where(v => v.Fecha >= desde && v.Fecha <= hasta && v.Id_sucursal == idSucursal)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

        public async Task<IEnumerable<Venta>> ObtenerPorClienteAsync(int idCliente)
            => await _dbSet
                .Where(v => v.Id_cliente == idCliente)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

        public async Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal)
        {
            var hoy = DateTime.Today;
            return await _dbSet
                .Where(v => v.Id_sucursal == idSucursal
                         && v.Fecha >= hoy
                         && v.Estado == 2) // Pagada
                .SumAsync(v => v.TotalFinal);
        }
    }
}
