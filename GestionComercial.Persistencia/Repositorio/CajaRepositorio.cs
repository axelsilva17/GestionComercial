using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class CajaRepositorio : RepositorioBase<Caja>, ICajaRepositorio
    {
        public CajaRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal)
            => await _dbSet
                .Include(c => c.UsuarioApertura)
                .FirstOrDefaultAsync(c => c.Id_sucursal == idSucursal && c.Estado == 1);
        public async Task<bool> ExisteCajaAbiertaAsync(int idSucursal)
    => await _dbSet.AnyAsync(c => c.Id_sucursal == idSucursal && c.Estado == 1);

        public async Task<Caja?> ObtenerConMovimientosAsync(int idCaja)
            => await _dbSet
                .Include(c => c.Movimientos)
                .Include(c => c.UsuarioApertura)
                .Include(c => c.UsuarioCierre)
                .FirstOrDefaultAsync(c => c.Id == idCaja);

        public async Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta)
            => await _dbSet
                .Include(c => c.Ventas)
                .Include(c => c.Movimientos)
                    .ThenInclude(m => m.Usuario)
                .Include(c => c.UsuarioApertura)
                .Include(c => c.UsuarioCierre)
                .Where(c => c.Id_sucursal == idSucursal && c.FechaApertura >= desde && c.FechaApertura <= hasta)
                .OrderByDescending(c => c.FechaApertura)
                .ToListAsync();
    }
}
