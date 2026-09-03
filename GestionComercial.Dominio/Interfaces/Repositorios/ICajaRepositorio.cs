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
        Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Obtiene historial de cajas con proyección ligera y límite (Take) en SQL.
        /// Evita cargar el grafo completo (Ventas, Movimientos, navegaciones).
        /// </summary>
        Task<List<CajaHistorialDto>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default);

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
}
