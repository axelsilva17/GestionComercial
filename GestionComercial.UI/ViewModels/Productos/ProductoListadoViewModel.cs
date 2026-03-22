using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Productos;  // ProductoItemDto + CategoriaItemDto
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
        private readonly ILogger<ProductoListadoViewModel> _logger;

        public ProductoListadoViewModel(IProductoServicio productoServicio, ShellViewModel shell, ILogger<ProductoListadoViewModel> logger)
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

        // ── Lifecycle ─────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await CargarCategoriasAsync();
            await CargarAsync();
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
                _logger.LogError(ex, "Error cargando categorías");
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
                _logger.LogError(ex, "Error cargando productos");
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
            PaginaActual = 1;
            await CargarAsync();
        }

        public async Task NuevoProducto()
        {
            // ProductoFormularioViewModel sirve tanto para crear como para editar
            var vm = IoC.Get<ProductoFormularioViewModel>();
            vm.InicializarParaCrear();
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
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
                _logger.LogError(ex, "Error desactivando producto");
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
