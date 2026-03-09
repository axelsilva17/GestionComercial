using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteGerenciaViewModel : NavigableViewModel
    {
        private readonly IVentaServicio  _ventaServicio;
        private readonly ICompraServicio _compraServicio;
        private readonly SesionServicio  _sesion;

        private ShellViewModel Shell => IoC.Get<ShellViewModel>();

        public ReporteGerenciaViewModel(
            IVentaServicio  ventaServicio,
            ICompraServicio compraServicio,
            SesionServicio  sesion)
        {
            _ventaServicio  = ventaServicio;
            _compraServicio = compraServicio;
            _sesion         = sesion;
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

        private int _anioSeleccionado = DateTime.Now.Year;
        public int AnioSeleccionado
        {
            get => _anioSeleccionado;
            set { _anioSeleccionado = value; NotifyOfPropertyChange(() => AnioSeleccionado); }
        }

        // ── Tabla mensual ─────────────────────────────────────────────────────
        public ObservableCollection<ReporteVentaMensualDto> VentasMensuales { get; set; } = new();
        public ObservableCollection<ReporteProductoTopDto>  TopProductos    { get; set; } = new();
        public ObservableCollection<ReporteVendedorDto>     Vendedores      { get; set; } = new();

        // ── Gráfico 1: Barras Ventas vs Compras ───────────────────────────────
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

        // ── Gráfico 2: Línea evolución ventas ─────────────────────────────────
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

        // ── Gráfico 3: Torta métodos de pago ─────────────────────────────────
        private ISeries[] _seriesTorta = Array.Empty<ISeries>();
        public ISeries[] SeriesTorta
        {
            get => _seriesTorta;
            set { _seriesTorta = value; NotifyOfPropertyChange(() => SeriesTorta); }
        }

        // Eje Y compartido (tema oscuro)
        public Axis[] EjeY { get; } = new[]
        {
            new Axis
            {
                LabelsPaint = new SolidColorPaint(SKColor.Parse("#9CA3AF")),
                SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#374151")),
                Labeler = value => $"${value / 1000:N0}K",
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
                var desde = new DateTime(AnioSeleccionado, 1, 1);
                var hasta = AnioSeleccionado == DateTime.Now.Year
                    ? DateTime.Today
                    : new DateTime(AnioSeleccionado, 12, 31);

                var todasVentas = (await _ventaServicio.ObtenerPorSucursalAsync(
                    _sesion.IdSucursal, desde, hasta)).ToList();

                VentasAcumuladas = todasVentas.Sum(v => v.TotalFinal);

                var todasCompras = (await _compraServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal))
                    .Where(c => c.Fecha >= desde && c.Fecha <= hasta).ToList();
                ComprasAcumuladas = todasCompras.Sum(c => c.Total);

                MargenPromedio = VentasAcumuladas > 0
                    ? (double)(ResultadoAcumulado / VentasAcumuladas * 100)
                    : 0;

                // Últimos 6 meses
                var meses = Enumerable.Range(0, 6)
                    .Select(i =>
                    {
                        var mes    = DateTime.Today.AddMonths(-5 + i);
                        var inicio = new DateTime(mes.Year, mes.Month, 1);
                        var fin    = inicio.AddMonths(1).AddDays(-1);
                        var vMes   = todasVentas.Where(v => v.Fecha >= inicio && v.Fecha <= fin).ToList();
                        var cMes   = todasCompras.Where(c => c.Fecha >= inicio && c.Fecha <= fin).ToList();
                        var ventas  = vMes.Sum(v => v.TotalFinal);
                        var compras = cMes.Sum(c => c.Total);
                        return new ReporteVentaMensualDto
                        {
                            Mes       = mes.ToString("MMM yyyy"),
                            Ventas    = ventas,
                            Compras   = compras,
                            Resultado = ventas - compras,
                            Margen    = ventas > 0 ? (double)((ventas - compras) / ventas * 100) : 0,
                        };
                    }).ToList();

                VentasMensuales = new ObservableCollection<ReporteVentaMensualDto>(meses);

                // ── Gráfico barras: Ventas vs Compras ─────────────────────────
                var labels   = meses.Select(m => m.Mes).ToArray();
                var venArray = meses.Select(m => (double)m.Ventas).ToArray();
                var comArray = meses.Select(m => (double)m.Compras).ToArray();

                SeriesBarras = new ISeries[]
                {
                    new ColumnSeries<double>
                    {
                        Name   = "Ventas",
                        Values = venArray,
                        Fill   = new SolidColorPaint(SKColor.Parse("#10B981")),
                        MaxBarWidth = 28,
                    },
                    new ColumnSeries<double>
                    {
                        Name   = "Compras",
                        Values = comArray,
                        Fill   = new SolidColorPaint(SKColor.Parse("#EF4444")),
                        MaxBarWidth = 28,
                    },
                };

                EjeXBarras = new[]
                {
                    new Axis
                    {
                        Labels      = labels,
                        LabelsPaint = new SolidColorPaint(SKColor.Parse("#9CA3AF")),
                        SeparatorsPaint = new SolidColorPaint(SKColors.Transparent),
                    }
                };

                // ── Gráfico línea: evolución ventas ───────────────────────────
                SeriesLinea = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Name   = "Ventas",
                        Values = venArray,
                        Stroke = new SolidColorPaint(SKColor.Parse("#6366F1")) { StrokeThickness = 3 },
                        Fill   = new LinearGradientPaint(
                            new[] { SKColor.Parse("#6366F180"), SKColor.Parse("#6366F100") },
                            new SKPoint(0.5f, 0f), new SKPoint(0.5f, 1f)),
                        GeometryFill   = new SolidColorPaint(SKColor.Parse("#6366F1")),
                        GeometryStroke = new SolidColorPaint(SKColor.Parse("#FFFFFF")) { StrokeThickness = 2 },
                        GeometrySize   = 10,
                    },
                };

                EjeXLinea = new[]
                {
                    new Axis
                    {
                        Labels      = labels,
                        LabelsPaint = new SolidColorPaint(SKColor.Parse("#9CA3AF")),
                        SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#374151")),
                    }
                };

                // ── Gráfico torta: métodos de pago (agrupado por nombre) ──────
                // Aproximado desde ventas — distribuir proporcionalmente por terceras partes
                // En producción esto vendría de un join con Pagos
                var totalV = (double)VentasAcumuladas;
                SeriesTorta = new ISeries[]
                {
                    new PieSeries<double> { Name = "Efectivo",      Values = new[] { totalV * 0.35 }, Fill = new SolidColorPaint(SKColor.Parse("#10B981")) },
                    new PieSeries<double> { Name = "Débito",        Values = new[] { totalV * 0.25 }, Fill = new SolidColorPaint(SKColor.Parse("#6366F1")) },
                    new PieSeries<double> { Name = "Crédito",       Values = new[] { totalV * 0.20 }, Fill = new SolidColorPaint(SKColor.Parse("#F59E0B")) },
                    new PieSeries<double> { Name = "Transferencia", Values = new[] { totalV * 0.12 }, Fill = new SolidColorPaint(SKColor.Parse("#3B82F6")) },
                    new PieSeries<double> { Name = "Cuenta cte.",   Values = new[] { totalV * 0.08 }, Fill = new SolidColorPaint(SKColor.Parse("#EC4899")) },
                };

                NotifyOfPropertyChange(() => VentasMensuales);
                NotifyOfPropertyChange(() => TopProductos);
                NotifyOfPropertyChange(() => Vendedores);
                NotifyOfPropertyChange(() => ResultadoAcumulado);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Actualizar() => await CargarAsync();
    }
}
