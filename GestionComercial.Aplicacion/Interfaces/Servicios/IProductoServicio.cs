using GestionComercial.Aplicacion.DTOs.Productos;

using GestionComercial.Dominio.Entidades.Proveedores;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
public interface IProductoServicio
    {
        Task<IEnumerable<ProductoListadoDto>> ObtenerTodosAsync(int idEmpresa);
        Task<IEnumerable<ProductoListadoDto>> ObtenerStockCriticoAsync(int idEmpresa);
        Task<ProductoDto?>                    ObtenerPorIdAsync(int id);
        Task<ProductoDto>                     CrearAsync(ProductoCrearDto dto);
        Task                                  ActualizarAsync(ProductoActualizarDto dto);
        Task<(ProductoDto Producto, bool FueActualizacion)> CrearOActualizarAsync(ProductoImportarDto dto, bool actualizarExistentes);
        // Nuevo: ajuste de precios por proveedor (global por empresa del proveedor)
        Task<(int Nuevos, int Actualizados)> AjustePreciosPorProveedorAsync(int idProveedor, decimal porcentaje);
        Task<(int Nuevos, int Actualizados, int Omitidos)> ImportarMasivoAsync(IEnumerable<ProductoImportarDto> dtos, bool actualizarExistentes, IProgress<(int current, int total, string message)>? progreso = null);
        Task                                  DesactivarAsync(int id);

        // Reference data
        Task<IEnumerable<CategoriaItemDto>>   ObtenerCategoriasAsync(int idEmpresa);
        Task<IEnumerable<UnidadMedidaItemDto>> ObtenerUnidadesMedidaAsync();
        Task<IEnumerable<Proveedor>> ObtenerProveedoresAsync();
    }
}
