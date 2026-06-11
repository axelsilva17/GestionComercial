using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Dominio.Entidades.Pagos.Strategies
{
    public class TarjetaStrategy : IProcesadorPago
    {
        public bool AfectaCajaFisica => false;
        public string Categoria => "Tarjeta";

        public Task ProcesarPagoAsync(Pago pago, Ventas.Venta venta, IUnitOfWork uow)
        {
            return Task.CompletedTask;
        }
    }
}
