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

        public async Task<Caja?> ObtenerConMovimientosAsync(int idCaja)
            => await _dbSet
                .Include(c => c.Movimientos)
                .Include(c => c.UsuarioApertura)
                .Include(c => c.UsuarioCierre)
                .FirstOrDefaultAsync(c => c.Id == idCaja);
    }
}
