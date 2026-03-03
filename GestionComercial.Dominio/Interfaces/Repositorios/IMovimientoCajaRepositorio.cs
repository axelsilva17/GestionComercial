using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IMovimientoCajaRepositorio : IRepositorioBase<TipoMovimientoCaja>
    {
        Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorCajaAsync(int idCaja);
    }
}
