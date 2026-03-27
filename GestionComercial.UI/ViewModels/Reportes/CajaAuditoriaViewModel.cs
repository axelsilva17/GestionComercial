using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces;
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
    /// <summary>
    /// DTO para mostrar movimientos de caja en la auditoría.
    /// </summary>
    public class MovimientoAuditoriaDto
    {
        public string TipoOperacion { get; set; } = string.Empty;
        public string Descripcion   { get; set; } = string.Empty;
        public decimal Monto        { get; set; }
        public string Fecha        { get; set; } = string.Empty;
        public string Usuario       { get; set; } = string.Empty;
        public string Icono        { get; set; } = string.Empty;
        public bool   EsIngreso    { get; set; }
    }

    /// <summary>
    /// DTO para mostrar un cierre de caja en la auditoría.
    /// </summary>
    public class CajaAuditoriaItemDto
    {
        public int     Id              { get; set; }
        public string  FechaApertura   { get; set; } = string.Empty;
        public string  HoraApertura    { get; set; } = string.Empty;
        public string  UsuarioApertura { get; set; } = string.Empty;
        public decimal MontoInicial   { get; set; }
        public decimal VentasEfectivo { get; set; }
        public decimal Ingresos       { get; set; }
        public decimal Egresos        { get; set; }
        public decimal EfectivoEnCaja  { get; set; } // MontoInicial + VentasEfectivo + Ingresos - Egresos
        public decimal? MontoFinal    { get; set; }
        public decimal DiferenciaSinEfectivo { get; set; } // Conteo físico - MontoInicial (sin ventas)
        public decimal DiferenciaConEfectivo { get; set; } // Conteo físico - (MontoInicial + VentasEfectivo + Ingresos - Egresos)
        public string? FechaCierre     { get; set; }
        public string? UsuarioCierre  { get; set; }
        public string  Estado          { get; set; } = string.Empty;
        public string  EstadoColor     { get; set; } = "#10B981"; // Verde por defecto
    }

    public class CajaAuditoriaViewModel : NavigableViewModel
    {
        private readonly ICajaServicio _cajaServicio;
        private readonly IUnitOfWork   _uow;

        private DateTime _fechaDesde = DateTime.Today.AddDays(-30);
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); _ = CargarDatosAsync(); }
        }

        private DateTime _fechaHasta = DateTime.Today;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); _ = CargarDatosAsync(); }
        }

        private ObservableCollection<CajaAuditoriaItemDto> _cajas = new();
        public ObservableCollection<CajaAuditoriaItemDto> Cajas
        {
            get => _cajas;
            set => SetProperty(ref _cajas, value);
        }

        private CajaAuditoriaItemDto? _cajaSeleccionada;
        public CajaAuditoriaItemDto? CajaSeleccionada
        {
            get => _cajaSeleccionada;
            set
            {
                SetProperty(ref _cajaSeleccionada, value);
                if (value != null)
                    _ = CargarMovimientosAsync(value.Id);
            }
        }

        private ObservableCollection<MovimientoAuditoriaDto> _movimientos = new();
        public ObservableCollection<MovimientoAuditoriaDto> Movimientos
        {
            get => _movimientos;
            set => SetProperty(ref _movimientos, value);
        }

        // ── KPIs summary ───────────────────────────────────────────────────────
        private int _totalCajas;
        public int TotalCajas
        {
            get => _totalCajas;
            set => SetProperty(ref _totalCajas, value);
        }

        private int _cajasCerradas;
        public int CajasCerradas
        {
            get => _cajasCerradas;
            set => SetProperty(ref _cajasCerradas, value);
        }

        private decimal _totalDiferencias;
        public decimal TotalDiferencias
        {
            get => _totalDiferencias;
            set => SetProperty(ref _totalDiferencias, value);
        }

        private int _cajasConDiferencia;
        public int CajasConDiferencia
        {
            get => _cajasConDiferencia;
            set => SetProperty(ref _cajasConDiferencia, value);
        }

        // ── Chart para diferencias ─────────────────────────────────────────────
        private ObservableCollection<ISeries> _series = new();
        public ObservableCollection<ISeries> Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        public CajaAuditoriaViewModel(ICajaServicio cajaServicio, IUnitOfWork uow)
        {
            _cajaServicio = cajaServicio;
            _uow = uow;
            Titulo = "Auditoría";
            Subtitulo = "Caja — historial y diferencias";
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            IsLoading = true;
            try
            {
                var historial = await _cajaServicio.ObtenerHistorialAsync(0, FechaDesde, FechaHasta.AddDays(1));
                
                var cajasDto = historial.Select(c => new CajaAuditoriaItemDto
                {
                    Id              = c.Id,
                    FechaApertura   = c.FechaApertura.ToString("dd/MM/yyyy"),
                    HoraApertura    = c.FechaApertura.ToString("HH:mm"),
                    UsuarioApertura = c.UsuarioApertura?.Nombre ?? "Desconocido",
                    MontoInicial    = c.MontoInicial,
                    MontoFinal      = c.MontoFinal,
                    FechaCierre     = c.FechaCierre?.ToString("dd/MM/yyyy"),
                    UsuarioCierre   = c.UsuarioCierre?.Nombre,
                    Estado          = c.EstaAbierta ? "Abierta" : "Cerrada",
                    EstadoColor     = c.EstaAbierta ? "#F59E0B" : "#10B981"
                }).ToList();

                // Obtener ventas efectivo por caja (simulado por ahora)
                foreach (var caja in cajasDto)
                {
                    var ventasEfvo = await _cajaServicio.ObtenerTotalEfectivoPorCajaAsync(caja.Id);
                    caja.VentasEfectivo = ventasEfvo;
                    caja.EfectivoEnCaja = caja.MontoInicial + ventasEfvo;

                    // Calcular diferencias si hay monto final
                    if (caja.MontoFinal.HasValue)
                    {
                        caja.DiferenciaSinEfectivo = caja.MontoFinal.Value - caja.MontoInicial;
                        caja.DiferenciaConEfectivo = caja.MontoFinal.Value - caja.EfectivoEnCaja;
                    }
                }

                Cajas = new ObservableCollection<CajaAuditoriaItemDto>(cajasDto);
                
                // KPIs
                TotalCajas = cajasDto.Count;
                CajasCerradas = cajasDto.Count(c => c.Estado == "Cerrada");
                TotalDiferencias = cajasDto.Where(c => c.MontoFinal.HasValue).Sum(c => Math.Abs(c.DiferenciaConEfectivo));
                CajasConDiferencia = cajasDto.Count(c => c.MontoFinal.HasValue && c.DiferenciaConEfectivo != 0);

                // Chart
                ActualizarGrafico(cajasDto);
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar auditoría: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CargarMovimientosAsync(int idCaja)
        {
            try
            {
                var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja);
                
                var movDto = movimientos.Select(m => new MovimientoAuditoriaDto
                {
                    TipoOperacion = m.Tipo == 1 ? "Apertura" : 
                                   m.Tipo == 2 ? "Ingreso" : 
                                   m.Tipo == 3 ? "Egreso" : "Cierre",
                    Descripcion   = m.Concepto ?? "-",
                    Monto         = m.Monto,
                    Fecha         = m.Fecha.ToString("dd/MM HH:mm"),
                    Usuario       = m.Usuario?.Nombre ?? "Sistema",
                    Icono         = m.Tipo == 2 ? "↑" : m.Tipo == 3 ? "↓" : "•",
                    EsIngreso     = m.Tipo == 2
                }).ToList();

                Movimientos = new ObservableCollection<MovimientoAuditoriaDto>(movDto);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CajaAuditoria] Error movimientos: {ex.Message}");
            }
        }

        private void ActualizarGrafico(System.Collections.Generic.List<CajaAuditoriaItemDto> cajas)
        {
            var diffs = cajas.Where(c => c.MontoFinal.HasValue).Take(10).ToList();
            
            Series = new ObservableCollection<ISeries>
            {
                new ColumnSeries<decimal>
                {
                    Values = diffs.Select(c => c.DiferenciaConEfectivo).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#38BDF8")),
                    Name = "Diferencia ($)"
                }
            };
        }

        // ── Exportar ──────────────────────────────────────────────────────────
        public async Task ExportarExcel()
        {
            try
            {
                IsLoading = true;
                ExportHelper.ExportarAuditoriaCaja(Cajas.ToList());
            }
            catch (Exception ex)
            {
                MostrarError($"Error al exportar: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
