using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteStockCriticoDto
    {
        public string Nombre      { get; set; } = string.Empty;
        public int    StockActual { get; set; }
        public int    StockMinimo { get; set; }
        public string Estado      => StockActual == 0 ? "Sin stock" : "Crítico";
    }

    public class ReporteCompraRecienteDto
    {
        public string  Proveedor { get; set; } = string.Empty;
        public string  Fecha     { get; set; } = string.Empty;
        public decimal Total     { get; set; }
        public int     Productos { get; set; }
    }

    public class ReporteAdminViewModel : NavigableViewModel
    {
        private readonly IVentaServicio    _ventaServicio;
        private readonly IProductoServicio _productoServicio;
        private readonly ICompraServicio   _compraServicio;
        private readonly SesionServicio    _sesion;

        public ReporteAdminViewModel(
            IVentaServicio    ventaServicio,
            IProductoServicio productoServicio,
            ICompraServicio   compraServicio,
            SesionServicio    sesion)
        {
            _ventaServicio    = ventaServicio;
            _productoServicio = productoServicio;
            _compraServicio   = compraServicio;
            _sesion           = sesion;
            Titulo            = "Reportes";
            Subtitulo         = "Administración — operaciones";
        }

        // ── KPIs ──────────────────────────────────────────────────────────────
        private int _cantidadVentasMes;
        public int CantidadVentasMes
        {
            get => _cantidadVentasMes;
            set { _cantidadVentasMes = value; NotifyOfPropertyChange(() => CantidadVentasMes); }
        }

        private int _productosStockCritico;
        public int ProductosStockCritico
        {
            get => _productosStockCritico;
            set { _productosStockCritico = value; NotifyOfPropertyChange(() => ProductosStockCritico); }
        }

        private int _comprasDelMes;
        public int ComprasDelMes
        {
            get => _comprasDelMes;
            set { _comprasDelMes = value; NotifyOfPropertyChange(() => ComprasDelMes); }
        }

        private int _clientesNuevos;
        public int ClientesNuevos
        {
            get => _clientesNuevos;
            set { _clientesNuevos = value; NotifyOfPropertyChange(() => ClientesNuevos); }
        }

        private int _ventasPendientes;
        public int VentasPendientes
        {
            get => _ventasPendientes;
            set { _ventasPendientes = value; NotifyOfPropertyChange(() => VentasPendientes); }
        }

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<ReporteStockCriticoDto>   StockCritico     { get; set; } = new();
        public ObservableCollection<ReporteCompraRecienteDto> ComprasRecientes { get; set; } = new();

        // ── Gráfico 1: Línea ventas por día ───────────────────────────────────
        private ISeries[] _seriesVentasDia = Array.Empty<ISeries>();
        public ISeries[] SeriesVentasDia
        {
            get => _seriesVentasDia;
            set { _seriesVentasDia = value; NotifyOfPropertyChange(() => SeriesVentasDia); }
        }

        private Axis[] _ejeXVentasDia = Array.Empty<Axis>();
        public Axis[] EjeXVentasDia
        {
            get => _ejeXVentasDia;
            set { _ejeXVentasDia = value; NotifyOfPropertyChange(() => EjeXVentasDia); }
        }

        // ── Gráfico 2: Barras stock actual vs mínimo ──────────────────────────
        private ISeries[] _seriesStock = Array.Empty<ISeries>();
        public ISeries[] SeriesStock
        {
            get => _seriesStock;
            set { _seriesStock = value; NotifyOfPropertyChange(() => SeriesStock); }
        }

        private Axis[] _ejeXStock = Array.Empty<Axis>();
        public Axis[] EjeXStock
        {
            get => _ejeXStock;
            set { _ejeXStock = value; NotifyOfPropertyChange(() => EjeXStock); }
        }

        public Axis[] EjeYEntero { get; } = new[]
        {
            new Axis
            {
                LabelsPaint     = new SolidColorPaint(SKColor.Parse("#9CA3AF")),
                SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#374151")),
                MinStep = 1,
            }
        };

        public Axis[] EjeYMoneda { get; } = new[]
        {
            new Axis
            {
                LabelsPaint     = new SolidColorPaint(SKColor.Parse("#9CA3AF")),
                SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#374151")),
                Labeler = v => $"${v / 1000:N0}K",
            }
        };

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var hoy       = DateTime.Today;
                var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

                // Ventas del mes
                var ventasMes = (await _ventaServicio.ObtenerPorSucursalAsync(
                    _sesion.IdSucursal, inicioMes, hoy)).ToList();
                CantidadVentasMes = ventasMes.Count;
                VentasPendientes  = ventasMes.Count(v => v.Estado == "Pendiente");

                // Stock crítico
                var criticos = (await _productoServicio.ObtenerStockCriticoAsync(_sesion.IdEmpresa)).ToList();
                ProductosStockCritico = criticos.Count;
                StockCritico = new ObservableCollection<ReporteStockCriticoDto>(
                    criticos.Take(10).Select(p => new ReporteStockCriticoDto
                    {
                        Nombre      = p.Nombre,
                        StockActual = p.StockActual,
                        StockMinimo = p.StockMinimo,
                    }));

                // Compras del mes
                var todasCompras = (await _compraServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal))
                    .Where(c => c.Fecha >= inicioMes && c.Fecha <= hoy).ToList();
                ComprasDelMes = todasCompras.Count;
                ComprasRecientes = new ObservableCollection<ReporteCompraRecienteDto>(
                    todasCompras.OrderByDescending(c => c.Fecha).Take(8).Select(c => new ReporteCompraRecienteDto
                    {
                        Proveedor = c.ProveedorNombre,
                        Fecha     = c.Fecha.ToString("dd/MM/yyyy"),
                        Total     = c.Total,
                        Productos = c.Items?.Count ?? 0,
                    }));

                // ── Gráfico 1: ventas por día del mes ─────────────────────────
                int diasMes  = DateTime.DaysInMonth(hoy.Year, hoy.Month);
                int diasHasta = hoy.Day;
                var labels   = Enumerable.Range(1, diasHasta).Select(d => d.ToString()).ToArray();
                var valores  = Enumerable.Range(1, diasHasta).Select(d =>
                {
                    var dia = new DateTime(hoy.Year, hoy.Month, d);
                    return (double)ventasMes
                        .Where(v => v.Fecha.Date == dia)
                        .Sum(v => v.TotalFinal);
                }).ToArray();

                SeriesVentasDia = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name   = "Ventas",
                        Values = valores,
                        Stroke = new SolidColorPaint(SKColor.Parse("#6366F1")) { StrokeThickness = 2 },
                        Fill   = new LinearGradientPaint(
                            new[] { SKColor.Parse("#6366F150"), SKColor.Parse("#6366F100") },
                            new SKPoint(0.5f, 0f), new SKPoint(0.5f, 1f)),
                        GeometryFill   = new SolidColorPaint(SKColor.Parse("#6366F1")),
                        GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                        GeometrySize   = 8,
                        LineSmoothness = 0.5,
                    },
                };

                EjeXVentasDia = new[]
                {
                    new Axis
                    {
                        Labels      = labels,
                        LabelsPaint = new SolidColorPaint(SKColor.Parse("#9CA3AF")),
                        SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#374151")),
                        // Mostrar solo cada 5 días para no saturar
                        LabelsRotation = 0,
                        ForceStepToMin = true,
                        MinStep = 1,
                    }
                };

                // ── Gráfico 2: stock crítico — actual vs mínimo ───────────────
                if (criticos.Any())
                {
                    var top8     = criticos.Take(8).ToList();
                    var nombres  = top8.Select(p => p.Nombre.Length > 12
                        ? p.Nombre[..12] + "…"
                        : p.Nombre).ToArray();
                    var actuales = top8.Select(p => (double)p.StockActual).ToArray();
                    var minimos  = top8.Select(p => (double)p.StockMinimo).ToArray();

                    SeriesStock = new ISeries[]
                    {
                        new ColumnSeries<double>
                        {
                            Name        = "Stock actual",
                            Values      = actuales,
                            Fill        = new SolidColorPaint(SKColor.Parse("#F59E0B")),
                            MaxBarWidth = 22,
                        },
                        new ColumnSeries<double>
                        {
                            Name        = "Stock mínimo",
                            Values      = minimos,
                            Fill        = new SolidColorPaint(SKColor.Parse("#EF444480")),
                            MaxBarWidth = 22,
                        },
                    };

                    EjeXStock = new[]
                    {
                        new Axis
                        {
                            Labels          = nombres,
                            LabelsPaint     = new SolidColorPaint(SKColor.Parse("#9CA3AF")),
                            SeparatorsPaint = new SolidColorPaint(SKColors.Transparent),
                            LabelsRotation  = -30,
                        }
                    };
                }

                NotifyOfPropertyChange(() => StockCritico);
                NotifyOfPropertyChange(() => ComprasRecientes);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Actualizar() => await CargarAsync();
    }
}
