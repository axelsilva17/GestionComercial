using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
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
            set { _productoSeleccionado = value; NotifyOfPropertyChange(() => ProductoSeleccionado); }
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
                
                Proveedores = new ObservableCollection<ProveedorItemDto>(listaProveedores);
            }
            finally { IsLoading = false; }
        }

        // ── Buscar productos ───────────────────────────────────────────
        public async Task BuscarProducto()
        {
            if (string.IsNullOrWhiteSpace(BusquedaProducto)) return;
            
            var productos = await _productoServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
            var filtered = productos.Where(p => 
                p.Nombre.Contains(BusquedaProducto, StringComparison.OrdinalIgnoreCase) ||
                (p.CodigoBarra?.Contains(BusquedaProducto) ?? false))
                .Take(10)
                .Select(p => new ProductoItemDto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    CodigoBarra = p.CodigoBarra ?? string.Empty,
                    PrecioCostoActual = p.PrecioCostoActual,
                    StockActual = p.StockActual
                }).ToList();

            ProductosEncontrados = new ObservableCollection<ProductoItemDto>(filtered);
        }

        // ── Agregar producto al carrito ───────────────────────────────
        public void AgregarProducto()
        {
            if (ProductoSeleccionado == null) return;

            var itemExistente = Items.FirstOrDefault(i => i.ProductoId == ProductoSeleccionado.IdProducto);
            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
                itemExistente.SubTotal = itemExistente.Cantidad * itemExistente.PrecioCosto;
            }
            else
            {
                Items.Add(new CompraItemDto
                {
                    ProductoId = ProductoSeleccionado.IdProducto,
                    ProductoNombre = ProductoSeleccionado.Nombre,
                    Cantidad = 1,
                    PrecioCosto = ProductoSeleccionado.PrecioCostoActual,
                    SubTotal = ProductoSeleccionado.PrecioCostoActual
                });
            }

            RecalcularTotales();
            BusquedaProducto = string.Empty;
            ProductosEncontrados.Clear();
            NotifyOfPropertyChange(() => Items);
        }

        private void SumarCantidad(CompraItemDto item)
        {
            if (item == null) return;
            item.Cantidad++;
            item.SubTotal = item.Cantidad * item.PrecioCosto;
            RecalcularTotales();
            NotifyOfPropertyChange(() => Items);
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
        }

        private void QuitarItem(CompraItemDto item)
        {
            if (item == null) return;
            Items.Remove(item);
            RecalcularTotales();
        }

        private void RecalcularTotales()
        {
            Total = Items.Sum(i => i.SubTotal);
            CantidadItems = Items.Sum(i => i.Cantidad);
        }

        // ── Guardar compra ───────────────────────────────────────────
        public bool CanGuardar => ProveedorSeleccionado != null && Items.Count > 0 && !IsLoading;

        public async Task Guardar()
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
    }
}