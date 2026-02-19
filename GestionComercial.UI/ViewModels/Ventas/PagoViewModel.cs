using Caliburn.Micro;
using GestionComercial.UI.Views.Comandos;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Ventas
{
    // DTOs locales de pago — no están en capa Aplicación porque son específicos de esta pantalla
    public class MetodoPagoItemDto
    {
        public int    Id             { get; set; }
        public string Nombre         { get; set; }
        public string Icono          { get; set; }
        public bool   EsEfectivo     { get; set; }
        public bool   IsSeleccionado { get; set; }
    }

    public class PagoIngresadoDto
    {
        public int     MetodoId     { get; set; }
        public string  MetodoNombre { get; set; }
        public string  Icono        { get; set; }
        public decimal Monto        { get; set; }
    }

    public class PagoViewModel : NavigableViewModel
    {
        // ── Datos de la venta ─────────────────────────────────────────────────
        private string _clienteNombre;
        public string ClienteNombre
        {
            get => _clienteNombre;
            set { _clienteNombre = value; NotifyOfPropertyChange(() => ClienteNombre); }
        }

        public string ClienteInicial => string.IsNullOrEmpty(ClienteNombre)
            ? "?" : ClienteNombre[0].ToString().ToUpper();

        private decimal _totalBruto;
        public decimal TotalBruto
        {
            get => _totalBruto;
            set { _totalBruto = value; NotifyOfPropertyChange(() => TotalBruto); }
        }

        private decimal _totalDescuento;
        public decimal TotalDescuento
        {
            get => _totalDescuento;
            set { _totalDescuento = value; NotifyOfPropertyChange(() => TotalDescuento); }
        }

        private decimal _totalFinal;
        public decimal TotalFinal
        {
            get => _totalFinal;
            set { _totalFinal = value; NotifyOfPropertyChange(() => TotalFinal); RecalcularVuelto(); }
        }

        private ObservableCollection<VentaItemDto> _itemsVenta = new();
        public ObservableCollection<VentaItemDto> ItemsVenta
        {
            get => _itemsVenta;
            set { _itemsVenta = value; NotifyOfPropertyChange(() => ItemsVenta); }
        }

        // ── Métodos de pago ───────────────────────────────────────────────────
        private ObservableCollection<MetodoPagoItemDto> _metodosPago = new();
        public ObservableCollection<MetodoPagoItemDto> MetodosPago
        {
            get => _metodosPago;
            set { _metodosPago = value; NotifyOfPropertyChange(() => MetodosPago); }
        }

        private MetodoPagoItemDto _metodoSeleccionado;
        public MetodoPagoItemDto MetodoSeleccionado
        {
            get => _metodoSeleccionado;
            set { _metodoSeleccionado = value; NotifyOfPropertyChange(() => MetodoSeleccionado); }
        }

        // ── Pagos ingresados ──────────────────────────────────────────────────
        private ObservableCollection<PagoIngresadoDto> _pagosIngresados = new();
        public ObservableCollection<PagoIngresadoDto> PagosIngresados
        {
            get => _pagosIngresados;
            set { _pagosIngresados = value; NotifyOfPropertyChange(() => PagosIngresados); }
        }

        private string _montoIngresado;
        public string MontoIngresado
        {
            get => _montoIngresado;
            set { _montoIngresado = value; NotifyOfPropertyChange(() => MontoIngresado); }
        }

        // ── Totales cobro ─────────────────────────────────────────────────────
        private decimal _totalPagado;
        public decimal TotalPagado
        {
            get => _totalPagado;
            set { _totalPagado = value; NotifyOfPropertyChange(() => TotalPagado); RecalcularVuelto(); }
        }

        private decimal _vueltoOSaldo;
        public decimal VueltoOSaldo
        {
            get => _vueltoOSaldo;
            set { _vueltoOSaldo = value; NotifyOfPropertyChange(() => VueltoOSaldo); }
        }

        private bool _pagoCompleto;
        public bool PagoCompleto
        {
            get => _pagoCompleto;
            set { _pagoCompleto = value; NotifyOfPropertyChange(() => PagoCompleto); }
        }

        // ── Comandos ──────────────────────────────────────────────────────────
        public RelayCommand<MetodoPagoItemDto> SeleccionarMetodoCommand { get; }
        public RelayCommand<PagoIngresadoDto>  QuitarPagoCommand        { get; }

        public PagoViewModel()
        {
            SeleccionarMetodoCommand = new RelayCommand<MetodoPagoItemDto>(SeleccionarMetodo);
            QuitarPagoCommand        = new RelayCommand<PagoIngresadoDto>(QuitarPago);
        }

        // ── Inicialización ────────────────────────────────────────────────────
        public void InicializarConVenta(VentaViewModel venta)
        {
            ClienteNombre  = venta.ClienteNombre;
            TotalBruto     = venta.TotalBruto;
            TotalDescuento = venta.TotalDescuento;
            TotalFinal     = venta.TotalFinal;
            ItemsVenta     = new ObservableCollection<VentaItemDto>(venta.Items);

            MetodosPago = new ObservableCollection<MetodoPagoItemDto>
            {
                new() { Id = 1, Nombre = "Efectivo",         Icono = "💵", EsEfectivo = true  },
                new() { Id = 2, Nombre = "Débito",           Icono = "💳", EsEfectivo = false },
                new() { Id = 3, Nombre = "Crédito",          Icono = "💳", EsEfectivo = false },
                new() { Id = 4, Nombre = "Transferencia",    Icono = "🏦", EsEfectivo = false },
                new() { Id = 5, Nombre = "QR / MercadoPago", Icono = "📱", EsEfectivo = false },
            };
        }

        // ── Acciones Caliburn ─────────────────────────────────────────────────
        public void AgregarPago()
        {
            if (MetodoSeleccionado == null) return;
            if (!decimal.TryParse(MontoIngresado?.Replace(",", "."), out var monto) || monto <= 0) return;

            PagosIngresados.Add(new PagoIngresadoDto
            {
                MetodoId     = MetodoSeleccionado.Id,
                MetodoNombre = MetodoSeleccionado.Nombre,
                Icono        = MetodoSeleccionado.Icono,
                Monto        = monto
            });

            MontoIngresado = string.Empty;
            RecalcularTotalPagado();
        }

        public async Task ConfirmarCobro()
        {
            if (!PagoCompleto) return;
            IsLoading = true;
            try
            {
                // TODO: await _ventaServicio.ConfirmarAsync(...)
                await Task.Delay(300);
                await IoC.Get<ShellViewModel>()
                         .ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);
            }
            finally { IsLoading = false; }
        }

        public async Task Volver()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaViewModel>(), CancellationToken.None);
        }

        // ── Lógica interna ────────────────────────────────────────────────────
        private void SeleccionarMetodo(MetodoPagoItemDto metodo)
        {
            if (metodo == null) return;
            MetodoSeleccionado = metodo;
            if (TotalFinal - TotalPagado > 0)
                MontoIngresado = (TotalFinal - TotalPagado).ToString("F2");
        }

        private void QuitarPago(PagoIngresadoDto pago)
        {
            if (pago == null) return;
            PagosIngresados.Remove(pago);
            RecalcularTotalPagado();
        }

        private void RecalcularTotalPagado()
        {
            decimal t = 0;
            foreach (var p in PagosIngresados) t += p.Monto;
            TotalPagado = t;
        }

        private void RecalcularVuelto()
        {
            var diff     = TotalPagado - TotalFinal;
            PagoCompleto = diff >= 0;
            VueltoOSaldo = System.Math.Abs(diff);
        }
    }
}
