using GestionComercial.Dominio.Entidades.Caja;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface ICajaServicio
    {
        Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal);
        Task<Caja>  AbrirCajaAsync(int idSucursal, int idUsuario, decimal montoInicial);
        Task<Caja>  CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal);
        Task        RegistrarMovimientoAsync(int idCaja, TipoMovimientoCajaEnum tipo, decimal monto, string descripcion);
    }
}
