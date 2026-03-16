using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Proveedores;
namespace GestionComercial.UI.ViewModels.Compras
{

    // ── ViewModel ─────────────────────────────────────────────────────────────
    public class CompraListadoViewModel : NavigableViewModel
    {
        public CompraListadoViewModel()
        {
            Titulo    = "Compras";
            Subtitulo = "Historial de órdenes de compra";
        }

        private ObservableCollection<CompraDetalleDto> _compras = new();
        public ObservableCollection<CompraDetalleDto> Compras
        {
            get => _compras;
            set { _compras = value; NotifyOfPropertyChange(() => Compras); }
        }

        private ObservableCollection<ProveedorItemDto> _proveedores = new();
        public ObservableCollection<ProveedorItemDto> Proveedores
        {
            get => _proveedores;
            set { _proveedores = value; NotifyOfPropertyChange(() => Proveedores); }
        }

        private CompraDetalleDto _compraSeleccionada;
        public CompraDetalleDto CompraSeleccionada
        {
            get => _compraSeleccionada;
            set { _compraSeleccionada = value; NotifyOfPropertyChange(() => CompraSeleccionada); }
        }

        // Métricas
        private decimal _totalComprasMes;
        public decimal TotalComprasMes
        {
            get => _totalComprasMes;
            set { _totalComprasMes = value; NotifyOfPropertyChange(() => TotalComprasMes); }
        }

        private int _cantidadComprasMes;
        public int CantidadComprasMes
        {
            get => _cantidadComprasMes;
            set { _cantidadComprasMes = value; NotifyOfPropertyChange(() => CantidadComprasMes); }
        }

        private decimal _promedioCompra;
        public decimal PromedioCompra
        {
            get => _promedioCompra;
            set { _promedioCompra = value; NotifyOfPropertyChange(() => PromedioCompra); }
        }

        private string _proveedorTop = "-";
        public string ProveedorTop
        {
            get => _proveedorTop;
            set { _proveedorTop = value; NotifyOfPropertyChange(() => ProveedorTop); }
        }

        private int _productosRepuestos;
        public int ProductosRepuestos
        {
            get => _productosRepuestos;
            set { _productosRepuestos = value; NotifyOfPropertyChange(() => ProductosRepuestos); }
        }

        // Filtros
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }

        private ProveedorItemDto _proveedorFiltro;
        public ProveedorItemDto ProveedorFiltro
        {
            get => _proveedorFiltro;
            set { _proveedorFiltro = value; NotifyOfPropertyChange(() => ProveedorFiltro); }
        }

        // Paginación
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

        private int _totalCompras;
        public int TotalCompras
        {
            get => _totalCompras;
            set { _totalCompras = value; NotifyOfPropertyChange(() => TotalCompras); }
        }

        private int _comprasMostradas;
        public int ComprasMostradas
        {
            get => _comprasMostradas;
            set { _comprasMostradas = value; NotifyOfPropertyChange(() => ComprasMostradas); }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(200);
                Compras           = new ObservableCollection<CompraDetalleDto>();
                TotalCompras      = 0;
                ComprasMostradas  = 0;
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task NuevaCompra()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<CompraViewModel>(), CancellationToken.None);
        }

        public async Task Buscar()
        {
            PaginaActual = 1;
            await CargarAsync();
        }

        public void CerrarDetalle() => CompraSeleccionada = null;

        public async Task PaginaAnterior()
        {
            if (PaginaActual > 1) { PaginaActual--; await CargarAsync(); }
        }

        public async Task PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas) { PaginaActual++; await CargarAsync(); }
        }
    }
}
