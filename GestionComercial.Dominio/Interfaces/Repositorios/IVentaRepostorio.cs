
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;

public interface IVentaRepostorio : IRepositorioBase<Venta>
{
    Task<Venta?> ObtenerConDetallesAsync(int idVenta);
    Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal);
    Task<IEnumerable<Venta>> ObtenerPorClienteAsync(int idCliente);
    Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal);
}