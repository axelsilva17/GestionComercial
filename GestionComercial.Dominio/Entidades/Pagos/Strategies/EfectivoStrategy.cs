using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Dominio.Entidades.Pagos.Strategies
{
    public class EfectivoStrategy : IProcesadorPago
    {
        public bool AfectaCajaFisica => true;
        public string Categoria => "Efectivo";

        public async Task ProcesarPagoAsync(Pago pago, Ventas.Venta venta, IUnitOfWork uow)
        {
            if (!venta.Id_caja.HasValue)
                return;

            var montoNeto = pago.Monto - pago.Vuelto;

            var movimiento = new TipoMovimientoCaja
            {
                Tipo         = (int)TipoMovimientoCajaEnum.Ingreso,
                Monto        = montoNeto,
                Concepto     = $"Venta #{venta.Id} (recibido: ${pago.Monto:N2}, vuelto: ${pago.Vuelto:N2})",
                ReferenciaId = venta.Id,
                Id_venta     = venta.Id,
                Id_caja      = venta.Id_caja.Value,
                Id_usuario   = venta.Id_usuario,
                Fecha        = DateTime.Now,
            };
            await uow.MovimientosCaja.AgregarAsync(movimiento);
            await uow.GuardarCambiosAsync();

            pago.Id_movimientoCaja = movimiento.Id;

            venta.EfectivoRecibido = (venta.EfectivoRecibido ?? 0) + pago.Monto;

            if (pago.Vuelto > 0)
            {
                var movimientoVuelto = new TipoMovimientoCaja
                {
                    Tipo         = (int)TipoMovimientoCajaEnum.Egreso,
                    Monto        = pago.Vuelto,
                    Concepto     = $"Vuelto venta #{venta.Id}",
                    ReferenciaId = venta.Id,
                    Id_venta     = venta.Id,
                    Id_caja      = venta.Id_caja.Value,
                    Id_usuario   = venta.Id_usuario,
                    Fecha        = DateTime.Now,
                };
                await uow.MovimientosCaja.AgregarAsync(movimientoVuelto);
            }
        }
    }
}
