using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.Helpers;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteGerenciaViewModel : NavigableViewModel
    {
        private readonly IVentaServicio   _ventaServicio;
        private readonly ICompraServicio  _compraServicio;
        private readonly IReporteServicio  _reporteServicio;
        private readonly IUnitOfWork      _uow;
        private readonly SesionServicio   _sesion;

        private ShellViewModel Shell => IoC.Get<ShellViewModel>();

        // Paleta alineada al sistema
        private static readonly SKColor Col_Primary  = SKColor.Parse("#38BDF8");
        private static readonly SKColor Col_Success  = SKColor.Parse("#10B981");
        private static readonly SKColor Col_Warning  = SKColor.Parse("#F59E0B");
        private static readonly SKColor Col_Error    = SKColor.Parse("#EF4444");
        private static readonly SKColor Col_Info     = SKColor.Parse("#3B82F6");
        private static readonly SKColor Col_Positive = SKColor.Parse("#4ADE80");
        private static readonly SKColor Col_Neutral  = SKColor.Parse("#94A3B8");
        private static readonly SKColor Col_Text     = SKColor.Parse("#E0F2FE");
        private static readonly SKColor Col_TextSec  = SKColor.Parse("#64748B");
        private static readonly SKColor Col_Sep      = SKColor.Parse("#2A3D52");

        private static readonly SKColor[] _coloresTorta =
        {
            Col_Primary, Col_Success, Col_Warning, Col_Info, Col_Positive, Col_Neutral, Col_Error,
        };

        public ReporteGerenciaViewModel(
            IVentaServicio   ventaServicio,
            ICompraServicio  compraServicio,
            IReporteServicio reporteServicio,
            IUnitOfWork      uow,
            SesionServicio   sesion)
        {
            _ventaServicio   = ventaServicio;
            _compraServicio  = compraServicio;
            _reporteServicio = reporteServicio;
            _uow             = uow;
            _sesion          = sesion;
            Titulo          = "Reportes";
            Subtitulo       = "Gerencia — visión ejecutiva";
        }

        // ── KPIs ──────────────────────────────────────────────────────────────
        private decimal _ventasAcumuladas;
        public decimal VentasAcumuladas
        {
            get => _ventasAcumuladas;
            set { _ventasAcumuladas = value; NotifyOfPropertyChange(() => VentasAcumuladas); NotifyOfPropertyChange(() => ResultadoAcumulado); }
        }

        private decimal _comprasAcumuladas;
        public decimal ComprasAcumuladas
        {
            get => _comprasAcumuladas;
            set { _comprasAcumuladas = value; NotifyOfPropertyChange(() => ComprasAcumuladas); NotifyOfPropertyChange(() => ResultadoAcumulado); }
        }

        private double _margenPromedio;
        public double MargenPromedio
        {
            get => _margenPromedio;
            set { _margenPromedio = value; NotifyOfPropertyChange(() => MargenPromedio); }
        }

        private int _clientesNuevos;
        public int ClientesNuevos
        {
            get => _clientesNuevos;
            set { _clientesNuevos = value; NotifyOfPropertyChange(() => ClientesNuevos); }
        }

        public decimal ResultadoAcumulado => VentasAcumuladas - ComprasAcumuladas;

        // ── Filtros de fecha ──────────────────────────────────────────────────
        private DateTime _fechaDesde = new DateTime(DateTime.Today.Year, 1, 1);
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

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<ReporteVentaMensualDto> VentasMensuales { get; set; } = new();
        public ObservableCollection<ReporteProductoTopDto>  TopProductos    { get; set; } = new();
        public ObservableCollection<ReporteVendedorDto>     Vendedores      { get; set; } = new();
        public List<MetodosPagoMesDto> MetodosMensuales { get; set; } = new();

        // ── Gráfico 1: Barras ─────────────────────────────────────────────────
        private ISeries[] _seriesBarras = Array.Empty<ISeries>();
        public ISeries[] SeriesBarras
        {
            get => _seriesBarras;
            set { _seriesBarras = value; NotifyOfPropertyChange(() => SeriesBarras); }
        }

        private Axis[] _ejeXBarras = Array.Empty<Axis>();
        public Axis[] EjeXBarras
        {
            get => _ejeXBarras;
            set { _ejeXBarras = value; NotifyOfPropertyChange(() => EjeXBarras); }
        }

        // ── Gráfico 2: Línea ──────────────────────────────────────────────────
        private ISeries[] _seriesLinea = Array.Empty<ISeries>();
        public ISeries[] SeriesLinea
        {
            get => _seriesLinea;
            set { _seriesLinea = value; NotifyOfPropertyChange(() => SeriesLinea); }
        }

        private Axis[] _ejeXLinea = Array.Empty<Axis>();
        public Axis[] EjeXLinea
        {
            get => _ejeXLinea;
            set { _ejeXLinea = value; NotifyOfPropertyChange(() => EjeXLinea); }
        }

        // ── Gráfico 3: Torta ──────────────────────────────────────────────────
        private ISeries[] _seriesTorta = Array.Empty<ISeries>();
        public ISeries[] SeriesTorta
        {
            get => _seriesTorta;
            set { _seriesTorta = value; NotifyOfPropertyChange(() => SeriesTorta); }
        }

        // Ejes compartidos con paleta del sistema
        public Axis[] EjeY { get; } = new[]
        {
            new Axis
            {
                LabelsPaint     = new SolidColorPaint(SKColor.Parse("#64748B")),
                SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#2A3D52")),
                Labeler         = v => $"${v / 1000:N0}K",
            }
        };

        // Leyenda de la torta con color del sistema
        public LiveChartsCore.Measure.LegendPosition PosicionLeyenda =>
            LiveChartsCore.Measure.LegendPosition.Right;

        public SolidColorPaint LeyendaTextoPaint { get; } =
            new SolidColorPaint(SKColor.Parse("#E0F2FE")) { SKTypeface = SKTypeface.FromFamilyName("Segoe UI") };

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            
            var swTotal = Stopwatch.StartNew();
            var sw = new Stopwatch();
            
            try
            {
                var desde = FechaDesde;
                var hasta = FechaHasta;

                LogHelper.Log($"[ReporteGerencia] Filtro: desde={desde:yyyy-MM-dd} hasta={hasta:yyyy-MM-dd}");

                // Ventas del período
                sw.Restart();
                var todasVentas = (await _ventaServicio.ObtenerPorSucursalAsync(
                    _sesion.IdSucursal, desde, hasta)).ToList();
                VentasAcumuladas = todasVentas.Sum(v => v.TotalFinal);
                LogHelper.Log($"[ReporteGerencia] Ventas: {todasVentas.Count} registros en {sw.ElapsedMilliseconds}ms");

                // Compras del período
                sw.Restart();
                var todasCompras = (await _compraServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal))
                    .Where(c => c.Fecha >= desde && c.Fecha <= hasta).ToList();
                ComprasAcumuladas = todasCompras.Sum(c => c.Total);
                LogHelper.Log($"[ReporteGerencia] Compras: {todasCompras.Count} registros en {sw.ElapsedMilliseconds}ms");

                MargenPromedio = VentasAcumuladas > 0
                    ? (double)(ResultadoAcumulado / VentasAcumuladas * 100) : 0;

                // Calcular meses dentro del rango (máx 12)
                sw.Restart();
                var meses = GenerarMeses(desde, hasta, todasVentas, todasCompras);
                VentasMensuales = new ObservableCollection<ReporteVentaMensualDto>(meses);
                LogHelper.Log($"[ReporteGerencia] Meses calculados: {meses.Count} en {sw.ElapsedMilliseconds}ms");

                var labels   = meses.Select(m => m.Mes).ToArray();
                var venArray = meses.Select(m => (double)m.Ventas).ToArray();
                var comArray = meses.Select(m => (double)m.Compras).ToArray();

                // ── Barras ────────────────────────────────────────────────────
                SeriesBarras = new ISeries[]
                {
                    new ColumnSeries<double>
                    {
                        Name        = "Ventas",
                        Values      = venArray,
                        Fill        = new SolidColorPaint(Col_Success),
                        MaxBarWidth = 26,
                    },
                    new ColumnSeries<double>
                    {
                        Name        = "Compras",
                        Values      = comArray,
                        Fill        = new SolidColorPaint(Col_Error),
                        MaxBarWidth = 26,
                    },
                };
                EjeXBarras = new[]
                {
                    new Axis
                    {
                        Labels          = labels,
                        LabelsPaint     = new SolidColorPaint(Col_TextSec),
                        SeparatorsPaint = new SolidColorPaint(SKColors.Transparent),
                    }
                };

                // ── Línea ─────────────────────────────────────────────────────
                SeriesLinea = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name           = "Ventas",
                        Values         = venArray,
                        Stroke         = new SolidColorPaint(Col_Primary) { StrokeThickness = 3 },
                        Fill           = new LinearGradientPaint(
                            new[] { new SKColor(56, 189, 248, 100), new SKColor(56, 189, 248, 0) },
                            new SKPoint(0.5f, 0f), new SKPoint(0.5f, 1f)),
                        GeometryFill   = new SolidColorPaint(Col_Primary),
                        GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                        GeometrySize   = 10,
                    },
                };
                EjeXLinea = new[]
                {
                    new Axis
                    {
                        Labels          = labels,
                        LabelsPaint     = new SolidColorPaint(Col_TextSec),
                        SeparatorsPaint = new SolidColorPaint(Col_Sep),
                    }
                };

                // ── Torta: métodos de pago reales ─────────────────────────────
                sw.Restart();
                var pagosAgrupados = (await _uow.Pagos.ObtenerTotalesPorMetodoAsync(
                    _sesion.IdSucursal, desde, hasta)).ToList();
                LogHelper.Log($"[ReporteGerencia] Métodos pago: {pagosAgrupados.Count} en {sw.ElapsedMilliseconds}ms");

                SeriesTorta = pagosAgrupados.Any()
                    ? pagosAgrupados.Select((item, i) =>
                        (ISeries)new PieSeries<double>
                        {
                            Name            = item.Metodo,
                            Values          = new[] { (double)item.Total },
                            Fill            = new SolidColorPaint(_coloresTorta[i % _coloresTorta.Length]),
                            // Etiqueta dentro de la porción
                            DataLabelsPaint = new SolidColorPaint(SKColors.White),
                            DataLabelsSize  = 11,
                            DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                            DataLabelsFormatter = p => $"{p.StackedValue!.Share:P0}",
                        }).ToArray()
                    : new ISeries[]
                    {
                        new PieSeries<double>
                        {
                            Name   = "Sin datos",
                            Values = new[] { 1.0 },
                            Fill   = new SolidColorPaint(Col_Sep),
                        }
                    };

                NotifyOfPropertyChange(() => VentasMensuales);
                NotifyOfPropertyChange(() => TopProductos);
                NotifyOfPropertyChange(() => Vendedores);
                NotifyOfPropertyChange(() => ResultadoAcumulado);
                
                LogHelper.Log($"[ReporteGerencia] ✓ Carga total: {swTotal.ElapsedMilliseconds}ms");
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        private static List<ReporteVentaMensualDto> GenerarMeses(
            DateTime desde, DateTime hasta,
            List<GestionComercial.Aplicacion.DTOs.Ventas.VentaResumenDto> ventas,
            List<GestionComercial.Aplicacion.DTOs.Compras.CompraDto> compras)
        {
            var result = new List<ReporteVentaMensualDto>();
            var cursor = new DateTime(desde.Year, desde.Month, 1);
            var fin    = new DateTime(hasta.Year, hasta.Month, 1);

            while (cursor <= fin)
            {
                var inicio = cursor;
                var finMes = cursor.AddMonths(1).AddDays(-1);
                var v = ventas.Where(x => x.Fecha >= inicio && x.Fecha <= finMes).Sum(x => x.TotalFinal);
                var c = compras.Where(x => x.Fecha >= inicio && x.Fecha <= finMes).Sum(x => x.Total);
                result.Add(new ReporteVentaMensualDto
                {
                    Mes       = cursor.ToString("MMM yy"),
                    Ventas    = v,
                    Compras   = c,
                    Resultado = v - c,
                    Margen    = v > 0 ? (double)((v - c) / v * 100) : 0,
                });
                cursor = cursor.AddMonths(1);
            }
            return result;
        }

        // ── Acciones filtro ───────────────────────────────────────────────────
        public async Task Actualizar()       => await CargarAsync();
        public async Task FiltrarEsteAnio()  { FechaDesde = new DateTime(DateTime.Today.Year, 1, 1);  FechaHasta = DateTime.Today; await CargarAsync(); }
        public async Task FiltrarUltimos6()  { FechaDesde = DateTime.Today.AddMonths(-6);             FechaHasta = DateTime.Today; await CargarAsync(); }
        public async Task FiltrarUltimos3()  { FechaDesde = DateTime.Today.AddMonths(-3);             FechaHasta = DateTime.Today; await CargarAsync(); }
        public async Task FiltrarEsteMes()   { FechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); FechaHasta = DateTime.Today; await CargarAsync(); }

        // ── Exportar Excel ───────────────────────────────────────────────────
        public async Task ExportarExcel()
        {
            try
            {
                IsLoading = true;
                var swExport = Stopwatch.StartNew();
                var desde = FechaDesde;
                var hasta = FechaHasta;

                LogHelper.Log($"[ReporteGerencia] Iniciando exportación: {desde:dd/MM} - {hasta:dd/MM}");

                // VentaPorDia: agrupar las ventas ya cargadas en memoria
                swExport.Restart();
                var ventas = (await _ventaServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal, desde, hasta)).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Ventas cargadas ({ventas.Count}) en {swExport.ElapsedMilliseconds}ms");
                
                var ventaPorDia = ventas
                    .GroupBy(v => v.Fecha.Date)
                    .Select(g => new VentaPorDiaDto
                    {
                        Dia      = g.Key.ToString("dd/MM/yyyy"),
                        Total    = g.Sum(v => v.TotalFinal),
                        Cantidad = g.Count(),
                    })
                    .OrderBy(d => DateTime.ParseExact(d.Dia, "dd/MM/yyyy", null))
                    .ToList();

                // Margen: usar IReporteServicio.MargenPorProductoAsync
                swExport.Restart();
                var margen = (await _reporteServicio.MargenPorProductoAsync(
                    _sesion.IdEmpresa, desde, hasta)).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Margen ({margen.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Top Productos
                swExport.Restart();
                var topProductos = (await _reporteServicio.TopProductosAsync(
                    _sesion.IdSucursal, desde, hasta, 20)).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Top productos ({topProductos.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Ventas por Vendedor
                swExport.Restart();
                var vendedores = (await _reporteServicio.VentasPorVendedorAsync(
                    _sesion.IdSucursal, desde, hasta)).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Vendedores ({vendedores.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Rotación de Productos
                swExport.Restart();
                var rotacion = (await _reporteServicio.RotacionProductosAsync(
                    _sesion.IdEmpresa, desde, hasta)).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Rotación ({rotacion.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Métodos de Pago
                swExport.Restart();
                var metodosPago = (await _reporteServicio.MetodosPagoUtilizadosAsync(
                    _sesion.IdSucursal, desde, hasta)).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Métodos pago ({metodosPago.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Distribución mensual de métodos de pago
                swExport.Restart();
                var metodosMensuales = new List<MetodosPagoMesDto>();
                var cursor = new DateTime(desde.Year, desde.Month, 1);
                var fin = new DateTime(hasta.Year, hasta.Month, 1);
                while (cursor <= fin)
                {
                    var inicioMes = cursor;
                    var finMes = cursor.AddMonths(1).AddDays(-1);
                    var metodosDelMes = await _reporteServicio.MetodosPagoUtilizadosAsync(
                        _sesion.IdSucursal, inicioMes, finMes);
                    foreach (var m in metodosDelMes)
                    {
                        metodosMensuales.Add(new MetodosPagoMesDto
                        {
                            Mes = cursor.ToString("MMM yy"),
                            Metodo = m.Metodo,
                            Total = m.Total,
                            Cantidad = m.Cantidad
                        });
                    }
                    cursor = cursor.AddMonths(1);
                }
                LogHelper.Log($"[ReporteGerencia] Export: Métodos pago mensuales ({metodosMensuales.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Ventas mensuales (ya calculadas en CargarAsync)
                var ventasMensuales = VentasMensuales.ToList();

                // Exportar todo a un solo archivo con múltiples hojas (evita dialogos duplicados)
                swExport.Restart();
                ExportHelper.ExportarReporteGerenciaCompleto(
                    ventaPorDia, margen, topProductos, vendedores, rotacion, metodosPago, desde, hasta,
                    ventasAcumuladas: VentasAcumuladas,
                    comprasAcumuladas: ComprasAcumuladas,
                    resultadoNeto: ResultadoAcumulado,
                    margenPromedio: MargenPromedio,
                    ventasMensuales: ventasMensuales,
                    metodosPagoMensual: metodosMensuales);
                LogHelper.Log($"[ReporteGerencia] ✓ Exportación completada en {swExport.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar: {ex.Message}",
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }
    }
}
