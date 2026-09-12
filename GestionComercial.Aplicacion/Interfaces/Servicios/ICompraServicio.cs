using GestionComercial.Aplicacion.DTOs.Compras;
using System.Threading;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface ICompraServicio
    {
        Task<IEnumerable<CompraDto>> ObtenerPorSucursalAsync(int idSucursal, CancellationToken ct = default);
        Task<IEnumerable<CompraDto>> ObtenerPorPeriodoAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);
        Task<IEnumerable<CompraDto>> ObtenerPorProveedorAsync(int idProveedor, CancellationToken ct = default);
        Task<CompraDto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<CompraDto> CrearAsync(CompraCrearDto dto, CancellationToken ct = default);

        // Nuevo: compras paginadas para listado con filtro de fecha por defecto (últimos 30 días)
        Task<(IEnumerable<CompraDto> Items, int TotalCount)> ObtenerPorSucursalPaginadoAsync(
            int idSucursal, DateTime desde, DateTime hasta, int page, int pageSize,
            int? idProveedor = null, string? busquedaProveedor = null, CancellationToken ct = default);

        // Nuevo: métricas agregadas en SQL
        Task<MetricasComprasDto?> ObtenerMetricasComprasAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        // Totales de compras agrupados por mes para el reporte gerencial.
        Task<List<(int AnioMes, decimal Total)>> ObtenerComprasPorMesAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);
    }
}
