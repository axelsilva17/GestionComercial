using GestionComercial.Dominio.Interfaces;
using System.Threading;

namespace GestionComercial.Dominio.Entidades.Pagos
{
    public interface IProcesadorPago
    {
        Task ProcesarPagoAsync(Pago pago, Ventas.Venta venta, IUnitOfWork uow, CancellationToken ct = default);
        bool AfectaCajaFisica { get; }
        string Categoria { get; }
    }
}
