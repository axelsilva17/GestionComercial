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
using System.Windows.Input;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class VentaViewModel : NavigableViewModel
    {
        private readonly IProductoServicio _productoServicio;
        private readonly IVentaServicio    _ventaServicio;
        private readonly SesionServicio    _sesion;

        public VentaViewModel(
            IProductoServicio productoServicio,
            IVentaServicio    ventaServicio,
            SesionServicio    sesion)
        {
            _productoServicio     = productoServicio;
            _ventaServicio        = ventaServicio;
            _sesion               = sesion;
            Titulo                = "Nueva Venta";
            SumarCantidadCommand  = new RelayCommand<VentaItemDto>(SumarCantidad);
            RestarCantidadCommand = new RelayCommand<VentaItemDto>(RestarCantidad);
            QuitarItemCommand     = new RelayCommand<VentaItemDto>(QuitarItem);
        }

        // ── Cliente ───────────────────────────────────────────────────────────
        private string _clienteNombre = string.Empty;
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

        // ── Búsqueda (texto libre O código de barras del escáner) ─────────────
        private string _busquedaProducto = string.Empty;
        public string BusquedaProducto
        {
            get => _busquedaProducto;
            set { _busquedaProducto = value; NotifyOfPropertyChange(() => BusquedaProducto); }
        }

        // ── Items del carrito ─────────────────────────────────────────────────
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

        private string _descuentoManual = string.Empty;
        public string DescuentoManual
        {
            get => _descuentoManual;
            set { _descuentoManual = value; NotifyOfPropertyChange(() => DescuentoManual); RecalcularTotales(); }
        }

        // ── Commands para botones dentro del DataGrid ─────────────────────────
        public RelayCommand<VentaItemDto> SumarCantidadCommand  { get; }
        public RelayCommand<VentaItemDto> RestarCantidadCommand { get; }
        public RelayCommand<VentaItemDto> QuitarItemCommand     { get; }

        // ── Acciones públicas (bindeadas desde la View) ───────────────────────

        public async Task SeleccionarCliente()
        {
            var vm = IoC.Get<SeleccionClienteViewModel>();
            vm.VentaOrigen = this;
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        /// <summary>
        /// Busca por nombre O por código de barras exacto.
        /// El escáner físico envía el código como texto + Enter,
        /// así que este método se llama igual desde el botón o desde KeyDown Enter.
        /// </summary>
        public async Task AgregarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;
            IsLoading = true;
            LimpiarError();
            try
            {
                var todos    = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                var busqueda = BusquedaProducto.Trim().ToLower();

                // Prioridad 1: código de barras exacto (escáner)
                var producto = todos.FirstOrDefault(p =>
                        p.CodigoBarra != null &&
                        p.CodigoBarra.Trim().ToLower() == busqueda)
                    // Prioridad 2: nombre contiene el texto
                    ?? todos.FirstOrDefault(p =>
                        p.Nombre.ToLower().Contains(busqueda));

                if (producto == null)
                {
                    MostrarError($"No se encontró ningún producto con '{BusquedaProducto}'.");
                    return;
                }

                if (producto.StockActual <= 0)
                {
                    MostrarError($"'{producto.Nombre}' no tiene stock disponible.");
                    return;
                }

                var existente = Items.FirstOrDefault(i => i.ProductoId == producto.IdProducto);
                if (existente != null)
                {
                    if (existente.Cantidad >= producto.StockActual)
                    {
                        MostrarError($"Stock máximo: {producto.StockActual}");
                        return;
                    }
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
                RecalcularTotales();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        /// <summary>
        /// Crea la venta como Pendiente en BD y navega a PagoViewModel.
        /// El stock ya se descuenta al crear — si el pago falla/cancela,
        /// el operador puede anular la venta y el stock se repone.
        /// </summary>
        public async Task IrACobrar()
        {
            if (Items.Count == 0) { MostrarError("Agregá al menos un producto."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                var dto = new VentaCrearDto
                {
                    IdSucursal     = _sesion.IdSucursal,
                    IdCliente      = ClienteId > 0 ? ClienteId : 1, // 1 = consumidor final
                    IdUsuario      = _sesion.IdUsuario,
                    IdCaja         = _sesion.IdCajaActual ?? 0,
                    TotalDescuento = TotalDescuento,
                    Items          = Items.Select(i => new VentaDetalleCrearDto
                    {
                        IdProducto     = i.ProductoId,
                        Cantidad       = (int)i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario,
                        CostoUnitario  = i.CostoUnitario,
                    }).ToList(),
                };

                var venta = await _ventaServicio.CrearAsync(dto);

                var vm = IoC.Get<PagoViewModel>();
                vm.InicializarConVenta(
                    venta.IdVenta,
                    ClienteNombre.Length > 0 ? ClienteNombre : "Consumidor Final",
                    venta.TotalFinal);

                await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        /// <summary>Resetea el formulario para una nueva venta.</summary>
        public void NuevaVenta()
        {
            Items            = new();
            ClienteId        = 0;
            ClienteNombre    = string.Empty;
            DescuentoManual  = string.Empty;
            BusquedaProducto = string.Empty;
            RecalcularTotales();
            LimpiarError();
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
