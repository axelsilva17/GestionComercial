using GestionComercial.Dominio.Entidades.Descuento;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IDescuentoConfiguracionServicio
    {
        Task<DescuentoConfiguracion> CrearAsync(
            int idEmpresa, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto);

        Task<DescuentoConfiguracion?> ObtenerPorIdAsync(int id);

        Task ActualizarAsync(
            int id, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto);

        Task EliminarAsync(int id);

        Task<List<DescuentoConfiguracion>> ObtenerTodosAsync(int idEmpresa, bool? activo = null, string? texto = null);

        Task<DescuentoConfiguracion?> ObtenerDescuentoAplicableAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<int> idsMetodosPago, bool esPagoUnico,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Entidades.Producto.Categoria> categoriasCache);

        Task<DescuentoConfiguracion?> ObtenerDescuentoProductoAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Entidades.Producto.Categoria> categoriasCache);

        Task<DescuentoConfiguracion?> ObtenerDescuentoTotalVentaAsync(
            int idEmpresa,
            int idMetodoPago,
            List<DescuentoConfiguracion> descuentosCache);
    }
}