using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;
using System.Threading;

namespace GestionComercial.Dominio.Entidades.Pagos.Strategies
{
    public class EfectivoStrategy : IProcesadorPago
    {
        public bool AfectaCajaFisica => true;
        public string Categoria => "Efectivo";

        public async Task ProcesarPagoAsync(Pago pago, Ventas.Venta venta, IUnitOfWork uow, CancellationToken ct = default)
        {
            if (!venta.Id_caja.HasValue)
                return;

            // Registrar el monto BRUTO recibido (el cliente entregó 5000, no 3000).
            // El vuelto ya se registra como Egreso separado más abajo.
            var movimiento = new TipoMovimientoCaja
            {
                Tipo         = (int)TipoMovimientoCajaEnum.Ingreso,
                Monto        = pago.Monto,
                Concepto     = $"Venta #{venta.Id} (recibido: ${pago.Monto:N2}, vuelto: ${pago.Vuelto:N2})",
                ReferenciaId = venta.Id,
                Id_venta     = venta.Id,
                Id_caja      = venta.Id_caja.Value,
                Id_usuario   = venta.Id_usuario,
                Fecha        = DateTime.Now,
            };
            await uow.MovimientosCaja.AgregarAsync(movimiento, ct);

            // Actualizar saldo de caja en tiempo real
            // Neto: bruto recibido - vuelto devuelto
            var caja = await uow.Cajas.ObtenerPorIdAsync(venta.Id_caja.Value, ct);
            if (caja != null)
            {
                caja.MontoFinal = (caja.MontoFinal ?? caja.MontoInicial) + pago.Monto - pago.Vuelto;
                uow.Cajas.Actualizar(caja);
            }

            await uow.GuardarCambiosAsync(ct);

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
                await uow.MovimientosCaja.AgregarAsync(movimientoVuelto, ct);
            }
        }
    }
}