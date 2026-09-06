using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface ICajaRepositorio : IRepositorioBase<Caja>
    {
        Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal, CancellationToken ct = default);
        Task<Caja?> ObtenerConMovimientosAsync(int idCaja, CancellationToken ct = default);
        Task<bool> ExisteCajaAbiertaAsync(int idSucursal, CancellationToken ct = default);
        /// <summary>
        /// Obtiene el historial de cajas de la sucursal en el período (grafo completo).
        /// </summary>
        Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Obtiene las últimas cajas de la sucursal en el período con sus ventas incluidas
        /// y las navegaciones de usuario (apertura/cierre). Proyección ligera: solo `take` filas.
        /// </summary>
        Task<IEnumerable<Caja>> ObtenerUltimasCajasConVentasAsync(
            int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default);

        /// <summary>
        /// Cuenta las cajas abiertas en el período para la sucursal (total y cerradas),
        /// sin materializar el grafo.
        /// </summary>
        Task<(int Total, int Cerradas)> ObtenerConteoCajasPeriodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Obtiene historial de cajas con proyección ligera y límite (Take) en SQL.
        /// Evita cargar el grafo completo (Ventas, Movimientos, navegaciones).
        /// </summary>
        Task<List<CajaHistorialDto>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default);

        /// <summary>
        /// Historial de cajas con proyección ligera para exportación Excel.
        /// No materializa el grafo completo (Ventas, Movimientos, navegaciones).
        /// </summary>
        Task<List<CajaExportRow>> ObtenerHistorialExportAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Ventas de las cajas del período con proyección ligera (solo CajaId, Fecha, TotalFinal).
        /// No carga Pagos, Usuario ni Movimientos.
        /// </summary>
        Task<List<VentaExportRow>> ObtenerVentasExportPorCajaAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Obtiene las cajas de una sucursal filtradas por turno.
        /// </summary>
        Task<List<Caja>> ObtenerCajasPorTurnoAsync(int idSucursal, string turno, CancellationToken ct = default);

        /// <summary>
        /// Verifica si existe una caja abierta para un turno específico en una sucursal.
        /// </summary>
        Task<bool> ExisteCajaAbiertaEnTurnoAsync(int idSucursal, string turno, CancellationToken ct = default);

        /// <summary>
        /// Obtiene la caja abierta para un turno específico en una sucursal (si existe).
        /// </summary>
        Task<Caja?> ObtenerCajaAbiertaPorSucursYTurnoAsync(int idSucursal, string turno, CancellationToken ct = default);

        /// <summary>
        /// Historial de cajas con proyección ligera para la pantalla de auditoría.
        /// Reemplaza el grafo completo (Ventas, Movimientos y navegaciones) por columnas
        /// planas: mismas filas filtradas, sin materializar entidades relacionadas.
        /// </summary>
        Task<List<CajaAuditoriaRow>> ObtenerHistorialAuditoriaAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);
    }

    /// <summary>
    /// DTO ligero para historial de cajas (proyección sin grafo completo).
    /// </summary>
    public class CajaHistorialDto
    {
        public int Id { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int Estado { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal? SaldoFinal { get; set; }
        public string SucursalNombre { get; set; } = string.Empty;
        public string UsuarioApertura { get; set; } = string.Empty;
    }

    /// <summary>
    /// Fila de caja para exportación Excel (proyección ligera, sin grafo completo).
    /// </summary>
    public record CajaExportRow(
        int Id,
        DateTime FechaApertura,
        DateTime? FechaCierre,
        decimal MontoInicial,
        decimal? MontoFinal,
        int Estado,
        string? UsuarioAperturaNombre,
        string? UsuarioCierreNombre);

    /// <summary>
    /// Fila de venta por caja para exportación Excel (proyección ligera).
    /// </summary>
    public record VentaExportRow(int CajaId, DateTime Fecha, decimal TotalFinal);

    /// <summary>
    /// Fila de caja para la pantalla de auditoría (proyección ligera, sin grafo completo).
    /// </summary>
    public record CajaAuditoriaRow(
        int Id,
        DateTime FechaApertura,
        DateTime? FechaCierre,
        decimal MontoInicial,
        decimal? MontoFinal,
        int Estado,
        string? Turno,
        string? UsuarioAperturaNombre,
        string? UsuarioCierreNombre);
}
