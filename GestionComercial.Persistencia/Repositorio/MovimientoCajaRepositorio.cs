using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class MovimientoCajaRepositorio : RepositorioBase<TipoMovimientoCaja>, IMovimientoCajaRepositorio
    {
        public MovimientoCajaRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorCajaAsync(int idCaja)
            => await _dbSet
                .Where(m => m.Id_caja == idCaja)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

        public async Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta)
            => await _dbSet
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
    }
}
