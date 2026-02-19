using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.UI.Views.Comandos;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Ventas
{
    // DTO local — solo para la pantalla de venta activa (carrito)
    public class VentaItemDto
    {
        public int     ProductoId     { get; set; }
        public string  ProductoNombre { get; set; }
        public string  CodigoBarra    { get; set; }
        public int     Cantidad       { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal       { get; set; }
    }

    public class VentaViewModel : NavigableViewModel
    {
        // ── Cliente ───────────────────────────────────────────────────────────
        private string _clienteNombre;
        public string ClienteNombre
        {
            get => _clienteNombre;
            set { _clienteNombre = value; NotifyOfPropertyChange(() => ClienteNombre); }
        }

        private int _clienteId;
        public int ClienteId
        {
            get => _clienteId;
            set { _clienteId = value; NotifyOfPropertyChange(() => ClienteId); }
        }

        // ── Búsqueda ──────────────────────────────────────────────────────────
        private string _busquedaProducto;
        public string BusquedaProducto
        {
            get => _busquedaProducto;
            set { _busquedaProducto = value; NotifyOfPropertyChange(() => BusquedaProducto); }
        }

        // ── Items ─────────────────────────────────────────────────────────────
        private ObservableCollection<VentaItemDto> _items = new();
        public ObservableCollection<VentaItemDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        // ── Totales ───────────────────────────────────────────────────────────
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
            set { _totalFinal = value; NotifyOfPropertyChange(() => TotalFinal); }
        }

        private string _descuentoManual;
        public string DescuentoManual
        {
            get => _descuentoManual;
            set { _descuentoManual = value; NotifyOfPropertyChange(() => DescuentoManual); RecalcularTotales(); }
        }

        // ── Comandos ──────────────────────────────────────────────────────────
        public RelayCommand<VentaItemDto> SumarCantidadCommand  { get; }
        public RelayCommand<VentaItemDto> RestarCantidadCommand { get; }
        public RelayCommand<VentaItemDto> QuitarItemCommand     { get; }

        public VentaViewModel()
        {
            SumarCantidadCommand  = new RelayCommand<VentaItemDto>(SumarCantidad);
            RestarCantidadCommand = new RelayCommand<VentaItemDto>(RestarCantidad);
            QuitarItemCommand     = new RelayCommand<VentaItemDto>(QuitarItem);
        }

        // ── Acciones Caliburn ─────────────────────────────────────────────────
        public async Task SeleccionarCliente()
        {
            var vm = IoC.Get<SeleccionClienteViewModel>();
            vm.VentaOrigen = this;
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task AgregarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;

            // TODO: buscar en IProductoServicio
            var existente = BuscarItem(i => i.ProductoNombre == BusquedaProducto);
            if (existente != null)
            {
                existente.Cantidad++;
                existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;
                RefrescarItem(existente);
            }
            else
            {
                Items.Add(new VentaItemDto
                {
                    ProductoId     = 1,
                    ProductoNombre = BusquedaProducto,
                    CodigoBarra    = "0000000",
                    Cantidad       = 1,
                    PrecioUnitario = 0,
                    Subtotal       = 0
                });
            }

            BusquedaProducto = string.Empty;
            RecalcularTotales();
            await Task.CompletedTask;
        }

        public async Task IrACobrar()
        {
            if (Items.Count == 0) return;
            var vm = IoC.Get<PagoViewModel>();
            vm.InicializarConVenta(this);
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task Volver()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);
        }

        // ── Lógica interna ────────────────────────────────────────────────────
        private void SumarCantidad(VentaItemDto item)
        {
            if (item == null) return;
            item.Cantidad++;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            RefrescarItem(item);
            RecalcularTotales();
        }

        private void RestarCantidad(VentaItemDto item)
        {
            if (item == null) return;
            if (item.Cantidad <= 1) { QuitarItem(item); return; }
            item.Cantidad--;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            RefrescarItem(item);
            RecalcularTotales();
        }

        private void QuitarItem(VentaItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotales();
        }

        private void RefrescarItem(VentaItemDto item)
        {
            var idx = Items.IndexOf(item);
            if (idx < 0) return;
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
        }

        private void RecalcularTotales()
        {
            TotalBruto = 0;
            foreach (var i in Items) TotalBruto += i.Subtotal;

            decimal pct = 0;
            if (decimal.TryParse(DescuentoManual, out var d))
                pct = Math.Clamp(d, 0, 100);

            TotalDescuento = Math.Round(TotalBruto * pct / 100, 2);
            TotalFinal     = TotalBruto - TotalDescuento;
        }

        private VentaItemDto BuscarItem(Func<VentaItemDto, bool> pred)
        {
            foreach (var i in Items) if (pred(i)) return i;
            return null;
        }
    }
}
