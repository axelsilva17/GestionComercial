using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Dominio.Entidades.Movimientos;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IStockServicio
    {
        Task<StockDto?>                    ObtenerStockAsync(int idProducto);
        Task                               AjustarStockAsync(int idProducto, int cantidad, string motivo, int idSucursal, int idUsuario);
        Task<IEnumerable<MovimientoStock>> ObtenerMovimientosAsync(int idProducto);
    }
}
