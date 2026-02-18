using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GestionComercial.UI.Views.Comandos;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class VentaViewModel : NavigableViewModel
    {
        // ─── Cliente ──────────────────────────────────────────────────────────────
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

        // ─── Búsqueda de productos ────────────────────────────────────────────────
        private string _busquedaProducto;
        public string BusquedaProducto
        {
            get => _busquedaProducto;
            set { _busquedaProducto = value; NotifyOfPropertyChange(() => BusquedaProducto); }
        }

        // ─── Items de la venta ────────────────────────────────────────────────────
        private ObservableCollection<VentaItemDto> _items = new();
        public ObservableCollection<VentaItemDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        // ─── Totales ──────────────────────────────────────────────────────────────
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
            set
            {
                _descuentoManual = value;
                NotifyOfPropertyChange(() => DescuentoManual);
                RecalcularTotales();
            }
        }

        // ─── Comandos ─────────────────────────────────────────────────────────────
        public RelayCommand<VentaItemDto> SumarCantidadCommand { get; }
        public RelayCommand<VentaItemDto> RestarCantidadCommand { get; }
        public RelayCommand<VentaItemDto> QuitarItemCommand { get; }

        public VentaViewModel()
        {
            SumarCantidadCommand  = new RelayCommand<VentaItemDto>(SumarCantidad);
            RestarCantidadCommand = new RelayCommand<VentaItemDto>(RestarCantidad);
            QuitarItemCommand     = new RelayCommand<VentaItemDto>(QuitarItem);
        }

        // ─── Métodos bindeados por Caliburn (x:Name) ─────────────────────────────

        public async Task SeleccionarCliente()
        {
            var vm = IoC.Get<SeleccionClienteViewModel>();
            vm.VentaOrigen = this;
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task AgregarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;

            // TODO: buscar en IProductoServicio por nombre o código de barra
            // var producto = await _productoServicio.BuscarAsync(BusquedaProducto);

            // Demo: agrega item de prueba
            var existente = Items.FirstOrDefault(i => i.ProductoNombre == BusquedaProducto);
            if (existente != null)
            {
                existente.Cantidad++;
                existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;
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
        }

        public async Task IrACobrar()
        {
            if (Items.Count == 0) return;

            var pagoVM = IoC.Get<PagoViewModel>();
            pagoVM.InicializarConVenta(this);
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(pagoVM, CancellationToken.None);
        }

        public async Task Volver()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);
        }

        // ─── Comandos de fila ─────────────────────────────────────────────────────
        private void SumarCantidad(VentaItemDto item)
        {
            if (item == null) return;
            item.Cantidad++;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            RecalcularTotales();
            // Forzar refresh del item en la colección
            var idx = Items.IndexOf(item);
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
        }

        private void RestarCantidad(VentaItemDto item)
        {
            if (item == null) return;
            if (item.Cantidad <= 1) { QuitarItem(item); return; }
            item.Cantidad--;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            RecalcularTotales();
            var idx = Items.IndexOf(item);
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
        }

        private void QuitarItem(VentaItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotales();
        }

        private void RecalcularTotales()
        {
            TotalBruto = 0;
            foreach (var item in Items)
                TotalBruto += item.Subtotal;

            decimal pctDesc = 0;
            if (decimal.TryParse(DescuentoManual, out var desc))
                pctDesc = Math.Clamp(desc, 0, 100);

            TotalDescuento = Math.Round(TotalBruto * pctDesc / 100, 2);
            TotalFinal     = TotalBruto - TotalDescuento;
        }

        // Helper para FirstOrDefault sin LINQ using (evita ambigüedad)
        private VentaItemDto FirstOrDefault(Func<VentaItemDto, bool> pred)
        {
            foreach (var i in Items) if (pred(i)) return i;
            return null;
        }
    }

    // ─── DTO local de item ────────────────────────────────────────────────────────
    public class VentaItemDto
    {
        public int     ProductoId     { get; set; }
        public string  ProductoNombre { get; set; }
        public string  CodigoBarra    { get; set; }
        public int     Cantidad       { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal       { get; set; }
    }
}
