using GestionComercial.Aplicacion.DTOs.Productos;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IProductoServicio
    {
        Task<IEnumerable<ProductoListadoDto>> ObtenerTodosAsync(int idEmpresa);
        Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa);
        Task<ProductoDto?>                    ObtenerPorIdAsync(int id);
        Task<ProductoDto>                     CrearAsync(ProductoCrearDto dto);
        Task                                  ActualizarAsync(ProductoActualizarDto dto);
        Task                                  DesactivarAsync(int id);

        // Reference data
        Task<IEnumerable<CategoriaItemDto>>   ObtenerCategoriasAsync(int idEmpresa);
        Task<IEnumerable<UnidadMedidaItemDto>> ObtenerUnidadesMedidaAsync();
    }
}
