using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Inventario;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Inventario
{
    public class InventarioViewModel : NavigableViewModel
    {
        private const int ItemsPorPagina = 15;

        public InventarioViewModel()
        {
            Titulo    = "Inventario";
            Subtitulo = "Movimientos de stock";
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

        // ── Paginación ───────────────────────────────────────────────────────
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

        // ── Panel nuevo movimiento ────────────────────────────────────────────
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
                await Task.Delay(200); // TODO: reemplazar con servicio real

                var todos = GenerarMock();

                // Poblar lista de sucursales disponibles para el filtro
                var sucursalesDisponibles = todos
                    .Select(m => m.SucursalNombre)
                    .Distinct()
                    .OrderBy(s => s)
                    .ToList();
                sucursalesDisponibles.Insert(0, "Todas");
                Sucursales = new ObservableCollection<string>(sucursalesDisponibles);

                var filtrados = todos.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(TextoBusqueda))
                    filtrados = filtrados.Where(m =>
                        m.ProductoNombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                        m.CodigoBarra.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase));

                if (FiltroTipo != "Todos")
                    filtrados = filtrados.Where(m => m.TipoMovimiento == FiltroTipo);

                if (FiltroSucursal != "Todas")
                    filtrados = filtrados.Where(m => m.SucursalNombre == FiltroSucursal);

                filtrados = filtrados.Where(m =>
                    m.Fecha.Date >= FechaDesde.Date &&
                    m.Fecha.Date <= FechaHasta.Date);

                var lista = filtrados.OrderByDescending(m => m.Fecha).ToList();

                // Resumen
                TotalEntradas      = lista.Count(m => m.TipoMovimiento == "Entrada");
                TotalSalidas       = lista.Count(m => m.TipoMovimiento == "Salida");
                TotalAjustes       = lista.Count(m => m.TipoMovimiento == "Ajuste");
                UnidadesIngresadas = lista.Where(m => m.TipoMovimiento == "Entrada").Sum(m => m.Cantidad);
                UnidadesEgresadas  = lista.Where(m => m.TipoMovimiento == "Salida").Sum(m => m.Cantidad);

                // Paginación
                TotalMovimientos     = lista.Count;
                TotalPaginas         = Math.Max(1, (int)Math.Ceiling(lista.Count / (double)ItemsPorPagina));
                PaginaActual         = Math.Min(PaginaActual, TotalPaginas);

                var pagina           = lista.Skip((PaginaActual - 1) * ItemsPorPagina).Take(ItemsPorPagina).ToList();
                MovimientosMostrados = pagina.Count;
                Movimientos          = new ObservableCollection<MovimientoStockDto>(pagina);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task Buscar()        { PaginaActual = 1; await CargarAsync(); }
        public async Task AplicarFiltros(){ PaginaActual = 1; await CargarAsync(); }

        public async Task LimpiarFiltros()
        {
            TextoBusqueda = string.Empty;
            FiltroTipo    = "Todos";
            FechaDesde      = DateTime.Today.AddDays(-30);
            FechaHasta      = DateTime.Today;
            FiltroSucursal  = "Todas";
            PaginaActual    = 1;
            await CargarAsync();
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
                await Task.Delay(300); // TODO: await _inventarioServicio.RegistrarMovimiento(...)
                PanelVisible = false;
                await CargarAsync();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Mock ──────────────────────────────────────────────────────────────
        private static System.Collections.Generic.List<MovimientoStockDto> GenerarMock()
        {
            var rand       = new Random(42);
            var productos  = new[] {
                ("Auriculares Pro X", "7890001", "Electrónica"),
                ("Mouse Inalámbrico", "7890002", "Periféricos"),
                ("Teclado Mecánico",  "7890003", "Periféricos"),
                ("Monitor 24\"",      "7890004", "Monitores"),
                ("Webcam HD 1080p",   "7890005", "Periféricos"),
                ("Cable HDMI 2m",     "7890006", "Accesorios"),
                ("Hub USB 4 puertos", "7890007", "Accesorios"),
            };
            var tipos      = new[] { "Entrada", "Salida", "Ajuste" };
            var sucursales = new[] { "Casa Central", "Sucursal Norte", "Sucursal Sur" };
            var usuarios   = new[] { "Juan García", "María López", "Carlos Martínez" };

            var lista = new System.Collections.Generic.List<MovimientoStockDto>();
            for (int i = 1; i <= 60; i++)
            {
                var prod = productos[rand.Next(productos.Length)];
                var tipo = tipos[rand.Next(tipos.Length)];
                lista.Add(new MovimientoStockDto
                {
                    IdMovimiento    = i,
                    TipoMovimiento  = tipo,
                    Cantidad        = rand.Next(1, 50),
                    Observacion     = tipo == "Entrada" ? "Compra proveedor"
                                    : tipo == "Salida"  ? "Venta"
                                    : "Corrección manual",
                    Fecha           = DateTime.Today.AddDays(-rand.Next(0, 30)).AddHours(rand.Next(8, 20)),
                    IdProducto      = i,
                    ProductoNombre  = prod.Item1,
                    CodigoBarra     = prod.Item2,
                    CategoriaNombre = prod.Item3,
                    SucursalNombre  = sucursales[rand.Next(sucursales.Length)],
                    UsuarioNombre   = usuarios[rand.Next(usuarios.Length)],
                });
            }
            return lista;
        }
    }
}
