using GestionComercial.Dominio.Entidades.Descuento;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IDescuentoConfiguracionServicio
    {
        Task<DescuentoConfiguracion> CrearAsync(int idEmpresa, string nombre, TipoDescuentoEnum tipo, decimal valor, int? idProducto, int? idCategoria, int? idMetodoPago, DateTime? fechaDesde, DateTime? fechaHasta, int prioridad);
        Task<DescuentoConfiguracion?> ObtenerPorIdAsync(int id);
        Task ActualizarAsync(int id, string nombre, TipoDescuentoEnum tipo, decimal valor, int? idProducto, int? idCategoria, int? idMetodoPago, DateTime? fechaDesde, DateTime? fechaHasta, int prioridad);
        Task EliminarAsync(int id);
        Task<List<DescuentoConfiguracion>> ObtenerTodosAsync(int idEmpresa, TipoDescuentoEnum? tipo = null, bool? activo = null, string? texto = null);
        Task<DescuentoConfiguracion?> ObtenerDescuentoAplicableAsync(int idEmpresa, int? idProducto, int? idCategoria, List<DescuentoConfiguracion> descuentosCache, Dictionary<int, Entidades.Producto.Categoria> categoriasCache);
        Task<DescuentoConfiguracion?> ObtenerDescuentoMetodoPagoAsync(int idEmpresa, List<int> idsMetodosPago, List<DescuentoConfiguracion> descuentosCache);
    }
}
