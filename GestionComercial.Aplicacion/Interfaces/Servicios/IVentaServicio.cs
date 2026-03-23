using GestionComercial.Aplicacion.DTOs.Ventas;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IVentaServicio
    {
        Task<IEnumerable<VentaResumenDto>> ObtenerPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta);
        Task<IEnumerable<VentaResumenDto>> ObtenerVentasAsync(
            int idSucursal,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null,
            string? dniCliente = null,
            int? estado = null);
        Task<VentaDto?>   ObtenerPorIdAsync(int id);
        Task<VentaDto>    CrearAsync(VentaCrearDto dto);
        Task              RegistrarPagoAsync(int idVenta, List<PagoItemDto> pagos);
        Task              CancelarAsync(int id, string motivo);
        Task<decimal>     ObtenerTotalDelDiaAsync(int idSucursal);
    }
}
