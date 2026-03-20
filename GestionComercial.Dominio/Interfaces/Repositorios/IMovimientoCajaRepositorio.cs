using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IMovimientoCajaRepositorio : IRepositorioBase<TipoMovimientoCaja>
    {
        Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorCajaAsync(int idCaja);
        
        /// <summary>
        /// Obtiene todos los movimientos de caja en un período para análisis de auditoría.
        /// </summary>
        Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta);
    }
}
