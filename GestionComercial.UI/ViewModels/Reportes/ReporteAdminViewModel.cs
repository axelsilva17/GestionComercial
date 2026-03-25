using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.Helpers;
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

    public class MovimientoCajaHistorialDto
    {
        public string  TipoOperacion { get; set; } = string.Empty;
        public string  Descripcion   { get; set; } = string.Empty;
        public decimal Monto          { get; set; }
        public string  Fecha          { get; set; } = string.Empty;
        public string  Usuario        { get; set; } = string.Empty;
        public string  Icono          { get; set; } = string.Empty;
        public bool    EsIngreso     { get; set; }
    }

    public class CajaHistorialDto
    {
        public int     Id            { get; set; }
        public string  FechaApertura { get; set; } = string.Empty;
        public string? FechaCierre   { get; set; }
        public decimal MontoInicial  { get; set; }
        public decimal? MontoFinal   { get; set; }
        public decimal? Diferencia    { get; set; }
        public string  TipoDiferencia { get; set; } = "Cero"; // Positivo, Negativo, Cero
        public string  UsuarioApertura { get; set; } = string.Empty;
        public string? UsuarioCierre  { get; set; }
        public string  Estado         { get; set; } = string.Empty;
        public bool    TieneDiferencia { get; set; }
    }

    public class ReporteAdminViewModel : NavigableViewModel
    {
        private readonly IVentaServicio    _ventaServicio;
        private readonly IProductoServicio _productoServicio;
        private readonly ICompraServicio   _compraServicio;
        private readonly ICajaServicio     _cajaServicio;
        private readonly IUnitOfWork      _uow;
        private readonly SesionServicio    _sesion;
        private readonly IAuditoriaServicio _auditoriaServicio;

        // Paleta del sistema
        private static readonly SKColor Col_Primary = SKColor.Parse("#38BDF8");
        private static readonly SKColor Col_Success = SKColor.Parse("#10B981");
        private static readonly SKColor Col_Warning = SKColor.Parse("#F59E0B");
        private static readonly SKColor Col_Error   = SKColor.Parse("#EF4444");
        private static readonly SKColor Col_TextSec = SKColor.Parse("#64748B");
        private static readonly SKColor Col_Sep     = SKColor.Parse("#2A3D52");

        public ReporteAdminViewModel(
            IVentaServicio    ventaServicio,
            IProductoServicio productoServicio,
            ICompraServicio   compraServicio,
            ICajaServicio     cajaServicio,
            IUnitOfWork      uow,
            SesionServicio    sesion,
            IAuditoriaServicio auditoriaServicio)
        {
            _ventaServicio    = ventaServicio;
            _productoServicio = productoServicio;
            _compraServicio   = compraServicio;
            _cajaServicio     = cajaServicio;
            _uow            = uow;
            _sesion           = sesion;
            _auditoriaServicio = auditoriaServicio;
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

        // ── Panel de Auditoría ────────────────────────────────────────────────
        private bool _mostrarPanelAuditoria;
        public bool MostrarPanelAuditoria
        {
            get => _mostrarPanelAuditoria;
            set { _mostrarPanelAuditoria = value; NotifyOfPropertyChange(() => MostrarPanelAuditoria); }
        }

        // ── Filtros ───────────────────────────────────────────────────────────
        private DateTime _fechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
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

        // ── Opción auto-abrir después de exportar ─────────────────────────────
        private bool _abrirDespuesDeExportar;
        public bool AbrirDespuesDeExportar
        {
            get => _abrirDespuesDeExportar;
            set { _abrirDespuesDeExportar = value; NotifyOfPropertyChange(() => AbrirDespuesDeExportar); }
        }

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<ReporteStockCriticoDto>   StockCritico     { get; set; } = new();
        public ObservableCollection<ReporteCompraRecienteDto> ComprasRecientes { get; set; } = new();
        public ObservableCollection<CajaHistorialDto>        HistorialCajas   { get; set; } = new();
        public ObservableCollection<MovimientoCajaHistorialDto> MovimientosCaja { get; set; } = new();
        public ObservableCollection<AuditoriaLogDto>        AuditoriaCajas       { get; set; } = new();
        public ObservableCollection<AuditoriaLogDto>        AuditoriaMovimientos { get; set; } = new();

        // ── KPIs de caja ───────────────────────────────────────────────────────
        private int _totalCajas;
        public int TotalCajas
        {
            get => _totalCajas;
            set { _totalCajas = value; NotifyOfPropertyChange(() => TotalCajas); }
        }

        private int _cajasCerradas;
        public int CajasCerradas
        {
            get => _cajasCerradas;
            set { _cajasCerradas = value; NotifyOfPropertyChange(() => CajasCerradas); }
        }

        private int _cajasConDiferencia;
        public int CajasConDiferencia
        {
            get => _cajasConDiferencia;
            set { _cajasConDiferencia = value; NotifyOfPropertyChange(() => CajasConDiferencia); }
        }

        private decimal _totalIngresos;
        public decimal TotalIngresos
        {
            get => _totalIngresos;
            set { _totalIngresos = value; NotifyOfPropertyChange(() => TotalIngresos); }
        }

        private decimal _totalEgresos;
        public decimal TotalEgresos
        {
            get => _totalEgresos;
            set { _totalEgresos = value; NotifyOfPropertyChange(() => TotalEgresos); }
        }

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
                LabelsPaint     = new SolidColorPaint(SKColor.Parse("#64748B")),
                SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#2A3D52")),
                MinStep         = 1,
            }
        };

        public Axis[] EjeYMoneda { get; } = new[]
        {
            new Axis
            {
                LabelsPaint     = new SolidColorPaint(SKColor.Parse("#64748B")),
                SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#2A3D52")),
                Labeler         = v => $"${v / 1000:N0}K",
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
                var desde = FechaDesde.Date;
                var hasta = FechaHasta.Date.AddDays(1).AddTicks(-1); // Fin del día seleccionado

                LogHelper.Log($"[ReporteAdmin] Filtro: desde={desde:yyyy-MM-dd HH:mm} hasta={hasta:yyyy-MM-dd HH:mm}");

                // Ventas del período
                var ventasPeriodo = (await _ventaServicio.ObtenerPorSucursalAsync(
                    _sesion.IdSucursal, desde, hasta)).ToList();
                CantidadVentasMes = ventasPeriodo.Count;
                VentasPendientes  = ventasPeriodo.Count(v => v.Estado == "Pendiente");

                // Stock crítico (siempre actual, no depende de fechas)
                var criticos = (await _productoServicio.ObtenerStockCriticoAsync(_sesion.IdEmpresa)).ToList();
                ProductosStockCritico = criticos.Count;
                StockCritico = new ObservableCollection<ReporteStockCriticoDto>(
                    criticos.Take(10).Select(p => new ReporteStockCriticoDto
                    {
                        Nombre      = p.Nombre,
                        StockActual = p.StockActual,
                        StockMinimo = p.StockMinimo,
                    }));

                // Compras del período
                var comprasPeriodo = (await _compraServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal))
                    .Where(c => c.Fecha >= desde && c.Fecha <= hasta).ToList();
                ComprasDelMes = comprasPeriodo.Count;
                ComprasRecientes = new ObservableCollection<ReporteCompraRecienteDto>(
                    comprasPeriodo.OrderByDescending(c => c.Fecha).Take(8).Select(c => new ReporteCompraRecienteDto
                    {
                        Proveedor = c.ProveedorNombre,
                        Fecha     = c.Fecha.ToString("dd/MM/yyyy"),
                        Total     = c.Total,
                        Productos = c.Items?.Count ?? 0,
                    }));

                // Clientes nuevos del período
                var clientesNuevos = await _uow.Clientes.ObtenerPorEmpresaYFechaAsync(
                    _sesion.IdEmpresa, desde, hasta.AddDays(1));
                ClientesNuevos = clientesNuevos.Count();

                // ── Gráfico línea: agrupar por día si rango ≤ 31 días, por mes si más ──
                int diasRango = (hasta - desde).Days + 1;
                if (diasRango <= 62)
                {
                    // Ventas por día
                    var labels = Enumerable.Range(0, diasRango)
                        .Select(d => desde.AddDays(d).ToString("dd/MM"))
                        .ToArray();
                    var valores = Enumerable.Range(0, diasRango).Select(d =>
                    {
                        var dia = desde.AddDays(d).Date;
                        return (double)ventasPeriodo
                            .Where(v => v.Fecha.Date == dia)
                            .Sum(v => v.TotalFinal);
                    }).ToArray();

                    BuildLineaVentas(labels, valores);
                }
                else
                {
                    // Ventas por mes
                    var cursor = new DateTime(desde.Year, desde.Month, 1);
                    var finMes = new DateTime(hasta.Year, hasta.Month, 1);
                    var labelsList  = new System.Collections.Generic.List<string>();
                    var valoresList = new System.Collections.Generic.List<double>();
                    while (cursor <= finMes)
                    {
                        var ini = cursor;
                        var fin = cursor.AddMonths(1).AddDays(-1);
                        labelsList.Add(cursor.ToString("MMM yy"));
                        valoresList.Add((double)ventasPeriodo
                            .Where(v => v.Fecha >= ini && v.Fecha <= fin)
                            .Sum(v => v.TotalFinal));
                        cursor = cursor.AddMonths(1);
                    }
                    BuildLineaVentas(labelsList.ToArray(), valoresList.ToArray());
                }

                // ── Gráfico barras: stock actual vs mínimo ────────────────────
                if (criticos.Any())
                {
                    var top8     = criticos.Take(8).ToList();
                    var nombres  = top8.Select(p => p.Nombre.Length > 12 ? p.Nombre[..12] + "…" : p.Nombre).ToArray();
                    var actuales = top8.Select(p => (double)p.StockActual).ToArray();
                    var minimos  = top8.Select(p => (double)p.StockMinimo).ToArray();

                    SeriesStock = new ISeries[]
                    {
                        new ColumnSeries<double>
                        {
                            Name        = "Stock actual",
                            Values      = actuales,
                            Fill        = new SolidColorPaint(Col_Primary),
                            MaxBarWidth = 22,
                        },
                        new ColumnSeries<double>
                        {
                            Name        = "Stock mínimo",
                            Values      = minimos,
                            Fill        = new SolidColorPaint(new SKColor(239, 68, 68, 100)),
                            MaxBarWidth = 22,
                        },
                    };

                    EjeXStock = new[]
                    {
                        new Axis
                        {
                            Labels          = nombres,
                            LabelsPaint     = new SolidColorPaint(Col_TextSec),
                            SeparatorsPaint = new SolidColorPaint(SKColors.Transparent),
                            LabelsRotation  = -30,
                        }
                    };
                }

                // ── Historial de cajas ────────────────────────────────────────────
                var cajas = (await _cajaServicio.ObtenerHistorialAsync(_sesion.IdSucursal, desde, hasta)).ToList();
                TotalCajas = cajas.Count;
                CajasCerradas = cajas.Count(c => c.Estado == 2);
                
                decimal totalIng = 0, totalEgr = 0;
                var cajasConDif = 0;
                var historialList = new List<CajaHistorialDto>();
                var movimientosList = new List<MovimientoCajaHistorialDto>();

                foreach (var caja in cajas.Take(15))
                {
                    var diff = caja.MontoFinal.HasValue 
                        ? caja.MontoFinal.Value - (caja.MontoInicial + caja.Ventas.Sum(v => v.TotalFinal)) 
                        : (decimal?)null;
                    
                    if (diff.HasValue && Math.Abs(diff.Value) > 0.01m)
                        cajasConDif++;

                    historialList.Add(new CajaHistorialDto
                    {
                        Id               = caja.Id,
                        FechaApertura   = caja.FechaApertura.ToString("dd/MM HH:mm"),
                        FechaCierre     = caja.FechaCierre?.ToString("dd/MM HH:mm"),
                        MontoInicial    = caja.MontoInicial,
                        MontoFinal      = caja.MontoFinal,
                        Diferencia      = diff,
                        TipoDiferencia  = diff.HasValue ? (diff.Value > 0 ? "Positivo" : diff.Value < 0 ? "Negativo" : "Cero") : "Cero",
                        UsuarioApertura = caja.UsuarioApertura?.Nombre ?? "—",
                        UsuarioCierre   = caja.UsuarioCierre?.Nombre,
                        Estado          = caja.Estado == 1 ? "Abierta" : "Cerrada",
                        TieneDiferencia = diff.HasValue && Math.Abs(diff.Value) > 0.01m,
                    });

                    foreach (var mov in caja.Movimientos)
                    {
                        var esIngreso = mov.Tipo == 1;
                        if (esIngreso) totalIng += mov.Monto; else totalEgr += mov.Monto;

                        movimientosList.Add(new MovimientoCajaHistorialDto
                        {
                            TipoOperacion = esIngreso ? "Ingreso" : "Egreso",
                            Descripcion   = mov.Concepto ?? "—",
                            Monto         = mov.Monto,
                            Fecha         = mov.Fecha.ToString("dd/MM HH:mm"),
                            Usuario       = mov.Usuario?.Nombre ?? "—",
                            Icono         = esIngreso ? "↑" : "↓",
                            EsIngreso     = esIngreso,
                        });
                    }
                }

                CajasConDiferencia = cajasConDif;
                TotalIngresos = totalIng;
                TotalEgresos = totalEgr;
                HistorialCajas = new ObservableCollection<CajaHistorialDto>(historialList);
                MovimientosCaja = new ObservableCollection<MovimientoCajaHistorialDto>(
                    movimientosList.OrderByDescending(m => m.Fecha).Take(20));

                // Auditoría de cajas se carga单独的 via CargarAuditoriaAsync()
                // al abrir el panel de auditoría

                NotifyOfPropertyChange(() => StockCritico);
                NotifyOfPropertyChange(() => ComprasRecientes);
                NotifyOfPropertyChange(() => HistorialCajas);
                NotifyOfPropertyChange(() => MovimientosCaja);
                NotifyOfPropertyChange(() => AuditoriaCajas);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void BuildLineaVentas(string[] labels, double[] valores)
        {
            SeriesVentasDia = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name           = "Ventas",
                    Values         = valores,
                    Stroke         = new SolidColorPaint(Col_Primary) { StrokeThickness = 2 },
                    Fill           = new LinearGradientPaint(
                        new[] { new SKColor(56, 189, 248, 80), new SKColor(56, 189, 248, 0) },
                        new SKPoint(0.5f, 0f), new SKPoint(0.5f, 1f)),
                    GeometryFill   = new SolidColorPaint(Col_Primary),
                    GeometryStroke = new SolidColorPaint(SKColors.White) { StrokeThickness = 2 },
                    GeometrySize   = 7,
                    LineSmoothness = 0.4,
                },
            };
            EjeXVentasDia = new[]
            {
                new Axis
                {
                    Labels          = labels,
                    LabelsPaint     = new SolidColorPaint(Col_TextSec),
                    SeparatorsPaint = new SolidColorPaint(Col_Sep),
                }
            };
        }

        // ── Acciones de Auditoría ─────────────────────────────────────────────
        public void MostrarAuditoria()
        {
            MostrarPanelAuditoria = true;
            _ = CargarAuditoriaAsync();
        }

        public void CargarAuditoria()
        {
            _ = CargarAuditoriaAsync();
        }

        public void OcultarAuditoria()
        {
            MostrarPanelAuditoria = false;
        }

        public async Task CargarAuditoriaAsync()
        {
            if (!MostrarPanelAuditoria) return;

            IsLoading = true;
            LimpiarError();
            try
            {
                var desde = FechaDesde.Date;
                var hasta = FechaHasta.Date.AddDays(1).AddTicks(-1);

                var auditoriaCajas = (await _auditoriaServicio.ObtenerAuditoriaCajaAsync(desde, hasta)).ToList();
                var auditoriaMovimientos = (await _auditoriaServicio.ObtenerAuditoriaMovimientoCajaAsync(desde, hasta)).ToList();

                // Deserializar JSON para mostrar montos correctamente
                foreach (var a in auditoriaCajas) a.DeserializarJson();
                foreach (var a in auditoriaMovimientos) a.DeserializarJson();

                LogHelper.Log($"[ReporteAdmin] AuditoriaCajas: {auditoriaCajas.Count}, AuditoriaMovimientos: {auditoriaMovimientos.Count}");
                AuditoriaCajas = new ObservableCollection<AuditoriaLogDto>(auditoriaCajas);
                AuditoriaMovimientos = new ObservableCollection<AuditoriaLogDto>(auditoriaMovimientos);

                NotifyOfPropertyChange(() => AuditoriaCajas);
                NotifyOfPropertyChange(() => AuditoriaMovimientos);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Acciones filtro ───────────────────────────────────────────────────
        public async Task Actualizar()      => await CargarAsync();
        public async Task FiltrarEsteMes()  { FechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); FechaHasta = DateTime.Today; await CargarAsync(); }
        public async Task FiltrarUltimos3() { FechaDesde = DateTime.Today.AddMonths(-3); FechaHasta = DateTime.Today; await CargarAsync(); }
        public async Task FiltrarUltimos6() { FechaDesde = DateTime.Today.AddMonths(-6); FechaHasta = DateTime.Today; await CargarAsync(); }
        public async Task FiltrarEsteAnio() { FechaDesde = new DateTime(DateTime.Today.Year, 1, 1); FechaHasta = DateTime.Today; await CargarAsync(); }

        // ── Exportar Excel ───────────────────────────────────────────────────
        public async Task ExportarExcel()
        {
            try
            {
                IsLoading = true;

                // Asegurar datos de auditoría actualizados
                var desde = FechaDesde.Date;
                var hasta = FechaHasta.Date.AddDays(1).AddTicks(-1);

                var auditoriaCajas = (await _auditoriaServicio.ObtenerAuditoriaCajaAsync(desde, hasta)).ToList();
                var auditoriaMovimientos = (await _auditoriaServicio.ObtenerAuditoriaMovimientoCajaAsync(desde, hasta)).ToList();

                foreach (var a in auditoriaCajas) a.DeserializarJson();
                foreach (var a in auditoriaMovimientos) a.DeserializarJson();

                // Cargar datos de cajas para los nuevos exports
                var cajas = (await _cajaServicio.ObtenerHistorialAsync(_sesion.IdSucursal, desde, hasta)).ToList();
                var historialCajas = cajas.Select(caja =>
                {
                    var diff = caja.MontoFinal.HasValue
                        ? caja.MontoFinal.Value - (caja.MontoInicial + caja.Ventas.Sum(v => v.TotalFinal))
                        : (decimal?)null;
                    return new CajaHistorialDto
                    {
                        Id = caja.Id,
                        FechaApertura = caja.FechaApertura.ToString("dd/MM/yyyy HH:mm"),
                        FechaCierre = caja.FechaCierre?.ToString("dd/MM/yyyy HH:mm"),
                        MontoInicial = caja.MontoInicial,
                        MontoFinal = caja.MontoFinal,
                        Diferencia = diff,
                        TipoDiferencia = diff.HasValue ? (diff.Value > 0 ? "Positivo" : diff.Value < 0 ? "Negativo" : "Cero") : "Cero",
                        UsuarioApertura = caja.UsuarioApertura?.Nombre ?? "—",
                        UsuarioCierre = caja.UsuarioCierre?.Nombre,
                        Estado = caja.Estado == 1 ? "Abierta" : "Cerrada",
                        TieneDiferencia = diff.HasValue && Math.Abs(diff.Value) > 0.01m,
                    };
                }).ToList();

                // Cargar ventas por caja
                var ventasPorCaja = cajas
                    .SelectMany(caja => caja.Ventas.Select(venta => new VentaResumenCajaDto
                    {
                        CajaId = caja.Id,
                        NumeroCaja = $"Caja {caja.Id}",
                        FechaVenta = venta.Fecha.ToString("dd/MM/yyyy HH:mm"),
                        Total = venta.TotalFinal,
                        MetodoPago = venta.Pagos?.FirstOrDefault()?.MetodoPago?.Nombre ?? "—",
                        Vendedor = venta.Usuario?.Nombre ?? "—"
                    }))
                    .OrderByDescending(v => v.FechaVenta)
                    .ToList();

                ExportHelper.ExportarAuditoriaCompleto(auditoriaCajas, auditoriaMovimientos, historialCajas, ventasPorCaja, desde, hasta, AbrirDespuesDeExportar);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al exportar: {ex.Message}",
                    "Error", System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    /// <summary>
    /// DTO para ventas por caja en export.
    /// </summary>
    public class VentaResumenCajaDto
    {
        public int CajaId { get; set; }
        public string NumeroCaja { get; set; } = string.Empty;
        public string FechaVenta { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string Vendedor { get; set; } = string.Empty;
    }
}
