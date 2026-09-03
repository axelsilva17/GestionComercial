using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IVentaRepostorio : IRepositorioBase<Venta>
    {
        Task<Venta?> ObtenerConDetallesAsync(int idVenta, CancellationToken ct = default);
        Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal, CancellationToken ct = default);
        Task<IEnumerable<Venta>> ObtenerPorClienteAsync(int idCliente, CancellationToken ct = default);
        Task<IEnumerable<Venta>> ObtenerConDetallesPorFechaAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);
        Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal, CancellationToken ct = default);
        
        ///         /// Obtiene ventas anuladas en un período para auditoría de fraude.
        Task<IEnumerable<Venta>> ObtenerVentasAnuladasAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);
        
        ///         /// Obtiene ventas por período con pagos incluidos para análisis.
        Task<IEnumerable<Venta>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de top productos (sin cargar entidades a memoria).
        Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>> 
            ObtenerTopProductosAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta, int top, CancellationToken ct = default);

        ///         /// Agregación SQL de top productos por empresa (sin cargar entidades a memoria).
        Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>> 
            ObtenerTopProductosPorEmpresaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, int top, CancellationToken ct = default);

        ///         /// Agregación SQL de rotación por producto (sin cargar entidades a memoria).
        Task<List<(int IdProducto, string Nombre, string Categoria, decimal StockActual, int CantidadVendida, DateTime? UltimaVenta)>> 
            ObtenerRotacionProductosAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de ventas agrupadas por día.
        Task<List<(string Dia, decimal Total, int Cantidad)>> 
            ObtenerVentasPorDiaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de ventas agrupadas por sucursal.
        Task<List<(int IdSucursal, string SucursalNombre, decimal Total, int Cantidad)>> 
            ObtenerVentasPorSucursalAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de ventas agrupadas por vendedor.
        Task<List<(int IdUsuario, string UsuarioNombre, string SucursalNombre, int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos)>> 
            ObtenerVentasPorVendedorAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de ventas por vendedor filtrando por Id_usuario.
        Task<List<(int IdUsuario, string UsuarioNombre, string SucursalNombre, int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos)>> 
            ObtenerVentasPorVendedorAsync(int idSucursal, int idUsuario, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de KPIs de ventas (totales, ticket promedio).
        Task<(decimal TotalVentas, int TotalTransacciones, decimal TicketPromedio)?> 
            ObtenerKpisVentasAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL del vendedor con mayor total de ventas.
        Task<string?> ObtenerTopVendedorAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL del producto con mayor total de ingresos.
        Task<string?> ObtenerTopProductoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Obtiene ventas de una caja específica con proyección ligera (sin includes innecesarios).
        Task<IEnumerable<(int Id, decimal TotalFinal, int Estado, int? IdCaja, decimal? EfectivoRecibido)>>
            ObtenerVentasLigerasPorCajaAsync(int idCaja, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Obtiene pagos agrupados por método de una caja específica.
        Task<IEnumerable<(string Metodo, decimal Total, int Cantidad)>>
            ObtenerPagosPorCajaAsync(int idCaja, CancellationToken ct = default);
    }
}