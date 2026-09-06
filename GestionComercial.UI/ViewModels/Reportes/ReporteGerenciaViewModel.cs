using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Compras;
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

        // Permiso para mostrar botón Caja Auditoría
        public bool PuedeVerCajaAuditoria => _sesion.HasPermission("Caja.Auditoria");

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
            var sw = Stopwatch.StartNew();
            
            try
            {
                var desde = FechaDesde;
                var hasta = FechaHasta;
                // Normalizar hasta al fin del día para incluir todo el día actual
                if (hasta.TimeOfDay == TimeSpan.Zero)
                    hasta = hasta.Date.AddDays(1).AddSeconds(-1);

                LogHelper.Log($"[ReporteGerencia] Filtro: desde={desde:yyyy-MM-dd HH:mm} hasta={hasta:yyyy-MM-dd HH:mm}");

                // ── Consultas independientes en paralelo (Task.WhenAll) ──────────
                // KPIs via SQL aggregation (IReporteServicio)
                var kpisTask = _reporteServicio.KpisVentasBaseAsync(_sesion.IdEmpresa, _sesion.IdSucursal, desde, hasta);
                // Compras metrics via ICompraServicio (new SQL aggregation)
                var metricasComprasTask = _compraServicio.ObtenerMetricasComprasAsync(_sesion.IdSucursal, desde, hasta);
                // Ventas por día (SQL aggregation) para gráfico mensual
                var ventasPorDiaTask = _reporteServicio.VentasPorDiaAsync(_sesion.IdEmpresa, desde, hasta);
                // Torta: métodos de pago reales (IReporteServicio)
                var metodosPagoTask = _reporteServicio.MetodosPagoUtilizadosAsync(_sesion.IdSucursal, desde, hasta);
                await Task.WhenAll(kpisTask, metricasComprasTask, ventasPorDiaTask, metodosPagoTask);
                LogHelper.Log($"[ReporteGerencia] Consultas paralelas: {sw.ElapsedMilliseconds}ms");

                // ── KPIs ─────────────────────────────────────────────────────────
                var kpis = await kpisTask;
                if (kpis != null)
                {
                    VentasAcumuladas = kpis.TotalVentasPeriodo;
                }
                LogHelper.Log($"[ReporteGerencia] KPIs procesados");

                // Procesar métricas compras
                var metricasCompras = await metricasComprasTask;
                if (metricasCompras != null)
                {
                    ComprasAcumuladas = metricasCompras.Total;
                }
                LogHelper.Log($"[ReporteGerencia] Métricas compras procesadas");

                MargenPromedio = VentasAcumuladas > 0
                    ? (double)(ResultadoAcumulado / VentasAcumuladas * 100) : 0;

                // ── Ventas por día (SQL aggregation) para gráfico mensual ───────────
                var ventasPorDia = await ventasPorDiaTask;
                var meses = GenerarMesesDesdeVentasPorDia(desde, hasta, ventasPorDia, metricasCompras);
                VentasMensuales = new ObservableCollection<ReporteVentaMensualDto>(meses);
                LogHelper.Log($"[ReporteGerencia] Meses calculados: {meses.Count}");

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

                // ── Torta: métodos de pago reales (IReporteServicio) ─────────────
                var metodosPago = await metodosPagoTask;
                LogHelper.Log($"[ReporteGerencia] Métodos pago: {metodosPago.Count()}");

                SeriesTorta = metodosPago.Any()
                    ? metodosPago.Select((item, i) =>
                        (ISeries)new PieSeries<double>
                        {
                            Name            = item.Metodo,
                            Values          = new[] { (double)item.Total },
                            Fill            = new SolidColorPaint(_coloresTorta[i % _coloresTorta.Length]),
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

        private static List<ReporteVentaMensualDto> GenerarMesesDesdeVentasPorDia(
            DateTime desde, DateTime hasta,
            IEnumerable<VentaPorDiaDto> ventasPorDia,
            MetricasComprasDto? metricasCompras)
        {
            var result = new List<ReporteVentaMensualDto>();
            var cursor = new DateTime(desde.Year, desde.Month, 1);
            var fin    = new DateTime(hasta.Year, hasta.Month, 1);

            // Agrupar compras por mes (aproximado - distribuir total proporcionalmente)
            // Nota: IReporteServicio no tiene compras por día, usamos métrica total
            decimal totalCompras = metricasCompras?.Total ?? 0;
            int mesesEnRango = 0;
            var tempCursor = new DateTime(desde.Year, desde.Month, 1);
            while (tempCursor <= fin)
            {
                mesesEnRango++;
                tempCursor = tempCursor.AddMonths(1);
            }
            decimal comprasPorMes = mesesEnRango > 0 ? totalCompras / mesesEnRango : 0;

            while (cursor <= fin)
            {
                var inicio = cursor;
                var finMes = cursor.AddMonths(1).AddDays(-1);
                var v = ventasPorDia
                    .Where(x => DateTime.TryParseExact(x.Dia, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dt) 
                             && dt >= inicio && dt <= finMes)
                    .Sum(x => x.Total);
                var c = comprasPorMes;
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

        // True while the workbook build + SaveAs run on a background task:
        // drives the "Exportando..." busy overlay and disables the export button.
        private bool _isExportando;
        public bool IsExportando
        {
            get => _isExportando;
            set
            {
                _isExportando = value;
                NotifyOfPropertyChange(() => IsExportando);
                NotifyOfPropertyChange(() => CanExportarExcel);
            }
        }

        public bool CanExportarExcel => !IsExportando;

        public async Task ExportarExcel()
        {
            try
            {
                IsLoading = true;
                var swExport = Stopwatch.StartNew();
                var desde = FechaDesde;
                var hasta = FechaHasta;

                LogHelper.Log($"[ReporteGerencia] Iniciando exportación: {desde:dd/MM} - {hasta:dd/MM}");

                // Las 7 consultas se disparan en paralelo (antes se ejecutaban una tras otra,
                // sumando sus tiempos de forma secuencial).
                var tareaVentaPorDia = _reporteServicio.VentasPorDiaAsync(_sesion.IdEmpresa, desde, hasta);
                var tareaMargen = _reporteServicio.MargenPorProductoAsync(_sesion.IdEmpresa, desde, hasta);
                var tareaTopProductos = _reporteServicio.TopProductosAsync(_sesion.IdSucursal, desde, hasta, 20);
                var tareaVendedores = _reporteServicio.VentasPorVendedorAsync(_sesion.IdSucursal, desde, hasta);
                var tareaRotacion = _reporteServicio.RotacionProductosAsync(_sesion.IdEmpresa, desde, hasta);
                var tareaMetodosPago = _reporteServicio.MetodosPagoUtilizadosAsync(_sesion.IdSucursal, desde, hasta);
                var tareaMetodosMensuales = _reporteServicio.MetodosPagoMensualAsync(_sesion.IdSucursal, desde, hasta);

                await Task.WhenAll(tareaVentaPorDia, tareaMargen, tareaTopProductos, tareaVendedores,
                                   tareaRotacion, tareaMetodosPago, tareaMetodosMensuales);

                // Materializar cada dataset (la duración se loguea por dataset)
                // VentaPorDia: usar IReporteServicio
                swExport.Restart();
                var ventaPorDia = (await tareaVentaPorDia).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: VentaPorDia ({ventaPorDia.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Margen: usar IReporteServicio.MargenPorProductoAsync
                swExport.Restart();
                var margen = (await tareaMargen).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Margen ({margen.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Top Productos
                swExport.Restart();
                var topProductos = (await tareaTopProductos).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Top productos ({topProductos.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Ventas por Vendedor
                swExport.Restart();
                var vendedores = (await tareaVendedores).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Vendedores ({vendedores.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Rotación de Productos
                swExport.Restart();
                var rotacion = (await tareaRotacion).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Rotación ({rotacion.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Métodos de Pago
                swExport.Restart();
                var metodosPago = (await tareaMetodosPago).ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Métodos pago ({metodosPago.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Distribución mensual de métodos de pago (una sola consulta SQL agrupada)
                swExport.Restart();
                var metodosMensuales = (await tareaMetodosMensuales)
                    .Select(m => new MetodosPagoMesDto
                    {
                        Mes = m.Mes.ToString("MMM yy"),
                        Metodo = m.Metodo,
                        Total = m.Total,
                        Cantidad = m.Cantidad
                    })
                    .ToList();
                LogHelper.Log($"[ReporteGerencia] Export: Métodos pago mensuales ({metodosMensuales.Count}) en {swExport.ElapsedMilliseconds}ms");

                // Capturar valores de propiedades antes del export (la tarea en
                // background no puede leer colecciones bound al hilo UI)
                var ventasMensuales = VentasMensuales.ToList();
                var ventasAcum = VentasAcumuladas;
                var comprasAcum = ComprasAcumuladas;
                var resultadoNeto = ResultadoAcumulado;
                var margenProm = MargenPromedio;

                // Exportar: el SaveFileDialog corre en el hilo UI (dentro del helper) y
                // ANTES de la tarea en background; el build de hojas + SaveAs (~12.9s)
                // corren fuera del hilo UI con el overlay "Exportando..." visible.
                swExport.Restart();
                IsExportando = true;
                try
                {
                    await ExportHelper.ExportarReporteGerenciaCompleto(
                        ventaPorDia, margen, topProductos, vendedores, rotacion, metodosPago, desde, hasta,
                        ventasAcumuladas: ventasAcum,
                        comprasAcumuladas: comprasAcum,
                        resultadoNeto: resultadoNeto,
                        margenPromedio: margenProm,
                        ventasMensuales: ventasMensuales,
                        metodosPagoMensual: metodosMensuales);
                }
                finally { IsExportando = false; }
                LogHelper.Log($"[ReporteGerencia] ✓ Exportación completada en {swExport.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    System.Windows.MessageBox.Show($"Error al exportar: {ex.Message}",
                        "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                });
            }
            finally { IsLoading = false; }
        }

        // ── Navegar a Caja Auditoría ───────────────────────────────────────────
        public async Task IrCajaAuditoria()
        {
            var vm = Caliburn.Micro.IoC.Get<CajaAuditoriaViewModel>();
            vm.VolverAGerencia = true;
            await Shell.ActivateItemAsync(vm, CancellationToken.None);
        }
    }
}