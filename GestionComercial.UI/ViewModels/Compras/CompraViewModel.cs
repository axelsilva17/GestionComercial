using Caliburn.Micro;
using GestionComercial.UI.Views.Comandos;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Compras
{
    public class CompraViewModel : NavigableViewModel
    {
        // ── Proveedores ───────────────────────────────────────────────
        private ObservableCollection<ProveedorItemDto> _proveedores = new();
        public ObservableCollection<ProveedorItemDto> Proveedores
        {
            get => _proveedores;
            set { _proveedores = value; NotifyOfPropertyChange(() => Proveedores); }
        }

        private ProveedorItemDto _proveedorSeleccionado;
        public ProveedorItemDto ProveedorSeleccionado
        {
            get => _proveedorSeleccionado;
            set
            {
                _proveedorSeleccionado = value;
                NotifyOfPropertyChange(() => ProveedorSeleccionado);
                NotifyOfPropertyChange(() => ProveedorNombre);
            }
        }

        public string ProveedorNombre => ProveedorSeleccionado?.Nombre ?? "Sin proveedor";

        // ── Búsqueda de producto ──────────────────────────────────────
        private string _busquedaProducto = string.Empty;
        public string BusquedaProducto
        {
            get => _busquedaProducto;
            set { _busquedaProducto = value; NotifyOfPropertyChange(() => BusquedaProducto); }
        }

        // ── Carrito — usa CompraItemDto (temporal, en memoria) ────────
        // NO usar CompraDetalleDto acá — ese es para mostrar compras ya guardadas
        private ObservableCollection<CompraItemDto> _items = new();
        public ObservableCollection<CompraItemDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        // ── Totales ───────────────────────────────────────────────────
        private decimal _total;
        public decimal Total
        {
            get => _total;
            set { _total = value; NotifyOfPropertyChange(() => Total); }
        }

        private int _cantidadItems;
        public int CantidadItems
        {
            get => _cantidadItems;
            set { _cantidadItems = value; NotifyOfPropertyChange(() => CantidadItems); }
        }

        // ── Otros campos ──────────────────────────────────────────────
        private string _observacion = string.Empty;
        public string Observacion
        {
            get => _observacion;
            set { _observacion = value; NotifyOfPropertyChange(() => Observacion); }
        }

        private DateTime _fecha = DateTime.Now;
        public DateTime Fecha
        {
            get => _fecha;
            set { _fecha = value; NotifyOfPropertyChange(() => Fecha); }
        }

        // ── Commands para botones dentro del DataGrid ─────────────────
        // Caliburn no puede bindear métodos del VM dentro de un DataTemplate,
        // por eso usamos RelayCommand con el ítem como parámetro via Tag+CommandParameter
        public RelayCommand<CompraItemDto> SumarCantidadCommand  { get; }
        public RelayCommand<CompraItemDto> RestarCantidadCommand { get; }
        public RelayCommand<CompraItemDto> QuitarItemCommand     { get; }

        public CompraViewModel()
        {
            Titulo    = "Nueva Compra";
            Subtitulo = "Registrar ingreso de mercadería";

            SumarCantidadCommand  = new RelayCommand<CompraItemDto>(SumarCantidad);
            RestarCantidadCommand = new RelayCommand<CompraItemDto>(RestarCantidad);
            QuitarItemCommand     = new RelayCommand<CompraItemDto>(QuitarItem);
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            IsLoading = true;
            try
            {
                // TODO: Proveedores = await _proveedorServicio.ObtenerItemsAsync(empresaId);
                await Task.Delay(100);
                Proveedores = new ObservableCollection<ProveedorItemDto>();
            }
            finally { IsLoading = false; }
        }

        // ── Agregar producto al carrito ───────────────────────────────
        public async Task AgregarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;

            // Si ya está, suma cantidad en vez de duplicar
            var existente = Items.FirstOrDefault(i =>
                i.CodigoBarra.Equals(BusquedaProducto, StringComparison.OrdinalIgnoreCase) ||
                i.ProductoNombre.Contains(BusquedaProducto, StringComparison.OrdinalIgnoreCase));

            if (existente != null)
            {
                SumarCantidad(existente);
            }
            else
            {
                // TODO: reemplazar con resultado de _productoServicio.BuscarAsync(BusquedaProducto)
                Items.Add(new CompraItemDto
                {
                    ProductoId     = 0,
                    ProductoNombre = BusquedaProducto,
                    CodigoBarra    = BusquedaProducto,
                    Cantidad       = 1,
                    PrecioCosto    = 0,
                    SubTotal       = 0
                });
            }

            BusquedaProducto = string.Empty;
            RecalcularTotal();
        }

        // ── Confirmar y guardar ───────────────────────────────────────
        public async Task ConfirmarCompra()
        {
            LimpiarError();

            if (ProveedorSeleccionado == null)
            {
                MostrarError("Seleccioná un proveedor antes de confirmar.");
                return;
            }
            if (!Items.Any())
            {
                MostrarError("Agregá al menos un producto.");
                return;
            }
            if (Items.Any(i => i.PrecioCosto <= 0))
            {
                MostrarError("Todos los productos deben tener precio de costo mayor a cero.");
                return;
            }

            IsLoading = true;
            try
            {
                // Mapear CompraItemDto → CompraDetalleCrearDto para la capa de Aplicación
                var dto = new CompraCrearDto
                {
                    IdProveedor = ProveedorSeleccionado.IdProveedor,
                    IdSucursal  = 1, // TODO: obtener de sesión
                    IdUsuario   = 1, // TODO: obtener de sesión
                    Fecha       = Fecha,
                    Observacion = Observacion,
                    Items       = Items.Select(i => new CompraDetalleCrearDto
                    {
                        IdProducto  = i.ProductoId,
                        Cantidad    = i.Cantidad,
                        PrecioCosto = i.PrecioCosto
                    }).ToList()
                };

                // TODO: await _compraServicio.CrearAsync(dto);
                await Task.Delay(400);

                await Volver();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al confirmar: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        public async Task Volver()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<CompraListadoViewModel>(), CancellationToken.None);
        }

        // ── Lógica carrito ────────────────────────────────────────────
        private void SumarCantidad(CompraItemDto item)
        {
            if (item == null) return;
            item.Cantidad++;
            item.SubTotal = item.Cantidad * item.PrecioCosto;
            RecalcularTotal();
            // INotifyPropertyChanged en CompraItemDto actualiza el DataGrid automáticamente
        }

        private void RestarCantidad(CompraItemDto item)
        {
            if (item == null) return;
            if (item.Cantidad <= 1) { QuitarItem(item); return; }
            item.Cantidad--;
            item.SubTotal = item.Cantidad * item.PrecioCosto;
            RecalcularTotal();
        }

        private void QuitarItem(CompraItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotal();
        }

        private void RecalcularTotal()
        {
            Total         = Items.Sum(i => i.SubTotal);
            CantidadItems = Items.Sum(i => i.Cantidad);
        }
    }
}
