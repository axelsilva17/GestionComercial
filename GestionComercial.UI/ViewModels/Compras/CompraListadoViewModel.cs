using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Compras
{
    // ── ViewModel ─────────────────────────────────────────────────────────────
    public class CompraListadoViewModel : NavigableViewModel
    {
        private readonly IProveedorServicio _proveedorServicio;
        private readonly ICompraServicio _compraServicio;
        private readonly SesionServicio _sesion;

        public CompraListadoViewModel(
            IProveedorServicio proveedorServicio,
            ICompraServicio compraServicio,
            SesionServicio sesion)
        {
            _proveedorServicio = proveedorServicio;
            _compraServicio = compraServicio;
            _sesion = sesion;
            Titulo    = "Compras";
            Subtitulo = "Historial de órdenes de compra";
        }

        // ── Listas ─────────────────────────────────────────────────────────────
        private ObservableCollection<CompraDto> _compras = new();
        public ObservableCollection<CompraDto> Compras
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

        private CompraDto _compraSeleccionada;
        public CompraDto CompraSeleccionada
        {
            get => _compraSeleccionada;
            set { _compraSeleccionada = value; NotifyOfPropertyChange(() => CompraSeleccionada); }
        }

        // ── Métricas ──────────────────────────────────────────────────────────
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

        private int _productosRepuestos;
        public int ProductosRepuestos
        {
            get => _productosRepuestos;
            set { _productosRepuestos = value; NotifyOfPropertyChange(() => ProductosRepuestos); }
        }

        private string _proveedorTop = "—";
        public string ProveedorTop
        {
            get => _proveedorTop;
            set { _proveedorTop = value; NotifyOfPropertyChange(() => ProveedorTop); }
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
            set 
            { 
                if (_fechaDesde == value) return;
                _fechaDesde = value; 
                NotifyOfPropertyChange(() => FechaDesde);
                // Auto-filtro al cambiar fecha
                _ = Task.Run(async () => await CargarAsync());
            }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set 
            { 
                if (_fechaHasta == value) return;
                _fechaHasta = value; 
                NotifyOfPropertyChange(() => FechaHasta);
                // Auto-filtro al cambiar fecha
                _ = Task.Run(async () => await CargarAsync());
            }
        }

        private ProveedorItemDto _proveedorFiltro;
        public ProveedorItemDto ProveedorFiltro
        {
            get => _proveedorFiltro;
            set 
            { 
                if (ReferenceEquals(_proveedorFiltro, value)) return;
                _proveedorFiltro = value; 
                NotifyOfPropertyChange(() => ProveedorFiltro);
                // Auto-filtro al cambiar proveedor
                _ = Task.Run(async () => await CargarAsync());
            }
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

        /// <summary>
        /// Método público para precargar proveedores desde otro ViewModel.
        /// </summary>
        public async Task<IEnumerable<ProveedorItemDto>> CargarProveedoresAsync()
        {
            if (Proveedores != null && Proveedores.Count > 0)
                return Proveedores;
            
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
            return Proveedores;
        }

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // Mantener el proveedor seleccionado antes de recargar
                var proveedorSeleccionado = ProveedorFiltro;
                
                // solo cargar proveedores si es la primera vez o está vacío
                if (Proveedores == null || Proveedores.Count == 0)
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
                
                // Restaurar la selección del proveedor anterior
                if (proveedorSeleccionado != null)
                {
                    proveedorSeleccionado = Proveedores.FirstOrDefault(p => p.IdProveedor == proveedorSeleccionado.IdProveedor);
                    _proveedorFiltro = proveedorSeleccionado;
                }
                
                // Cargar compras con filtros
                DateTime desde = FechaDesde ?? DateTime.Now.AddMonths(-1);
                DateTime hasta = FechaHasta ?? DateTime.Now;
                
                var compras = await _compraServicio.ObtenerPorPeriodoAsync(
                    _sesion.IdSucursal, desde, hasta.AddDays(1));
                
                // Aplicar filtros
                var filtered = compras.AsEnumerable();
                
                if (ProveedorFiltro != null)
                {
                    filtered = filtered.Where(c => c.Id_proveedor == ProveedorFiltro.IdProveedor);
                }
                
                if (!string.IsNullOrWhiteSpace(TextoBusqueda))
                    filtered = filtered.Where(c => c.ProveedorNombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase));
                
                var lista = filtered.ToList();
                
                Compras = new ObservableCollection<CompraDto>(lista);
                TotalCompras = lista.Count;
                ComprasMostradas = lista.Count;
                
                // Calcular métricas sobre la lista FILTRADA
                TotalComprasMes = lista.Sum(c => c.Total);
                CantidadComprasMes = lista.Count;
                PromedioCompra = CantidadComprasMes > 0 ? TotalComprasMes / CantidadComprasMes : 0;
                
                // Proveedor más frecuente: calcular sobre TODAS las compras del período (sin filtro)
                var proveedorAgrupado = compras
                    .GroupBy(c => c.ProveedorNombre)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();
                ProveedorTop = proveedorAgrupado?.Key ?? "—";
                
                // Productos repuestos (suma de cantidad de items)
                ProductosRepuestos = lista.SelectMany(c => c.Items).Sum(i => i.Cantidad);
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