using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface ICompraRepositorio : IRepositorioBase<Compra>
    {
        Task<Compra?> ObtenerConDetallesAsync(int idCompra, CancellationToken ct = default);
        Task<IEnumerable<Compra>> ObtenerPorProveedorAsync(int idProveedor, CancellationToken ct = default);
        Task<IEnumerable<Compra>> ObtenerPorSucursalAsync(int idSucursal, CancellationToken ct = default);
        Task<IEnumerable<Compra>> ObtenerPorPeriodoAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        // Nuevo: métricas agregadas en SQL para Compras
        Task<(decimal Total, int Count, decimal Promedio, string ProveedorTop, int ProductosRepuestos)?> 
            ObtenerMetricasComprasAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        // Nuevo: compras paginadas por sucursal y fecha
        Task<(IEnumerable<Compra> Items, int TotalCount)> ObtenerPorSucursalPaginadoAsync(
            int idSucursal, DateTime desde, DateTime hasta, int page, int pageSize,
            int? idProveedor = null, string? busquedaProveedor = null, CancellationToken ct = default);
    }
}