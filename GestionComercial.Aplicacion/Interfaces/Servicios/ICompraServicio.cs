using GestionComercial.Aplicacion.DTOs.Compras;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface ICompraServicio
    {
        Task<IEnumerable<CompraDto>> ObtenerPorSucursalAsync(int idSucursal);
        Task<CompraDto?>             ObtenerPorIdAsync(int id);
        Task<CompraDto>              CrearAsync(CompraCrearDto dto);
    }
}
