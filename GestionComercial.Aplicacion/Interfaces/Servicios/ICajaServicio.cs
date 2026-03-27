using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Dominio.Entidades.Caja;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface ICajaServicio
    {
        Task<Caja?>           ObtenerCajaAbiertaAsync(int idSucursal);
        Task<Caja>            AbrirCajaAsync(int idSucursal, int idUsuario, decimal montoInicial);
        Task<Caja>            CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal);
        Task                  RegistrarMovimientoAsync(int idCaja, TipoMovimientoCajaEnum tipo, decimal monto, string descripcion);
        Task<IEnumerable<Caja>> ObtenerHistorialAsync(int idSucursal, DateTime desde, DateTime hasta);

        /// <summary>
        /// Calcula automáticamente el resumen del turno separando efectivo
        /// de otros métodos de pago. Usar antes de mostrar el cierre.
        /// </summary>
        Task<ResumenCierreDto> ObtenerResumenCierreAsync(int idCaja);

        /// <summary>
        /// Registra la auditoría del cierre de caja (diferencia, modo, etc.)
        /// </summary>
        Task RegistrarAuditoriaCierreAsync(int idCaja, int idUsuario, string datosAuditoriaJson, decimal montoFinal, decimal diferencia);

        /// <summary>
        /// Obtiene el total de efectivo recibido por caja desde las ventas.
        /// Usado para cierre automático de caja.
        /// </summary>
        Task<decimal> ObtenerTotalEfectivoPorCajaAsync(int idCaja);
    }
}
