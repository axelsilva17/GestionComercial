using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;

namespace GestionComercial.UI.ViewModels.Productos
{
    public class ProductoListadoViewModel : NavigableViewModel
    {
        private int _totalProductos;
        private int _productosActivos;
        private int _productosStockBajo;
        private int _productosSinStock;
        private int _productosMostrados;
        private int _paginaActual = 1;
        private int _totalPaginas = 1;
        private string _textoBusqueda;

        public ProductoListadoViewModel()
        {
            Titulo = "Productos";
            Subtitulo = "Catálogo de productos";
        }

        public int TotalProductos
        {
            get => _totalProductos;
            set { _totalProductos = value; NotifyOfPropertyChange(() => TotalProductos); }
        }

        public int ProductosActivos
        {
            get => _productosActivos;
            set { _productosActivos = value; NotifyOfPropertyChange(() => ProductosActivos); }
        }

        public int ProductosStockBajo
        {
            get => _productosStockBajo;
            set { _productosStockBajo = value; NotifyOfPropertyChange(() => ProductosStockBajo); }
        }

        public int ProductosSinStock
        {
            get => _productosSinStock;
            set { _productosSinStock = value; NotifyOfPropertyChange(() => ProductosSinStock); }
        }

        public int ProductosMostrados
        {
            get => _productosMostrados;
            set { _productosMostrados = value; NotifyOfPropertyChange(() => ProductosMostrados); }
        }

        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; NotifyOfPropertyChange(() => PaginaActual); }
        }

        public int TotalPaginas
        {
            get => _totalPaginas;
            set { _totalPaginas = value; NotifyOfPropertyChange(() => TotalPaginas); }
        }

        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        public void Buscar() { /* TODO: implementar búsqueda contra BD */ }
        public void NuevoProducto() { /* TODO: navegar a ProductoCrearViewModel */ }
        public void PaginaAnterior() { if (PaginaActual > 1) PaginaActual--; }
        public void PaginaSiguiente() { if (PaginaActual < TotalPaginas) PaginaActual++; }
    }
}
