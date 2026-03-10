using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.Views.Comandos;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GestionComercial.Aplicacion.DTOs.Ventas;

namespace GestionComercial.UI.ViewModels.Ventas
{

    public class VentaViewModel : NavigableViewModel
    {
        private readonly IProductoServicio _productoServicio;
        private readonly SesionServicio    _sesion;

        public VentaViewModel(IProductoServicio productoServicio, SesionServicio sesion)
        {
            _productoServicio     = productoServicio;
            _sesion               = sesion;
            Titulo                = "Nueva Venta";
            SumarCantidadCommand  = new RelayCommand<VentaItemDto>(SumarCantidad);
            RestarCantidadCommand = new RelayCommand<VentaItemDto>(RestarCantidad);
            QuitarItemCommand     = new RelayCommand<VentaItemDto>(QuitarItem);
        }

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

        public RelayCommand<VentaItemDto> SumarCantidadCommand  { get; }
        public RelayCommand<VentaItemDto> RestarCantidadCommand { get; }
        public RelayCommand<VentaItemDto> QuitarItemCommand     { get; }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task SeleccionarCliente()
        {
            var vm = IoC.Get<SeleccionClienteViewModel>();
            vm.VentaOrigen = this;
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task AgregarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;
            IsLoading = true;
            try
            {
                // ObtenerTodosAsync y filtrar por nombre o código de barras
                var todos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                var busqueda = BusquedaProducto.Trim().ToLower();
                var producto = todos.FirstOrDefault(p =>
                    (p.CodigoBarra != null && p.CodigoBarra.ToLower() == busqueda) ||
                    p.Nombre.ToLower().Contains(busqueda));

                if (producto == null)
                {
                    ErrorMessage = "Producto no encontrado.";
                    return;
                }

                var existente = Items.FirstOrDefault(i => i.ProductoId == producto.IdProducto);
                if (existente != null)
                {
                    var idx = Items.IndexOf(existente);
                    existente.Cantidad++;
                    existente.Subtotal = existente.Cantidad * existente.PrecioUnitario;
                    Items.RemoveAt(idx);
                    Items.Insert(idx, existente);
                }
                else
                {
                    Items.Add(new VentaItemDto
                    {
                        ProductoId     = producto.IdProducto,
                        ProductoNombre = producto.Nombre,
                        CodigoBarra    = producto.CodigoBarra ?? string.Empty,
                        Cantidad       = 1,
                        PrecioUnitario = producto.PrecioVentaActual,
                        CostoUnitario  = producto.PrecioCostoActual,
                        Subtotal       = producto.PrecioVentaActual,
                    });
                }

                BusquedaProducto = string.Empty;
                ErrorMessage     = string.Empty;
                RecalcularTotales();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally { IsLoading = false; }
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
            var idx = Items.IndexOf(item);
            item.Cantidad++;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
            RecalcularTotales();
        }

        private void RestarCantidad(VentaItemDto item)
        {
            if (item == null) return;
            if (item.Cantidad <= 1) { QuitarItem(item); return; }
            var idx = Items.IndexOf(item);
            item.Cantidad--;
            item.Subtotal = item.Cantidad * item.PrecioUnitario;
            Items.RemoveAt(idx);
            Items.Insert(idx, item);
            RecalcularTotales();
        }

        private void QuitarItem(VentaItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotales();
        }

        private void RecalcularTotales()
        {
            TotalBruto = Items.Sum(i => i.Subtotal);
            decimal pct = 0;
            if (decimal.TryParse(DescuentoManual, out var d))
                pct = Math.Clamp(d, 0, 100);
            TotalDescuento = Math.Round(TotalBruto * pct / 100, 2);
            TotalFinal     = TotalBruto - TotalDescuento;
        }
    }
}
