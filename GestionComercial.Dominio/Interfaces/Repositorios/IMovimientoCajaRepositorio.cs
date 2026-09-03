using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IMovimientoCajaRepositorio : IRepositorioBase<TipoMovimientoCaja>
    {
        Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorCajaAsync(int idCaja, CancellationToken ct = default);
        
        ///         /// Obtiene todos los movimientos de caja en un período para análisis de auditoría.
        Task<IEnumerable<TipoMovimientoCaja>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);
    }
}
