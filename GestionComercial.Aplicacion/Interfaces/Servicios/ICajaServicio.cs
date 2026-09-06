using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Threading;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface ICajaServicio
    {
        Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal, CancellationToken ct = default);
        Task<Caja> AbrirCajaAsync(int idSucursal, int idUsuario, decimal montoInicial, TurnoCajaEnum? turno = null, bool esPrimaria = false, CancellationToken ct = default);
        Task<Caja> CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal, CancellationToken ct = default);
        Task RegistrarMovimientoAsync(int idCaja, TipoMovimientoCajaEnum tipo, decimal monto, string descripcion, CancellationToken ct = default);
        Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Obtiene las últimas cajas de la sucursal en el período con sus ventas incluidas.
        /// Proyección ligera para consumo en reportes (evita materializar todo el grafo).
        /// </summary>
        Task<IEnumerable<Dominio.Entidades.Caja.Caja>> ObtenerUltimasCajasConVentasAsync(
            int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default);

        /// <summary>
        /// Resumen SQL agregado de ingresos/egresos de caja para una sucursal en el período.
        /// </summary>
        Task<(decimal Ingresos, decimal Egresos)> ObtenerResumenMoviCajaAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Conteo de cajas de la sucursal abiertas en el período (total y cerradas).
        /// </summary>
        Task<(int Total, int Cerradas)> ObtenerConteoCajasPeriodoAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        // Nuevo: historial con proyección ligera y Take en SQL
        Task<List<CajaHistorialDto>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta, int take, CancellationToken ct = default);

        /// <summary>
        /// Historial de cajas con proyección ligera para exportación Excel (sin grafo completo).
        /// </summary>
        Task<List<CajaExportRow>> ObtenerHistorialExportAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Ventas por caja con proyección ligera para exportación Excel (sin Pagos/Usuario/Movimientos).
        /// </summary>
        Task<List<VentaExportRow>> ObtenerVentasExportPorCajaAsync(int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Obtiene los movimientos de una caja para mostrar en la UI.
        Task<IEnumerable<MovimientoCajaDto>> ObtenerMovimientosAsync(int idCaja, CancellationToken ct = default);

        ///         /// Obtiene las ventas del día para una caja.
        Task<IEnumerable<VentaDto>> ObtenerVentasDelDiaAsync(int idCaja, CancellationToken ct = default);

        ///         /// Obtiene el desglose de pagos por método para una caja.
        Task<IEnumerable<DesglosePagoDto>> ObtenerDesglosePorMetodoAsync(int idCaja, CancellationToken ct = default);

        ///         /// Calcula automáticamente el resumen del turno separando efectivo
        /// de otros métodos de pago. Usar antes de mostrar el cierre.
        Task<ResumenCierreDto> ObtenerResumenCierreAsync(int idCaja, CancellationToken ct = default);

        ///         /// Calcula la diferencia entre el conteo físico (MontoFinal) y el saldo esperado.
        /// Devuelve 0 si la caja sigue abierta (sin MontoFinal).
        Task<decimal> ObtenerDiferenciaCierreAsync(int idCaja, CancellationToken ct = default);

        /// <summary>
        /// Historial de cajas con proyección ligera para la pantalla de auditoría
        /// (sin ventas, movimientos ni navegaciones de usuario).
        /// </summary>
        Task<List<CajaAuditoriaRow>> ObtenerHistorialAuditoriaAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        /// <summary>
        /// Resumen centralizado (ventas en efectivo, ingresos/egresos manuales, saldo esperado
        /// y diferencia) de TODAS las cajas de la sucursal abiertas en el período, calculado con
        /// agregación SQL en lote. Reemplaza el N+1 de ObtenerResumenCierreAsync +
        /// ObtenerDiferenciaCierreAsync en las pantallas de auditoría.
        /// </summary>
        Task<List<CajaAuditoriaResumenDto>> ObtenerAuditoriaResumenesAsync(
            int idSucursal, DateTime desde, DateTime hasta, CancellationToken ct = default);

        ///         /// Registra la auditoría del cierre de caja (diferencia, modo, etc.)
        Task RegistrarAuditoriaCierreAsync(int idCaja, int idUsuario, string datosAuditoriaJson, decimal montoFinal, decimal diferencia, CancellationToken ct = default);

        ///         /// Obtiene el total de efectivo recibido por caja desde las ventas.
        /// Usado para cierre automático de caja.
        Task<decimal> ObtenerTotalEfectivoPorCajaAsync(int idCaja, CancellationToken ct = default);

        ///         /// Elimina una caja (soft delete). No permite eliminar cajas primarias ni cajas abiertas.
        Task EliminarCajaAsync(int idCaja, CancellationToken ct = default);
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
    /// Resumen de una caja para la pantalla de auditoría (agregación SQL en lote).
    /// SaldoEsperado = MontoInicial + VentasEfectivo + IngresosEfectivo - EgresosEfectivo;
    /// Diferencia = MontoFinal - SaldoEsperado (0 si la caja sigue abierta).
    /// </summary>
    public record CajaAuditoriaResumenDto(
        int IdCaja,
        decimal VentasEfectivo,
        decimal IngresosEfectivo,
        decimal EgresosEfectivo,
        decimal SaldoEsperado,
        decimal Diferencia);
}
