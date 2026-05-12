using GestionComercial.Aplicacion.DTOs.Compras;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface ICompraServicio
    {
        Task<IEnumerable<CompraDto>> ObtenerPorSucursalAsync(int idSucursal);
        Task<IEnumerable<CompraDto>> ObtenerPorPeriodoAsync(int idSucursal, DateTime desde, DateTime hasta);
        Task<IEnumerable<CompraDto>> ObtenerPorProveedorAsync(int idProveedor);
        Task<CompraDto?>             ObtenerPorIdAsync(int id);
        Task<CompraDto>              CrearAsync(CompraCrearDto dto);
    }
}
