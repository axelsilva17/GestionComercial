using GestionComercial.Aplicacion.DTOs.Ventas;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IVentaServicio
    {
        Task<IEnumerable<VentaResumenDto>> ObtenerPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta);
        Task<VentaDto?>   ObtenerPorIdAsync(int id);
        Task<VentaDto>    CrearAsync(VentaCrearDto dto);
        Task              RegistrarPagoAsync(int idVenta, List<PagoItemDto> pagos); // ← nuevo
        Task              CancelarAsync(int id);
        Task<decimal>     ObtenerTotalDelDiaAsync(int idSucursal);
    }
}
