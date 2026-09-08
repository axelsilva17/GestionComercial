using GestionComercial.Aplicacion.DTOs.Ventas;
using System.Threading;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IVentaServicio
    {
        Task<IEnumerable<VentaResumenDto>> ObtenerPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);
        Task<IEnumerable<VentaResumenDto>> ObtenerVentasAsync(
            int idSucursal,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string? dniCliente = null,
            int? estado = null,
            CancellationToken ct = default);
        Task<VentaDto?>   ObtenerPorIdAsync(int id, CancellationToken ct = default);
        Task<VentaDto>    CrearAsync(VentaCrearDto dto, CancellationToken ct = default);
        Task              RegistrarPagoAsync(int idVenta, List<PagoItemDto> pagos, CancellationToken ct = default);
        Task              CobrarVentaAsync(int idVenta, CancellationToken ct = default);
        Task              CancelarAsync(int id, string motivo, CancellationToken ct = default);
        Task<decimal>     ObtenerTotalDelDiaAsync(int idSucursal, CancellationToken ct = default);

        // Nuevo: ventas por vendedor (filtrado por Id_usuario en SQL)
        Task<IEnumerable<VentaResumenDto>> ObtenerVentasPorVendedorAsync(
            int idSucursal, int idUsuario, DateTime desde, DateTime hasta, CancellationToken ct = default);

        // Per-client sales history: light SQL projection filtered by client and date range,
        // capped to the most recent `top` sales. Avoids loading the whole sucursal in memory.
        Task<IEnumerable<VentaResumenDto>> ObtenerHistorialPorClienteAsync(
            int idCliente, DateTime desde, DateTime hasta, int top, CancellationToken ct = default);
    }
}
