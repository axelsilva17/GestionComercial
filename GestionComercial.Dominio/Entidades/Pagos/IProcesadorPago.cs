using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Dominio.Entidades.Pagos
{
    public interface IProcesadorPago
    {
        Task ProcesarPagoAsync(Pago pago, Ventas.Venta venta, IUnitOfWork uow);
        bool AfectaCajaFisica { get; }
        string Categoria { get; }
    }
}
