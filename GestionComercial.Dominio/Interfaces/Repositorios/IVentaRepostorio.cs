using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IVentaRepostorio : IRepositorioBase<Venta>
    {
        Task<Venta?> ObtenerConDetallesAsync(int idVenta);
        Task<IEnumerable<Venta>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal);
        Task<IEnumerable<Venta>> ObtenerPorClienteAsync(int idCliente);
        Task<IEnumerable<Venta>> ObtenerConDetallesPorFechaAsync(int idEmpresa, DateTime desde, DateTime hasta);
        Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal);
        
        ///         /// Obtiene ventas anuladas en un período para auditoría de fraude.
        Task<IEnumerable<Venta>> ObtenerVentasAnuladasAsync(DateTime desde, DateTime hasta);
        
        ///         /// Obtiene ventas por período con pagos incluidos para análisis.
        Task<IEnumerable<Venta>> ObtenerPorPeriodoAsync(DateTime desde, DateTime hasta);

        ///         /// Agregación SQL de top productos (sin cargar entidades a memoria).
        Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>> 
            ObtenerTopProductosAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta, int top);

        ///         /// Agregación SQL de top productos por empresa (sin cargar entidades a memoria).
        Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>> 
            ObtenerTopProductosPorEmpresaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, int top);

        ///         /// Agregación SQL de rotación por producto (sin cargar entidades a memoria).
        Task<List<(int IdProducto, string Nombre, string Categoria, decimal StockActual, int CantidadVendida, DateTime? UltimaVenta)>> 
            ObtenerRotacionProductosAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta);

        ///         /// Agregación SQL de ventas agrupadas por día.
        Task<List<(string Dia, decimal Total, int Cantidad)>> 
            ObtenerVentasPorDiaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta);

        ///         /// Agregación SQL de ventas agrupadas por sucursal.
        Task<List<(int IdSucursal, string SucursalNombre, decimal Total, int Cantidad)>> 
            ObtenerVentasPorSucursalAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta);

        ///         /// Agregación SQL de ventas agrupadas por vendedor.
        Task<List<(int IdUsuario, string UsuarioNombre, string SucursalNombre, int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos)>> 
            ObtenerVentasPorVendedorAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta);

        ///         /// Agregación SQL de KPIs de ventas (totales, ticket promedio).
        Task<(decimal TotalVentas, int TotalTransacciones, decimal TicketPromedio)?> 
            ObtenerKpisVentasAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta);

        ///         /// Agregación SQL del vendedor con mayor total de ventas.
        Task<string?> ObtenerTopVendedorAsync(int idEmpresa, DateTime desde, DateTime hasta);

        ///         /// Agregación SQL del producto con mayor total de ingresos.
        Task<string?> ObtenerTopProductoAsync(int idEmpresa, DateTime desde, DateTime hasta);

        ///         /// Obtiene ventas de una caja específica con proyección ligera (sin includes innecesarios).
        Task<IEnumerable<(int Id, decimal TotalFinal, int Estado, int? IdCaja, decimal? EfectivoRecibido)>>
            ObtenerVentasLigerasPorCajaAsync(int idCaja, DateTime desde, DateTime hasta);

        ///         /// Obtiene pagos agrupados por método de una caja específica.
        Task<IEnumerable<(string Metodo, decimal Total, int Cantidad)>>
            ObtenerPagosPorCajaAsync(int idCaja);
    }
}