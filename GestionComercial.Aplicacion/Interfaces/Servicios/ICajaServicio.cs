using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Dominio.Entidades.Caja;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface ICajaServicio
    {
        Task<Caja?>           ObtenerCajaAbiertaAsync(int idSucursal);
        Task<Caja>            AbrirCajaAsync(int idSucursal, int idUsuario, decimal montoInicial, string? turno = null, bool esPrimaria = false);
        Task<Caja>            CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal);
        Task                  RegistrarMovimientoAsync(int idCaja, TipoMovimientoCajaEnum tipo, decimal monto, string descripcion);
        Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta);

        ///         /// Obtiene los movimientos de una caja para mostrar en la UI.
        Task<IEnumerable<MovimientoCajaDto>> ObtenerMovimientosAsync(int idCaja);

        ///         /// Obtiene las ventas del día para una caja.
        Task<IEnumerable<VentaDto>> ObtenerVentasDelDiaAsync(int idCaja);

        ///         /// Obtiene el desglose de pagos por método para una caja.
        Task<IEnumerable<DesglosePagoDto>> ObtenerDesglosePorMetodoAsync(int idCaja);

        ///         /// Calcula automáticamente el resumen del turno separando efectivo
        /// de otros métodos de pago. Usar antes de mostrar el cierre.
        Task<ResumenCierreDto> ObtenerResumenCierreAsync(int idCaja);

        ///         /// Registra la auditoría del cierre de caja (diferencia, modo, etc.)
        Task RegistrarAuditoriaCierreAsync(int idCaja, int idUsuario, string datosAuditoriaJson, decimal montoFinal, decimal diferencia);

        ///         /// Obtiene el total de efectivo recibido por caja desde las ventas.
        /// Usado para cierre automático de caja.
        Task<decimal> ObtenerTotalEfectivoPorCajaAsync(int idCaja);
    }
}
