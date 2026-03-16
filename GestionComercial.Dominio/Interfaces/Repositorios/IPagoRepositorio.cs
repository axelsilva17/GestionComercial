using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IPagoRepositorio : IRepositorioBase<Pago>
    {
        /// <summary>
        /// Agrupa los pagos por método de pago en un rango de fechas para una sucursal.
        /// Retorna lista de (NombreMetodo, MontoTotal).
        /// </summary>
        Task<IEnumerable<(string Metodo, decimal Total)>> ObtenerTotalesPorMetodoAsync(
            int idSucursal, DateTime desde, DateTime hasta);
    }
}
