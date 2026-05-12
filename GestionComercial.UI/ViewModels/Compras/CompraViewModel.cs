using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.Views.Comandos;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GestionComercial.UI.ViewModels.Compras
{
    public class CompraViewModel : NavigableViewModel
    {
        private readonly IProveedorServicio _proveedorServicio;
        private readonly ICompraServicio _compraServicio;
        private readonly IProductoServicio _productoServicio;
        private readonly SesionServicio _sesion;
        private readonly ShellViewModel _shell;

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
                NotifyOfPropertyChange(() => CanGuardar);
                NotifyOfPropertyChange(() => FiltroProveedorLabel);
                // Cargar historial de compras del proveedor
                _ = CargarHistorialProveedorAsync();
            }
        }

        public string ProveedorNombre => ProveedorSeleccionado?.Nombre ?? "Sin proveedor";
        public string FiltroProveedorLabel => ProveedorSeleccionado != null 
            ? $"Productos de {ProveedorSeleccionado.Nombre}" 
            : "Seleccioná un proveedor para ver sus productos";

        // ── Historial de compras del proveedor ───────────────────────────
        private ObservableCollection<CompraDto> _historialProveedor = new();
        public ObservableCollection<CompraDto> HistorialProveedor
        {
            get => _historialProveedor;
            set { _historialProveedor = value; NotifyOfPropertyChange(() => HistorialProveedor); }
        }

        private async Task CargarHistorialProveedorAsync()
        {
            if (ProveedorSeleccionado == null)
            {
                HistorialProveedor.Clear();
                return;
            }
            try
            {
                var compras = await _compraServicio.ObtenerPorProveedorAsync(ProveedorSeleccionado.IdProveedor);
                HistorialProveedor = new ObservableCollection<CompraDto>(compras.Take(5));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargando historial: {ex.Message}");
            }
        }

        // ── Búsqueda de producto con debounce ───────────────────────────
        private string _busquedaProducto = string.Empty;
        public string BusquedaProducto
        {
            get => _busquedaProducto;
            set 
            { 
                _busquedaProducto = value; 
                NotifyOfPropertyChange(() => BusquedaProducto);
                
                // Debounce: buscar después de 300ms
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }

        // Timer para debounce
        private readonly DispatcherTimer _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };

        // Flags para UI
        private bool _mostrarPopupBusqueda;
        public bool MostrarPopupBusqueda
        {
            get => _mostrarPopupBusqueda;
            set { _mostrarPopupBusqueda = value; NotifyOfPropertyChange(() => MostrarPopupBusqueda); }
        }

        // ── Productos para agregar ───────────────────────────────────
        private ObservableCollection<ProductoItemDto> _productosEncontrados = new();
        public ObservableCollection<ProductoItemDto> ProductosEncontrados
        {
            get => _productosEncontrados;
            set { _productosEncontrados = value; NotifyOfPropertyChange(() => ProductosEncontrados); }
        }

        private ProductoItemDto _productoSeleccionado;
        public ProductoItemDto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set 
            { 
                _productoSeleccionado = value; 
                NotifyOfPropertyChange(() => ProductoSeleccionado);
                
                // Resetear campos de agregar cuando se selecciona un producto
                if (value != null)
                {
                    PrecioAgregar = value.PrecioCostoActual;
                    CantidadAgregar = 1;
                }
            }
        }

        // ── Campos para agregar ─────────────────────────────────────────
        private int _cantidadAgregar = 1;
        public int CantidadAgregar
        {
            get => _cantidadAgregar;
            set { _cantidadAgregar = value; NotifyOfPropertyChange(() => CantidadAgregar); }
        }

        private decimal _precioAgregar;
        public decimal PrecioAgregar
        {
            get => _precioAgregar;
            set { _precioAgregar = value; NotifyOfPropertyChange(() => PrecioAgregar); }
        }

        // ── Carrito — usa CompraItemDto (temporal, en memoria) ────────
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

        // ── Commands ──────────────────────────────────────────────────
        public RelayCommand<CompraItemDto> SumarCantidadCommand  { get; }
        public RelayCommand<CompraItemDto> RestarCantidadCommand { get; }
        public RelayCommand<CompraItemDto> QuitarItemCommand     { get; }

        public CompraViewModel(
            IProveedorServicio proveedorServicio,
            ICompraServicio compraServicio,
            IProductoServicio productoServicio,
            SesionServicio sesion,
            ShellViewModel shell)
        {
            _proveedorServicio = proveedorServicio;
            _compraServicio = compraServicio;
            _productoServicio = productoServicio;
            _sesion = sesion;
            _shell = shell;

            Titulo    = "Nueva Compra";
            Subtitulo = "Registrar ingreso de mercadería";

            // Configurar debounce timer
            _debounceTimer.Tick += async (s, e) =>
            {
                _debounceTimer.Stop();
                await BuscarProductoAsync();
            };

            SumarCantidadCommand  = new RelayCommand<CompraItemDto>(SumarCantidad);
            RestarCantidadCommand = new RelayCommand<CompraItemDto>(RestarCantidad);
            QuitarItemCommand     = new RelayCommand<CompraItemDto>(QuitarItem);
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            IsLoading = true;
            try
            {
                var todosProveedores = await _proveedorServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                System.Diagnostics.Debug.WriteLine($"Proveedores cargados: {todosProveedores.Count()}");
                
                var listaProveedores = todosProveedores
                    .Where(p => p.Activo)
                    .Select(p => new ProveedorItemDto
                    {
                        IdProveedor = p.Id,
                        Nombre = p.Nombre,
                        Telefono = p.Telefono ?? string.Empty,
                        Email = p.Email ?? string.Empty,
                        Activo = p.Activo
                    }).ToList();
                
                System.Diagnostics.Debug.WriteLine($"Proveedores activos: {listaProveedores.Count}");
                Proveedores = new ObservableCollection<ProveedorItemDto>(listaProveedores);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargando proveedores: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        // ── Buscar productos con debounce ────────────────────────────────
        public async Task BuscarProductoAsync()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto) || BusquedaProducto.Length < 2)
            {
                MostrarPopupBusqueda = false;
                return;
            }
            
            try
            {
                var productos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                var termino = BusquedaProducto.Trim().ToLower();
                
                var filtered = productos
                    .Where(p => 
                        (p.Nombre?.ToLower().Contains(termino) ?? false) ||
                        (p.CodigoBarra?.ToLower().Contains(termino) ?? false))
                    .Take(8)
                    .Select(p => new ProductoItemDto
                    {
                        IdProducto = p.IdProducto,
                        Nombre = p.Nombre,
                        CodigoBarra = p.CodigoBarra ?? string.Empty,
                        PrecioCostoActual = p.PrecioCostoActual,
                        StockActual = p.StockActual
                    }).ToList();

                ProductosEncontrados = new ObservableCollection<ProductoItemDto>(filtered);
                MostrarPopupBusqueda = filtered.Count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error buscando productos: {ex.Message}");
                MostrarPopupBusqueda = false;
            }
        }

        // ── Agregar producto al carrito ───────────────────────────────
        public void AgregarProducto()
        {
            if (ProductoSeleccionado == null || CantidadAgregar <= 0) return;

            var itemExistente = Items.FirstOrDefault(i => i.ProductoId == ProductoSeleccionado.IdProducto);
            if (itemExistente != null)
            {
                // Si ya existe, sumar la cantidad nueva y actualizar precio promedio
                int nuevaCantidad = itemExistente.Cantidad + CantidadAgregar;
                decimal nuevoPrecio = ((itemExistente.PrecioCosto * itemExistente.Cantidad) + (PrecioAgregar * CantidadAgregar)) / nuevaCantidad;
                
                itemExistente.Cantidad = nuevaCantidad;
                itemExistente.PrecioCosto = nuevoPrecio;
                itemExistente.SubTotal = nuevaCantidad * nuevoPrecio;
            }
            else
            {
                Items.Add(new CompraItemDto
                {
                    ProductoId = ProductoSeleccionado.IdProducto,
                    ProductoNombre = ProductoSeleccionado.Nombre,
                    Cantidad = CantidadAgregar,
                    PrecioCosto = PrecioAgregar,
                    SubTotal = CantidadAgregar * PrecioAgregar
                });
            }

            RecalcularTotales();
            BusquedaProducto = string.Empty;
            ProductosEncontrados.Clear();
            ProductoSeleccionado = null;
            CantidadAgregar = 1;
            NotifyOfPropertyChange(() => Items);
            NotifyOfPropertyChange(() => CanGuardar);
        }

        private void SumarCantidad(CompraItemDto item)
        {
            if (item == null) return;
            item.Cantidad++;
            item.SubTotal = item.Cantidad * item.PrecioCosto;
            RecalcularTotales();
            NotifyOfPropertyChange(() => Items);
            NotifyOfPropertyChange(() => CanGuardar);
        }

        private void RestarCantidad(CompraItemDto item)
        {
            if (item == null) return;
            if (item.Cantidad > 1)
            {
                item.Cantidad--;
                item.SubTotal = item.Cantidad * item.PrecioCosto;
            }
            RecalcularTotales();
            NotifyOfPropertyChange(() => Items);
            NotifyOfPropertyChange(() => CanGuardar);
        }

        private void QuitarItem(CompraItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotales();
            NotifyOfPropertyChange(() => CanGuardar);
        }

        private void RecalcularTotales()
        {
            Total = Items.Sum(i => i.SubTotal);
            CantidadItems = Items.Sum(i => i.Cantidad);
        }

        // ── Confirmar compra ────────────────────────────────────────────
        public bool CanGuardar => ProveedorSeleccionado != null && Items.Count > 0 && !IsLoading;

        public async Task ConfirmarCompra()
        {
            if (!CanGuardar) return;

            IsLoading = true;
            try
            {
                var dto = new CompraCrearDto
                {
                    IdProveedor = ProveedorSeleccionado.IdProveedor,
                    IdSucursal = _sesion.IdSucursal,
                    IdUsuario = _sesion.IdUsuario,
                    Observacion = Observacion,
                    Items = Items.Select(i => new CompraDetalleCrearDto
                    {
                        IdProducto = i.ProductoId,
                        Cantidad = i.Cantidad,
                        PrecioCosto = i.PrecioCosto
                    }).ToList()
                };

                await _compraServicio.CrearAsync(dto);
                
                var listado = IoC.Get<CompraListadoViewModel>();
                await _shell.ActivateItemAsync(listado, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MostrarError($"Error al guardar: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task Cancelar()
        {
            var listado = IoC.Get<CompraListadoViewModel>();
            await _shell.ActivateItemAsync(listado, CancellationToken.None);
        }

        // Volver al listado de compras
        public async Task Volver()
        {
            var listado = IoC.Get<CompraListadoViewModel>();
            await _shell.ActivateItemAsync(listado, CancellationToken.None);
        }
    }
}