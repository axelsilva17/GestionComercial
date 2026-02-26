using Caliburn.Micro;
using System;
using System.Collections.Generic;

namespace GestionComercial.UI.Helpers
{
    /// <summary>
    /// Helper sobre el IEventAggregator de Caliburn para publicar/suscribir
    /// eventos tipados entre ViewModels sin acoplamiento directo.
    ///
    /// IMPORTANTE: Caliburn ya provee IEventAggregator. Esta clase es un wrapper
    /// que simplifica la publicación de eventos comunes del sistema.
    ///
    /// Uso en ViewModel:
    ///   _eventos.PublicarVentaRealizada(idVenta);
    ///
    /// Para suscribirse, implementar la interfaz correspondiente:
    ///   public class MiViewModel : Screen, IHandle<VentaRealizadaEvent>
    ///   {
    ///       public Task HandleAsync(VentaRealizadaEvent e, CancellationToken ct) { ... }
    ///   }
    ///   // Y en el constructor: _eventAggregator.SubscribeOnUIThread(this);
    /// </summary>
    public class EventosHelper
    {
        private readonly IEventAggregator _bus;

        public EventosHelper(IEventAggregator eventAggregator)
        {
            _bus = eventAggregator;
        }

        // ── Publicadores ──────────────────────────────────────────────────────

        public void PublicarVentaRealizada(int idVenta)
            => _bus.PublishOnUIThreadAsync(new VentaRealizadaEvent(idVenta));

        public void PublicarStockActualizado(int idProducto)
            => _bus.PublishOnUIThreadAsync(new StockActualizadoEvent(idProducto));

        public void PublicarCajaAbierta(int idCaja)
            => _bus.PublishOnUIThreadAsync(new CajaAbiertaEvent(idCaja));

        public void PublicarCajaCerrada(int idCaja)
            => _bus.PublishOnUIThreadAsync(new CajaCerradaEvent(idCaja));

        public void PublicarSesionCambiada()
            => _bus.PublishOnUIThreadAsync(new SesionCambiadaEvent());

        public void PublicarNavegar(string destino)
            => _bus.PublishOnUIThreadAsync(new NavegarEvent(destino));
    }

    // ══ EVENTOS ══════════════════════════════════════════════════════════════

    /// <summary>Se publica cuando se completa una venta.</summary>
    public record VentaRealizadaEvent(int IdVenta);

    /// <summary>Se publica cuando cambia el stock de un producto.</summary>
    public record StockActualizadoEvent(int IdProducto);

    /// <summary>Se publica cuando se abre la caja.</summary>
    public record CajaAbiertaEvent(int IdCaja);

    /// <summary>Se publica cuando se cierra la caja.</summary>
    public record CajaCerradaEvent(int IdCaja);

    /// <summary>Se publica cuando el usuario logueado cambia (login/logout).</summary>
    public record SesionCambiadaEvent();

    /// <summary>Se publica para navegar a una sección del shell.</summary>
    public record NavegarEvent(string Destino);
}
