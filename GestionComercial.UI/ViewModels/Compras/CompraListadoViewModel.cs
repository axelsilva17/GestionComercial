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
using System.Windows.Threading;

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

            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private async void DebounceTimer_Tick(object? sender, EventArgs e)
        {
            _debounceTimer.Stop();
            await RecargarAsync();
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
        private string _busquedaProveedor = string.Empty;
        public string BusquedaProveedor
        {
            get => _busquedaProveedor;
            set
            {
                if (_busquedaProveedor == value) return;
                _busquedaProveedor = value;
                NotifyOfPropertyChange(() => BusquedaProveedor);

                // Debounce: buscar después de 300ms de inactividad
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }

        // Timer para debounce de búsqueda por proveedor
        private readonly DispatcherTimer _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };

        private DateTime _fechaDesde = DateTime.Today.AddDays(-30);
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set 
            { 
                if (_fechaDesde == value) return;
                _fechaDesde = value; 
                NotifyOfPropertyChange(() => FechaDesde);
                ProgramarRecargaPorFechas();
            }
        }

        private DateTime _fechaHasta = DateTime.Today;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set 
            { 
                if (_fechaHasta == value) return;
                _fechaHasta = value; 
                NotifyOfPropertyChange(() => FechaHasta);
                ProgramarRecargaPorFechas();
            }
        }

        // ── Filtro de fecha ───────────────────────────────────────────────────
        // Las fechas del rango siempre se aplican (con "Todos" y con proveedor específico).

        // Recarga (con debounce) cuando cambian las fechas
        private void ProgramarRecargaPorFechas()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
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

                // Recargar la lista filtrada por proveedor (evitar al restaurar la selección en CargarAsync)
                if (!_restaurandoProveedor)
                {
                    _ = RecargarPorProveedorAsync();
                }
            }
        }

        // Flag para evitar recargar al restaurar la selección del ComboBox dentro de CargarAsync
        private bool _restaurandoProveedor;

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

        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            // Detener el debounce al salir de la vista para evitar recargas en segundo plano
            _debounceTimer.Stop();
            await base.OnDeactivateAsync(close, cancellationToken);
        }

        ///         /// Método público para precargar proveedores desde otro ViewModel.
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
                    
                    listaProveedores.Insert(0, new ProveedorItemDto
                    {
                        IdProveedor = 0,
                        Nombre = "Todos",
                        Telefono = string.Empty,
                        Email = string.Empty,
                        Activo = true
                    });
                    
                    Proveedores = new ObservableCollection<ProveedorItemDto>(listaProveedores);
                }
                
                // Restaurar la selección del proveedor anterior
                _restaurandoProveedor = true;
                try
                {
                    if (proveedorSeleccionado != null)
                    {
                        proveedorSeleccionado = Proveedores.FirstOrDefault(p => p.IdProveedor == proveedorSeleccionado.IdProveedor);
                        _proveedorFiltro = proveedorSeleccionado;
                    }
                }
                finally
                {
                    _restaurandoProveedor = false;
                }
                
                // Cargar compras paginadas con métricas SQL
                var desde = FechaDesde.Date;
                var hasta = FechaHasta.Date.AddDays(1).AddTicks(-1);
                
                // Métricas agregadas en SQL
                var metricas = await _compraServicio.ObtenerMetricasComprasAsync(_sesion.IdSucursal, desde, hasta);
                if (metricas != null)
                {
                    TotalComprasMes = metricas.Total;
                    CantidadComprasMes = metricas.Count;
                    PromedioCompra = metricas.Promedio;
                    ProveedorTop = metricas.ProveedorTop;
                    ProductosRepuestos = metricas.ProductosRepuestos;
                }
                
                // Compras paginadas
                var (items, totalCount) = await _compraServicio.ObtenerPorSucursalPaginadoAsync(
                    _sesion.IdSucursal, desde, hasta, PaginaActual, 20,
                    ProveedorFiltro is { IdProveedor: > 0 } ? ProveedorFiltro.IdProveedor : null,
                    string.IsNullOrWhiteSpace(BusquedaProveedor) ? null : BusquedaProveedor);
                
                Compras = new ObservableCollection<CompraDto>(items);
                TotalCompras = totalCount;
                TotalPaginas = (int)Math.Ceiling((double)totalCount / 20);
                ComprasMostradas = Compras.Count;
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // Recarga genérica: resetea a la primera página y vuelve a cargar filtrado
        private async Task RecargarAsync()
        {
            PaginaActual = 1;
            await CargarAsync();
        }

        // Recarga específicamente al cambiar el proveedor seleccionado en el ComboBox
        private async Task RecargarPorProveedorAsync()
        {
            PaginaActual = 1;
            await CargarAsync();
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