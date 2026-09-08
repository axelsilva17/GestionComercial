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

        /// <summary>
        /// Client sales history with a light projection: only the columns the history list
        /// displays (Id, Fecha, TotalFinal, Estado, ClienteNombre, UsuarioNombre), filtered in SQL
        /// by client and date range, with AsNoTracking and an OrderByDescending(Fecha).Take(top) cap.
        /// Avoids hydrating the full Venta graph (Detalles/Pagos navigation properties) and avoids
        /// loading the whole sucursal just to filter a single client in memory.
        /// </summary>
        Task<List<VentaHistorialClienteRow>> ObtenerHistorialPorClienteAsync(
            int idCliente, DateTime desde, DateTime hasta, int top, CancellationToken ct = default);
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
        /// `top` opcional: null devuelve todos los productos (sin LIMIT).
        Task<List<(int IdProducto, string Nombre, string Categoria, int Cantidad, decimal Ingresos, decimal Costo, DateTime? UltimaFecha)>> 
            ObtenerTopProductosPorEmpresaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, int? top = null, CancellationToken ct = default);

        ///         /// Agregación SQL de rotación por producto (sin cargar entidades a memoria).
        /// `top` opcional: null devuelve todos los productos (sin LIMIT).
        Task<List<(int IdProducto, string Nombre, string Categoria, decimal StockActual, int CantidadVendida, DateTime? UltimaVenta)>> 
            ObtenerRotacionProductosAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, int? top = null, CancellationToken ct = default);

        ///         /// Agregación SQL de ventas agrupadas por día.
        Task<List<(string Dia, decimal Total, int Cantidad)>> 
            ObtenerVentasPorDiaAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de ventas agrupadas por sucursal.
        Task<List<(int IdSucursal, string SucursalNombre, decimal Total, int Cantidad)>> 
            ObtenerVentasPorSucursalAgrupadoAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de ventas agrupadas por vendedor.
        Task<List<(int IdUsuario, string UsuarioNombre, string SucursalNombre, int CantidadVentas, decimal TotalVendido, decimal TotalDescuentos)>> 
            ObtenerVentasPorVendedorAgrupadoAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Individual sales for a specific vendor (no aggregation), ordered by Fecha DESC.
        Task<List<(int Id, int IdUsuario, string UsuarioNombre, DateTime Fecha, decimal TotalFinal, int Estado, string ClienteNombre)>> 
            ObtenerVentasPorVendedorAsync(int idSucursal, int idUsuario, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Agregación SQL de KPIs de ventas (totales, ticket promedio).
        Task<(decimal TotalVentas, int TotalTransacciones, decimal TicketPromedio)?> 
            ObtenerKpisVentasAsync(int idEmpresa, int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Cuenta los clientes distintos que tuvieron ventas en la sucursal y período.
        /// No filtra por Estado para replicar la semántica in-memory original
        /// (que contaba clientes sobre todas las ventas del período, incluidas las anuladas).
        /// </summary>
        Task<int> ObtenerClientesUnicosAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Resumen de ventas por sucursal y período (total, cantidad y promedio) como agregación SQL.
        /// No filtra por Estado para replicar la semántica original que incluía las anuladas.
        /// </summary>
        Task<(decimal TotalVentas, int CantidadVentas, decimal PromedioVenta)>
            ObtenerResumenVentasPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

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

    /// <summary>
    /// Light row for a single client's sales history list (projection without the Venta graph).
    /// </summary>
    public record VentaHistorialClienteRow(
        int Id,
        DateTime Fecha,
        decimal TotalFinal,
        int Estado,
        int IdCliente,
        string ClienteNombre,
        string UsuarioNombre);
}