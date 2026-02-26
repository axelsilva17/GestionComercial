using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.Aplicacion.DTOs.Productos;  // ProductoItemDto + CategoriaItemDto
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ProductoListadoViewModel : NavigableViewModel
    {
        public ProductoListadoViewModel()
        {
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
        private ObservableCollection<ProductoItemDto> _productos = new();
        public ObservableCollection<ProductoItemDto> Productos
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
        private ProductoItemDto _productoSeleccionado;
        public ProductoItemDto ProductoSeleccionado
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
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // TODO: cargar desde servicios reales
                await Task.Delay(200);
                Productos          = new ObservableCollection<ProductoItemDto>();
                Categorias         = new ObservableCollection<CategoriaItemDto>();
                TotalProductos     = 0;
                ProductosMostrados = 0;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
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

        public async Task DesactivarProducto()
        {
            if (ProductoSeleccionado == null) return;
            // TODO: await _productoServicio.DesactivarAsync(ProductoSeleccionado.IdProducto);
            await CargarAsync();
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
