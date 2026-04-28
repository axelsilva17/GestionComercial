using Caliburn.Micro;
using GestionComercial.Dominio.Entidades.Proveedores;
using ClosedXML.Excel;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Dominio.Interfaces.Servicios;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ProductoListadoViewModel : NavigableViewModel
    {
        private readonly IProductoServicio _productoServicio;
        private readonly ShellViewModel _shell;
        private readonly ILogger<ProductoListadoViewModel>? _logger;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private bool _isInitializing = true;

        public ProductoListadoViewModel(IProductoServicio productoServicio, ShellViewModel shell, ILogger<ProductoListadoViewModel>? logger = null)
        {
            _productoServicio = productoServicio;
            _shell = shell;
            _logger = logger;
            Titulo    = "Productos";
            Subtitulo = "Catálogo de productos";
        }


        // ── Métricas ──────────────────────────────────────────────────
        private int _totalProductos;
        public int TotalProductos
        {
            get => _totalProductos;
            set { _totalProductos = value; NotifyOfPropertyChange(() => TotalProductos); }
        }

        private int _productosActivos;
        public int ProductosActivos
        {
            get => _productosActivos;
            set { _productosActivos = value; NotifyOfPropertyChange(() => ProductosActivos); }
        }

        private int _productosStockBajo;
        public int ProductosStockBajo
        {
            get => _productosStockBajo;
            set { _productosStockBajo = value; NotifyOfPropertyChange(() => ProductosStockBajo); }
        }

        private int _productosSinStock;
        public int ProductosSinStock
        {
            get => _productosSinStock;
            set { _productosSinStock = value; NotifyOfPropertyChange(() => ProductosSinStock); }
        }

        // ── Listas ────────────────────────────────────────────────────
        private ObservableCollection<ProductoListadoDto> _productos = new();
        public ObservableCollection<ProductoListadoDto> Productos
        {
            get => _productos;
            set { _productos = value; NotifyOfPropertyChange(() => Productos); }
        }

        private ObservableCollection<CategoriaItemDto> _categorias = new();
        public ObservableCollection<CategoriaItemDto> Categorias
        {
            get => _categorias;
            set { _categorias = value; NotifyOfPropertyChange(() => Categorias); }
        }

        // ── Selección (sidebar) ───────────────────────────────────────
        private ProductoListadoDto _productoSeleccionado;
        public ProductoListadoDto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set { _productoSeleccionado = value; NotifyOfPropertyChange(() => ProductoSeleccionado); }
        }

        // ── Filtros ───────────────────────────────────────────────────
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private CategoriaItemDto _categoriaSeleccionada;
        public CategoriaItemDto CategoriaSeleccionada
        {
            get => _categoriaSeleccionada;
            set { _categoriaSeleccionada = value; NotifyOfPropertyChange(() => CategoriaSeleccionada); }
        }

        private int _filtroActivo;
        public int FiltroActivo
        {
            get => _filtroActivo;
            set { _filtroActivo = value; NotifyOfPropertyChange(() => FiltroActivo); }
        }

        // ── Paginación ────────────────────────────────────────────────
        private int _productosMostrados;
        public int ProductosMostrados
        {
            get => _productosMostrados;
            set { _productosMostrados = value; NotifyOfPropertyChange(() => ProductosMostrados); }
        }

        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; NotifyOfPropertyChange(() => PaginaActual); }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set { _totalPaginas = value; NotifyOfPropertyChange(() => TotalPaginas); }
        }

        // ── Ajuste Masivo de Precios ───────────────────────────────────────────
        private bool _mostrarPopupAjuste;
        public bool MostrarPopupAjuste
        {
            get => _mostrarPopupAjuste;
            set { _mostrarPopupAjuste = value; NotifyOfPropertyChange(() => MostrarPopupAjuste); }
        }

        private string _tipoAjuste = "porcentaje"; // "porcentaje" | "ganancia" | "fijo"
        public string TipoAjuste
        {
            get => _tipoAjuste;
            set { _tipoAjuste = value; NotifyOfPropertyChange(() => TipoAjuste); }
        }

        private decimal _porcentajeAjuste = 0;
        public decimal PorcentajeAjuste
        {
            get => _porcentajeAjuste;
            set { _porcentajeAjuste = value; NotifyOfPropertyChange(() => PorcentajeAjuste); }
        }

        private decimal _montoFijo = 0;
        public decimal MontoFijo
        {
            get => _montoFijo;
            set { _montoFijo = value; NotifyOfPropertyChange(() => MontoFijo); }
        }

        private ObservableCollection<ProductoListadoDto> _productosPreview = new();
        public ObservableCollection<ProductoListadoDto> ProductosPreview
        {
            get => _productosPreview;
            set { _productosPreview = value; NotifyOfPropertyChange(() => ProductosPreview); }
        }

        private bool _aplicarAPrecioVenta = true;
        public bool AplicarAPrecioVenta
        {
            get => _aplicarAPrecioVenta;
            set { _aplicarAPrecioVenta = value; NotifyOfPropertyChange(() => AplicarAPrecioVenta); }
        }

        private bool _aplicarAPrecioCosto = false;
        public bool AplicarAPrecioCosto
        {
            get => _aplicarAPrecioCosto;
            set { _aplicarAPrecioCosto = value; NotifyOfPropertyChange(() => AplicarAPrecioCosto); }
        }

        private int _productosActualizados;
        public int ProductosActualizados
        {
            get => _productosActualizados;
            set { _productosActualizados = value; NotifyOfPropertyChange(() => ProductosActualizados); }
        }

        // ── Lifecycle ─────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await CargarCategoriasAsync();
            await CargarAsync();
            await CargarProveedoresAjusteAsync();
        }

        private async Task CargarCategoriasAsync()
        {
            try
            {
                var categorias = await _productoServicio.ObtenerCategoriasAsync(_shell.IdEmpresaActual);
                Categorias = new ObservableCollection<CategoriaItemDto>(categorias);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error cargando categorías");
                MessageBox.Show("Error al cargar categorías: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CargarAsync()
        {
            try
            {
                IsLoading = true;
                var productos = await _productoServicio.ObtenerTodosAsync(_shell.IdEmpresaActual);
                var productosList = productos.ToList();

                // Aplicar filtros
                var filtrados = productosList.AsEnumerable();

                // Filtro por texto de búsqueda
                if (!string.IsNullOrWhiteSpace(TextoBusqueda))
                {
                    var busqueda = TextoBusqueda.Trim().ToLower();
                    filtrados = filtrados.Where(p =>
                        (p.Nombre?.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (p.CodigoBarra?.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ?? false));
                }

                // Filtro por categoría
                if (CategoriaSeleccionada != null)
                {
                    filtrados = filtrados.Where(p => p.IdCategoria == CategoriaSeleccionada.IdCategoria);
                }

                // Filtro por estado activo/inactivo
                if (FiltroActivo == 1)
                    filtrados = filtrados.Where(p => p.Activo);
                else if (FiltroActivo == 2)
                    filtrados = filtrados.Where(p => !p.Activo);

                var filtradosList = filtrados.ToList();
                Productos = new ObservableCollection<ProductoListadoDto>(filtradosList);
                TotalProductos = filtradosList.Count;

                // Métricas (sobre toda la lista, no filtrada)
                ProductosActivos = productosList.Count(p => p.Activo);
                ProductosStockBajo = productosList.Count(p => p.StockActual > 0 && p.StockActual <= 10);
                ProductosSinStock = productosList.Count(p => p.StockActual <= 0);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error cargando productos");
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Acciones ──────────────────────────────────────────────────
        public async Task Buscar()
        {
            if (!await _lock.WaitAsync(0)) return;
            try
            {
                PaginaActual = 1;
                await CargarAsync();
            }
            finally
            {
                _lock.Release();
                _isInitializing = false;
            }
        }

        public async Task NuevoProducto()
        {
            // ProductoFormularioViewModel sirve tanto para crear como para editar
            var vm = IoC.Get<ProductoFormularioViewModel>();
            vm.InicializarParaCrear();
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        // ── Ajuste Masivo de Precios ─────────────────────────────────
        public void AbrirAjusteMasivo()
        {
            MostrarPopupAjuste = true;
            PorcentajeAjuste = 0;
            MontoFijo = 0;
            TipoAjuste = "porcentaje";
            // Pre-cargar preview con todos los productos visibles
            _productosPreview = new ObservableCollection<ProductoListadoDto>(Productos);
            NotifyOfPropertyChange(() => ProductosPreview);
        }

        // Nueva: Ajuste por Proveedor (aplicación en modal de precios)
        public ObservableCollection<Proveedor> ProveedoresAjuste { get; set; } = new ObservableCollection<Proveedor>();
        public int? ProveedorAjusteId { get; set; }
        public decimal PorcentajeAjusteProveedor { get; set; }

        public async Task CargarProveedoresAjusteAsync()
        {
            var proveedores = await _productoServicio.ObtenerProveedoresAsync();
            ProveedoresAjuste = new ObservableCollection<Proveedor>(proveedores);
            NotifyOfPropertyChange(() => ProveedoresAjuste);
        }

        public async Task AplicarAjusteProveedorAsync()
        {
            if (ProveedorAjusteId == null || ProveedorAjusteId <= 0)
            {
                MessageBox.Show("Seleccione un proveedor para el ajuste", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (PorcentajeAjusteProveedor == 0)
            {
                MessageBox.Show("Indique un porcentaje distinto de 0 para el ajuste", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            // Confirmación simple para el usuario final
            var confirm = MessageBox.Show($"¿Está seguro de aplicar el ajuste de {PorcentajeAjusteProveedor}% para el proveedor seleccionado?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            var (nuevos, actualizados) = await _productoServicio.AjustePreciosPorProveedorAsync(ProveedorAjusteId.Value, PorcentajeAjusteProveedor);
            ProductosActualizados = actualizados;
            Status = $"Ajuste aplicado: {actualizados} productos actualizados";
            NotifyOfPropertyChange(() => Status);
            await CargarAsync();
        }

        public void CancelarAjusteMasivo()
        {
            MostrarPopupAjuste = false;
            ProductosPreview.Clear();
        }

        public void GenerarPreviewAjuste()
        {
            // Generar preview sin modificar la base de datos
            var preview = new ObservableCollection<ProductoListadoDto>();
            
            // Si no hay valor, mostrar todos los productos actuales
            if (PorcentajeAjuste == 0 && MontoFijo == 0)
            {
                foreach (var p in Productos)
                {
                    var copia = new ProductoListadoDto
                    {
                        IdProducto = p.IdProducto,
                        Nombre = p.Nombre,
                        CodigoBarra = p.CodigoBarra,
                        PrecioVentaActual = p.PrecioVentaActual,
                        PrecioCostoActual = p.PrecioCostoActual,
                        PrecioVentaNuevo = p.PrecioVentaActual,
                        PrecioCostoNuevo = p.PrecioCostoActual,
                    };
                    preview.Add(copia);
                }
                ProductosPreview = preview;
                ProductosActualizados = preview.Count;
                return;
            }
            
            foreach (var p in Productos)
            {
                decimal nuevoVenta = p.PrecioVentaActual;
                decimal nuevoCosto = p.PrecioCostoActual;

                if (TipoAjuste == "porcentaje")
                {
                    if (AplicarAPrecioVenta)
                        nuevoVenta = Math.Round(p.PrecioVentaActual * (1 + PorcentajeAjuste / 100m), 2);
                    if (AplicarAPrecioCosto)
                        nuevoCosto = Math.Round(p.PrecioCostoActual * (1 + PorcentajeAjuste / 100m), 2);
                }
                else if (TipoAjuste == "fijo")
                {
                    if (AplicarAPrecioVenta)
                        nuevoVenta = p.PrecioVentaActual + MontoFijo;
                    if (AplicarAPrecioCosto)
                        nuevoCosto = p.PrecioCostoActual + MontoFijo;
                }

                // Siempre agregar paraver el cambio
                var copia = new ProductoListadoDto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    CodigoBarra = p.CodigoBarra,
                    PrecioVentaActual = p.PrecioVentaActual,
                    PrecioCostoActual = p.PrecioCostoActual,
                    PrecioVentaNuevo = nuevoVenta,
                    PrecioCostoNuevo = nuevoCosto,
                };
                preview.Add(copia);
            }

            ProductosPreview = preview;
            ProductosActualizados = preview.Count;
        }

        public async Task ConfirmarAjusteMasivo()
        {
            if (ProductosPreview.Count == 0)
            {
                MessageBox.Show("No hay productos para actualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Se actualizarán {ProductosActualizados} productos.\n\nPrecio de venta: {(AplicarAPrecioVenta ? "✓ SÍ" : "✗ NO")}\nPorcentaje: {PorcentajeAjuste}%\n\n¿Continuar?",
                "Confirmar Ajuste Masivo",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                int actualizada = 0;

                foreach (var p in ProductosPreview)
                {
                    // Obtener producto actual de la DB para actualizar
                    var producto = await _productoServicio.ObtenerPorIdAsync(p.IdProducto);
                    if (producto != null)
                    {
                        if (AplicarAPrecioVenta)
                            producto.PrecioVentaActual = p.PrecioVentaNuevo ?? producto.PrecioVentaActual;
                        else
                            producto.PrecioCostoActual = p.PrecioVentaNuevo ?? producto.PrecioCostoActual;

                        // Map to DTO
                        var dto = new ProductoActualizarDto
                        {
                            IdProducto = producto.IdProducto,
                            Nombre = producto.Nombre,
                            CodigoBarra = producto.CodigoBarra,
                            PrecioVentaActual = producto.PrecioVentaActual,
                            PrecioCostoActual = producto.PrecioCostoActual,
                            StockMinimo = producto.StockMinimo,
                            Activo = producto.Activo,
                            IdCategoria = producto.IdCategoria,
                            IdUnidadMedida = producto.IdUnidadMedida
                        };
                        
                        await _productoServicio.ActualizarAsync(dto);
                        actualizada++;
                    }
                }

                MessageBox.Show($"Se actualizaron {actualizada} productos correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                
                MostrarPopupAjuste = false;
                await CargarAsync(); // Recargar lista
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error en ajuste masivo");
                MessageBox.Show("Error al aplicar ajuste: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task EditarProducto()
        {
            if (ProductoSeleccionado == null) return;
            var vm = IoC.Get<ProductoFormularioViewModel>();
            vm.InicializarParaEditar(ProductoSeleccionado.IdProducto);
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public void CerrarDetalle() => ProductoSeleccionado = null;

        public async Task VerMovimientos()
        {
            // TODO: navegar a movimientos filtrado por ProductoSeleccionado
            await Task.CompletedTask;
        }

        public async void DesactivarProducto()
        {
            if (ProductoSeleccionado == null) return;
            var result = MessageBox.Show($"¿Desactivar producto {ProductoSeleccionado.Nombre}?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                await _productoServicio.DesactivarAsync(ProductoSeleccionado.IdProducto);
                await CargarAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error desactivando producto");
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Paginación ────────────────────────────────────────────────
        public bool CanPaginaAnterior => PaginaActual > 1;
        public async Task PaginaAnterior()
        {
            if (PaginaActual > 1) { PaginaActual--; await CargarAsync(); }
            NotifyOfPropertyChange(() => CanPaginaAnterior);
            NotifyOfPropertyChange(() => CanPaginaSiguiente);
        }

        public bool CanPaginaSiguiente => PaginaActual < TotalPaginas;
        public async Task PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas) { PaginaActual++; await CargarAsync(); }
            NotifyOfPropertyChange(() => CanPaginaAnterior);
            NotifyOfPropertyChange(() => CanPaginaSiguiente);
        }
    }
}
