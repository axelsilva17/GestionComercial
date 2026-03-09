using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.Views.Comandos;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Ventas
{
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
        private readonly IVentaServicio _ventaServicio;
        private readonly SesionServicio _sesion;
        private VentaViewModel          _ventaOrigen;

        public PagoViewModel(IVentaServicio ventaServicio, SesionServicio sesion)
        {
            _ventaServicio           = ventaServicio;
            _sesion                  = sesion;
            SeleccionarMetodoCommand = new RelayCommand<MetodoPagoItemDto>(SeleccionarMetodo);
            QuitarPagoCommand        = new RelayCommand<PagoIngresadoDto>(QuitarPago);
        }

        private string _clienteNombre;
        public string ClienteNombre
        {
            get => _clienteNombre;
            set { _clienteNombre = value; NotifyOfPropertyChange(() => ClienteNombre); NotifyOfPropertyChange(() => ClienteInicial); }
        }
        public string ClienteInicial => string.IsNullOrEmpty(ClienteNombre) ? "?" : ClienteNombre[0].ToString().ToUpper();

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

        public RelayCommand<MetodoPagoItemDto> SeleccionarMetodoCommand { get; }
        public RelayCommand<PagoIngresadoDto>  QuitarPagoCommand        { get; }

        public void InicializarConVenta(VentaViewModel venta)
        {
            _ventaOrigen   = venta;
            ClienteNombre  = venta.ClienteNombre;
            TotalBruto     = venta.TotalBruto;
            TotalDescuento = venta.TotalDescuento;
            TotalFinal     = venta.TotalFinal;
            ItemsVenta     = new ObservableCollection<VentaItemDto>(venta.Items);
            PagosIngresados.Clear();

            MetodosPago = new ObservableCollection<MetodoPagoItemDto>
            {
                new() { Id = 1, Nombre = "Efectivo",      Icono = "💵", EsEfectivo = true  },
                new() { Id = 2, Nombre = "Débito",        Icono = "💳", EsEfectivo = false },
                new() { Id = 3, Nombre = "Crédito",       Icono = "💳", EsEfectivo = false },
                new() { Id = 4, Nombre = "Transferencia", Icono = "🏦", EsEfectivo = false },
                new() { Id = 5, Nombre = "Mercado Pago",  Icono = "📱", EsEfectivo = false },
            };
        }

        public void AgregarPago()
        {
            if (MetodoSeleccionado == null) return;
            if (!decimal.TryParse(MontoIngresado?.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var monto) || monto <= 0) return;

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
            if (!PagoCompleto || _ventaOrigen == null) return;
            IsLoading = true;
            try
            {
                var dto = new VentaCrearDto
                {
                    IdSucursal     = _sesion.IdSucursal,
                    IdCliente      = _ventaOrigen.ClienteId > 0 ? _ventaOrigen.ClienteId : 1,
                    IdUsuario      = _sesion.IdUsuario,
                    IdCaja         = _sesion.IdCajaActual ?? 0,
                    TotalDescuento = _ventaOrigen.TotalDescuento,
                    Items = _ventaOrigen.Items.Select(i => new VentaDetalleCrearDto  // nombre correcto
                    {
                        IdProducto     = i.ProductoId,
                        Cantidad       = (int)i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario,
                        CostoUnitario  = i.CostoUnitario,
                    }).ToList(),
                    Pagos = PagosIngresados.Select(p => new PagoCrearDto  // nombre correcto
                    {
                        IdMetodoPago = p.MetodoId,
                        Monto        = p.Monto,
                    }).ToList(),
                };

                await _ventaServicio.CrearAsync(dto);
                await IoC.Get<ShellViewModel>()
                         .ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally { IsLoading = false; }
        }

        public async Task Volver()
        {
            if (_ventaOrigen != null)
                await IoC.Get<ShellViewModel>().ActivateItemAsync(_ventaOrigen, CancellationToken.None);
            else
                await IoC.Get<ShellViewModel>().ActivateItemAsync(IoC.Get<VentaViewModel>(), CancellationToken.None);
        }

        private void SeleccionarMetodo(MetodoPagoItemDto metodo)
        {
            if (metodo == null) return;
            MetodoSeleccionado = metodo;
            var restante = TotalFinal - TotalPagado;
            if (restante > 0) MontoIngresado = restante.ToString("F2");
        }

        private void QuitarPago(PagoIngresadoDto pago)
        {
            if (pago == null) return;
            PagosIngresados.Remove(pago);
            RecalcularTotalPagado();
        }

        private void RecalcularTotalPagado()
            => TotalPagado = PagosIngresados.Sum(p => p.Monto);

        private void RecalcularVuelto()
        {
            var diff     = TotalPagado - TotalFinal;
            PagoCompleto = diff >= 0;
            VueltoOSaldo = Math.Abs(diff);
        }
    }
}
