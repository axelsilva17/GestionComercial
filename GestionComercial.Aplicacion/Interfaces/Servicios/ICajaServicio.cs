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
    }
}
