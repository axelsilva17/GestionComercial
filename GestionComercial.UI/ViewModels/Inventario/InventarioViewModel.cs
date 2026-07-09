using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Inventario;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Inventario
{
    public class InventarioViewModel : NavigableViewModel
    {
        private readonly IInventarioServicio _inventarioServicio;
        private readonly IProductoServicio _productoServicio;
        private readonly ShellViewModel _shell;
        private readonly ILogger<InventarioViewModel>? _logger;
        private const int ItemsPorPagina = 15;

        // Cache de productos para búsqueda
        private List<ProductoListadoDto> _productosCache = new();

        public InventarioViewModel(
            IInventarioServicio inventarioServicio,
            IProductoServicio productoServicio,
            ShellViewModel shell,
            ILogger<InventarioViewModel>? logger = null)
        {
            _inventarioServicio = inventarioServicio;
            _productoServicio = productoServicio;
            _shell = shell;
            _logger = logger;

            Titulo    = "Inventario";
            Subtitulo = "Movimientos de stock";

            // Inicializar función de búsqueda para el BuscadorProducto
            BuscarProductosFunc = BuscarProductosInternoAsync;
        }

        // ── Propiedad para BuscadorProducto (WPF no puede bindear métodos directamente) ─
        public Func<string, Task<IEnumerable<ProductoDto>>> BuscarProductosFunc { get; }

        // ── Método interno para búsqueda de productos ───────────────────────────────
        private async Task<IEnumerable<ProductoDto>> BuscarProductosInternoAsync(string texto)
        {
            // Cargar cache si está vacío
            if (_productosCache.Count == 0)
            {
                try
                {
                    var todos = await _productoServicio.ObtenerTodosAsync(IdEmpresa);
                    _productosCache = todos.ToList();
                }
                catch
                {
                    return Enumerable.Empty<ProductoDto>();
                }
            }

            // Filtrar por nombre o código
            if (string.IsNullOrWhiteSpace(texto))
                return Enumerable.Empty<ProductoDto>();

            var busqueda = texto.Trim().ToLowerInvariant();
            return _productosCache
                .Where(p => (p.Nombre?.ToLowerInvariant().Contains(busqueda) ?? false) ||
                           (p.CodigoBarra?.ToLowerInvariant().Contains(busqueda) ?? false))
                .Take(10)
                .Select(p => new ProductoDto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    CodigoBarra = p.CodigoBarra,
                    PrecioVentaActual = p.PrecioVentaActual,
                    StockActual = p.StockActual,
                    IdCategoria = p.IdCategoria,
                    CategoriaNombre = p.CategoriaNombre
                });
        }

        // ── Lista principal ──────────────────────────────────────────────────
        private ObservableCollection<MovimientoStockDto> _movimientos = new();
        public ObservableCollection<MovimientoStockDto> Movimientos
        {
            get => _movimientos;
            set { _movimientos = value; NotifyOfPropertyChange(() => Movimientos); }
        }

        // ── Filtros ──────────────────────────────────────────────────────────
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private string _filtroTipo = "Todos";
        public string FiltroTipo
        {
            get => _filtroTipo;
            set { _filtroTipo = value; NotifyOfPropertyChange(() => FiltroTipo); }
        }

        // ── Filtro usuario ───────────────────────────────────────────────────
        private string _filtroUsuario = "Todos";
        public string FiltroUsuario
        {
            get => _filtroUsuario;
            set { _filtroUsuario = value; NotifyOfPropertyChange(() => FiltroUsuario); }
        }

        private ObservableCollection<string> _usuarios = new() { "Todos" };
        public ObservableCollection<string> Usuarios
        {
            get => _usuarios;
            set { _usuarios = value; NotifyOfPropertyChange(() => Usuarios); }
        }

        private DateTime _fechaDesde = DateTime.Today.AddDays(-30);
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }

        private DateTime _fechaHasta = DateTime.Today;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }

        public ObservableCollection<string> TiposFiltro { get; } = new()
        {
            "Todos", "Entrada", "Salida", "Ajuste"
        };

        // ── Filtro sucursal ──────────────────────────────────────────────────
        private string _filtroSucursal = "Todas";
        public string FiltroSucursal
        {
            get => _filtroSucursal;
            set { _filtroSucursal = value; NotifyOfPropertyChange(() => FiltroSucursal); }
        }

        private ObservableCollection<string> _sucursales = new() { "Todas" };
        public ObservableCollection<string> Sucursales
        {
            get => _sucursales;
            set { _sucursales = value; NotifyOfPropertyChange(() => Sucursales); }
        }

        // ── Paginación ───────────────���───────────────────────────────────────
        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set
            {
                _paginaActual = value;
                NotifyOfPropertyChange(() => PaginaActual);
                NotifyOfPropertyChange(() => PuedePaginaAnterior);
                NotifyOfPropertyChange(() => PuedePaginaSiguiente);
                NotifyOfPropertyChange(() => TextoPaginacion);
            }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set
            {
                _totalPaginas = value;
                NotifyOfPropertyChange(() => TotalPaginas);
                NotifyOfPropertyChange(() => PuedePaginaAnterior);
                NotifyOfPropertyChange(() => PuedePaginaSiguiente);
                NotifyOfPropertyChange(() => TextoPaginacion);
            }
        }

        public bool   PuedePaginaAnterior  => PaginaActual > 1;
        public bool   PuedePaginaSiguiente => PaginaActual < TotalPaginas;
        public string TextoPaginacion      => $"Página {PaginaActual} de {TotalPaginas}";

        private int _totalMovimientos;
        public int TotalMovimientos
        {
            get => _totalMovimientos;
            set { _totalMovimientos = value; NotifyOfPropertyChange(() => TotalMovimientos); }
        }

        private int _movimientosMostrados;
        public int MovimientosMostrados
        {
            get => _movimientosMostrados;
            set { _movimientosMostrados = value; NotifyOfPropertyChange(() => MovimientosMostrados); }
        }

        // ── Resumen del período ───────────────────────────────────────────────
        private int _totalEntradas;
        public int TotalEntradas
        {
            get => _totalEntradas;
            set { _totalEntradas = value; NotifyOfPropertyChange(() => TotalEntradas); }
        }

        private int _totalSalidas;
        public int TotalSalidas
        {
            get => _totalSalidas;
            set { _totalSalidas = value; NotifyOfPropertyChange(() => TotalSalidas); }
        }

        private int _totalAjustes;
        public int TotalAjustes
        {
            get => _totalAjustes;
            set { _totalAjustes = value; NotifyOfPropertyChange(() => TotalAjustes); }
        }

        private int _unidadesIngresadas;
        public int UnidadesIngresadas
        {
            get => _unidadesIngresadas;
            set { _unidadesIngresadas = value; NotifyOfPropertyChange(() => UnidadesIngresadas); }
        }

        private int _unidadesEgresadas;
        public int UnidadesEgresadas
        {
            get => _unidadesEgresadas;
            set { _unidadesEgresadas = value; NotifyOfPropertyChange(() => UnidadesEgresadas); }
        }

        // ── Balance neto (Ingresadas - Egresadas) ───────────────────────────
        public int BalanceNeto => UnidadesIngresadas - UnidadesEgresadas;

        // ── Panel nuevo movimiento ───────────────────────────────────────────
        private bool _panelVisible;
        public bool PanelVisible
        {
            get => _panelVisible;
            set { _panelVisible = value; NotifyOfPropertyChange(() => PanelVisible); }
        }

        private string _nuevoTipo = "Entrada";
        public string NuevoTipo
        {
            get => _nuevoTipo;
            set { _nuevoTipo = value; NotifyOfPropertyChange(() => NuevoTipo); }
        }

        private int _nuevaCantidad = 1;
        public int NuevaCantidad
        {
            get => _nuevaCantidad;
            set { _nuevaCantidad = value; NotifyOfPropertyChange(() => NuevaCantidad); }
        }

        private string _nuevaObservacion = string.Empty;
        public string NuevaObservacion
        {
            get => _nuevaObservacion;
            set { _nuevaObservacion = value; NotifyOfPropertyChange(() => NuevaObservacion); }
        }

        // ProductoDto viene de GestionComercial.Aplicacion.DTOs.Productos
        private ProductoDto _productoSeleccionado;
        public ProductoDto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                NotifyOfPropertyChange(() => ProductoSeleccionado);
                NotifyOfPropertyChange(() => TieneProducto);
                NotifyOfPropertyChange(() => StockActualProducto);
            }
        }

        public bool   TieneProducto       => ProductoSeleccionado != null;
        public string StockActualProducto => ProductoSeleccionado != null
            ? $"Stock actual: {ProductoSeleccionado.StockActual} unidades"
            : string.Empty;

        public ObservableCollection<string> TiposMovimiento { get; } = new()
        {
            "Entrada", "Salida", "Ajuste"
        };

        // ── Activación ───────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        // ── Carga con filtros y paginación ───────────────────────────────────
        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // Obtener movimientos desde el servicio real (paginación en SQL)
                var resultado = await _inventarioServicio.ObtenerMovimientosAsync(
                    TextoBusqueda,
                    FiltroTipo,
                    FiltroUsuario,
                    FiltroSucursal,
                    FechaDesde,
                    FechaHasta,
                    PaginaActual,
                    ItemsPorPagina,
                    IdEmpresa);

                Movimientos = new ObservableCollection<MovimientoStockDto>(resultado.Items);
                TotalMovimientos = resultado.TotalItems;
                TotalPaginas = resultado.TotalPaginas;
                PaginaActual = Math.Min(PaginaActual, TotalPaginas);

                MovimientosMostrados = Movimientos.Count;

                // Calcular resumen del período (sin paginación)
                await CalcularResumenPeriodoAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al cargar inventario");
                MostrarError(ex.Message);
            }
            finally { IsLoading = false; }
        }

        private async Task CalcularResumenPeriodoAsync()
        {
            // Obtener todos los movimientos del período para el resumen (sin paginación)
            var resultado = await _inventarioServicio.ObtenerMovimientosAsync(
                null,  // sin filtro búsqueda
                null,  // sin filtro tipo
                null,  // sin filtro usuario
                null,  // sin filtro sucursal
                FechaDesde,
                FechaHasta,
                1,     // página 1
                int.MaxValue, // todos los registros
                IdEmpresa);

            var lista = resultado.Items;

            TotalEntradas      = lista.Count(m => m.TipoMovimiento == "Entrada");
            TotalSalidas       = lista.Count(m => m.TipoMovimiento == "Salida");
            TotalAjustes       = lista.Count(m => m.TipoMovimiento == "Ajuste");
            UnidadesIngresadas = lista.Where(m => m.TipoMovimiento == "Entrada").Sum(m => m.Cantidad);
            UnidadesEgresadas  = lista.Where(m => m.TipoMovimiento == "Salida").Sum(m => m.Cantidad);

            NotifyOfPropertyChange(() => BalanceNeto);
        }

        // ── Acciones ─────────────────────────────────────────────────────────
        public async Task Buscar()        { PaginaActual = 1; await CargarAsync(); }
        public async Task AplicarFiltros(){ PaginaActual = 1; await CargarAsync(); }

        public async Task LimpiarFiltros()
        {
            TextoBusqueda = string.Empty;
            FiltroTipo    = "Todos";
            FiltroUsuario = "Todos";
            FechaDesde      = DateTime.Today.AddDays(-30);
            FechaHasta      = DateTime.Today;
            FiltroSucursal  = "Todas";
            PaginaActual    = 1;
            await CargarAsync();
        }

        public async Task ExportarExcel()
        {
            try
            {
                IsLoading = true;
                var excel = await _inventarioServicio.ExportarAExcelAsync(
                    TextoBusqueda,
                    FiltroTipo,
                    FiltroUsuario,
                    FiltroSucursal,
                    FechaDesde,
                    FechaHasta,
                    IdEmpresa);

                // Guardar archivo
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    FileName = $"Inventario_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (dialog.ShowDialog() == true)
                {
                    await System.IO.File.WriteAllBytesAsync(dialog.FileName, excel);
                    MostrarError("Exportación completada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al exportar inventario");
                MostrarError($"Error al exportar: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        public async Task PaginaAnterior()
        {
            if (PuedePaginaAnterior) { PaginaActual--; await CargarAsync(); }
        }

        public async Task PaginaSiguiente()
        {
            if (PuedePaginaSiguiente) { PaginaActual++; await CargarAsync(); }
        }

        public void AbrirNuevoMovimiento()
        {
            NuevoTipo            = "Entrada";
            NuevaCantidad        = 1;
            NuevaObservacion     = string.Empty;
            ProductoSeleccionado = null;
            LimpiarError();
            PanelVisible = true;
        }

        public void CerrarPanel() => PanelVisible = false;

        public async Task GuardarMovimiento()
        {
            if (ProductoSeleccionado == null) { MostrarError("Seleccioná un producto.");        return; }
            if (NuevaCantidad <= 0)           { MostrarError("La cantidad debe ser mayor a 0."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                // Obtener el ID de la sucursal del contexto
                int idSucursal = IdSucursal;
                int idUsuario = IdUsuario;

                await _inventarioServicio.RegistrarMovimientoAsync(
                    ProductoSeleccionado.IdProducto,
                    NuevoTipo,
                    NuevaCantidad,
                    string.IsNullOrWhiteSpace(NuevaObservacion) ? null : NuevaObservacion,
                    idSucursal,
                    idUsuario);

                PanelVisible = false;
                await CargarAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error al guardar movimiento de stock");
                MostrarError(ex.Message);
            }
            finally { IsLoading = false; }
        }

        // ── Propiedades auxiliares para el ViewModel ─────────────────────────
        private int IdEmpresa => _shell.IdEmpresaActual;
        private int IdSucursal => _shell.IdSucursalActual;
        private int IdUsuario => _shell.SesionActual?.IdUsuario ?? 0;
    }
}