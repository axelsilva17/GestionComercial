using GestionComercial.Dominio.Entidades.Descuento;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IDescuentoConfiguracionServicio
    {
        Task<DescuentoConfiguracion> CrearAsync(
            int idEmpresa, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto,
            decimal? montoMinimoCompra = null,
            CancellationToken ct = default);

        Task<DescuentoConfiguracion?> ObtenerPorIdAsync(int id, CancellationToken ct = default);

        Task ActualizarAsync(
            int id, string nombre, decimal valor,
            int? idProducto, int? idCategoria,
            bool aplicaCualquierMetodoPago, List<int>? idsMetodosPago,
            DateTime? fechaDesde, DateTime? fechaHasta,
            AlcanceDescuentoEnum alcance = AlcanceDescuentoEnum.Producto,
            decimal? montoMinimoCompra = null,
            CancellationToken ct = default);

        Task EliminarAsync(int id, CancellationToken ct = default);

        Task ActivarAsync(int id, CancellationToken ct = default);

        Task<List<DescuentoConfiguracion>> ObtenerTodosAsync(int idEmpresa, bool? activo = null, string? texto = null, CancellationToken ct = default);

        Task<DescuentoConfiguracion?> ObtenerDescuentoAplicableAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<int> idsMetodosPago, bool esPagoUnico,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Entidades.Producto.Categoria> categoriasCache,
            CancellationToken ct = default);

        Task<DescuentoConfiguracion?> ObtenerDescuentoProductoAsync(
            int idEmpresa, int? idProducto, int? idCategoria,
            List<DescuentoConfiguracion> descuentosCache,
            Dictionary<int, Entidades.Producto.Categoria> categoriasCache,
            CancellationToken ct = default);

        Task<DescuentoConfiguracion?> ObtenerDescuentoTotalVentaAsync(
            int idEmpresa,
            int idMetodoPago,
            List<DescuentoConfiguracion> descuentosCache,
            CancellationToken ct = default);

        Task<DescuentoConfiguracion?> ObtenerDescuentoCompraMayorAsync(
            int idEmpresa,
            decimal totalVenta,
            List<DescuentoConfiguracion> descuentosCache,
            CancellationToken ct = default);
    }
}