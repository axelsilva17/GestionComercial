using GestionComercial.Dominio.Interfaces;
using System.Threading;

namespace GestionComercial.Dominio.Entidades.Pagos.Strategies
{
    public class OtroStrategy : IProcesadorPago
    {
        public bool AfectaCajaFisica => false;
        public string Categoria => "Otro";

        public Task ProcesarPagoAsync(Pago pago, Ventas.Venta venta, IUnitOfWork uow, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }
}