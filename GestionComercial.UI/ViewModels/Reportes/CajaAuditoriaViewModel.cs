using GestionComercial.Aplicacion.DTOs.Caja;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    ///     /// DTO para mostrar movimientos de caja en la auditoría.
    

public class CajaAuditoriaViewModel : NavigableViewModel
{
        private readonly ICajaServicio _cajaServicio;
        private readonly IUnitOfWork   _uow;
        private readonly SesionServicio _sesion;
        private CancellationTokenSource? _ctsCargarDatos;

        // ── Turno filter ──────────────────────────────────────────────
        public ObservableCollection<string> TurnosDisponibles { get; } = new() { "Todos", "Mañana", "Tarde", "Noche" };

        private string _turnoFiltro = "Todos";
        public string TurnoFiltro
        {
            get => _turnoFiltro;
            set
            {
                _turnoFiltro = value;
                NotifyOfPropertyChange(() => TurnoFiltro);
                AplicarFiltroTurno();
            }
        }

        private ObservableCollection<CajaAuditoriaItemDto> _cajasFiltradas = new();
        public ObservableCollection<CajaAuditoriaItemDto> CajasFiltradas
        {
            get => _cajasFiltradas;
            set => SetProperty(ref _cajasFiltradas, value);
        }

        // ── Crear caja ─────────────────────────────────────────────────
        private string _turnoNuevaCaja = "Mañana";
        public string TurnoNuevaCaja
        {
            get => _turnoNuevaCaja;
            set { _turnoNuevaCaja = value; NotifyOfPropertyChange(() => TurnoNuevaCaja); }
        }

        private DateTime _fechaDesde = DateTime.Today.AddMonths(-6);
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set 
            { 
                _fechaDesde = value; 
                NotifyOfPropertyChange(() => FechaDesde); 
                _ = CargarDatosAsync(); 
            }
        }

        private DateTime _fechaHasta = DateTime.Today;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set 
            { 
                _fechaHasta = value; 
                NotifyOfPropertyChange(() => FechaHasta); 
                _ = CargarDatosAsync(); 
            }
        }

        private ObservableCollection<CajaAuditoriaItemDto> _cajas = new();
        // Nueva exposición para total de ingresos/egresos por caja actual
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
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

    public CajaAuditoriaViewModel(ICajaServicio cajaServicio, IUnitOfWork uow, SesionServicio sesion)
        {
            _cajaServicio = cajaServicio;
            _uow          = uow;
            _sesion       = sesion;
            Titulo = "Auditoría";
            Subtitulo = "Caja — historial y diferencias";
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await CargarDatosAsync();
        }

    private async Task CargarDatosAsync()
    {
        // Cancelar carga anterior para evitar condiciones de carrera
        _ctsCargarDatos?.Cancel();
        _ctsCargarDatos = new CancellationTokenSource();
        var token = _ctsCargarDatos.Token;

        IsLoading = true;
        try
        {
            var historial = await _cajaServicio.ObtenerHistorialAsync(_sesion.IdSucursal, FechaDesde, FechaHasta.AddDays(1));
            
            if (token.IsCancellationRequested) return;

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
                Turno           = c.Turno ?? string.Empty,
                Estado          = c.EstaAbierta ? "Abierta" : "Cerrada",
                EstadoColor     = c.EstaAbierta ? "#F59E0B" : "#10B981"
            }).ToList();

            if (token.IsCancellationRequested) return;

            // Obtener ventas efectivo por caja (simulado por ahora)
            foreach (var caja in cajasDto)
            {
                var ventasEfvo = await _cajaServicio.ObtenerTotalEfectivoPorCajaAsync(caja.Id);
                caja.VentasEfectivo = ventasEfvo;
                caja.EfectivoEnCaja = caja.MontoInicial + ventasEfvo;
                // Asumimos ingresos totales como ventas en efectivo para mostrar en la UI,
                // y egresos como 0 por now (puedes ajustar si tienes datos de egresos específicos).
                caja.Ingresos = ventasEfvo;
                caja.Egresos = 0;

                // Calcular diferencias si hay monto final
                if (caja.MontoFinal.HasValue)
                {
                    caja.DiferenciaSinEfectivo = caja.MontoFinal.Value - caja.MontoInicial;
                    caja.DiferenciaConEfectivo = caja.MontoFinal.Value - caja.EfectivoEnCaja;
                }
            }

            if (token.IsCancellationRequested) return;

            Cajas = new ObservableCollection<CajaAuditoriaItemDto>(cajasDto);
            AplicarFiltroTurno();

            // KPIs
            TotalCajas = cajasDto.Count;
            CajasCerradas = cajasDto.Count(c => c.Estado == "Cerrada");
            TotalDiferencias = cajasDto.Where(c => c.MontoFinal.HasValue).Sum(c => Math.Abs(c.DiferenciaConEfectivo));
            CajasConDiferencia = cajasDto.Count(c => c.MontoFinal.HasValue && c.DiferenciaConEfectivo != 0);

            // Chart
            ActualizarGrafico(cajasDto);
        }
        catch (OperationCanceledException)
        {
            // Ignorar - se canceló por cambio de fecha
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

        // ── Filter by turno ────────────────────────────────────────────────
        private void AplicarFiltroTurno()
        {
            if (string.IsNullOrEmpty(TurnoFiltro) || TurnoFiltro == "Todos")
                CajasFiltradas = new ObservableCollection<CajaAuditoriaItemDto>(Cajas);
            else
                CajasFiltradas = new ObservableCollection<CajaAuditoriaItemDto>(
                    Cajas.Where(c => c.Turno == TurnoFiltro));
        }

        // ── Crear nueva caja ───────────────────────────────────────────────
        public async Task CrearCajaAsync()
        {
            try
            {
                await _cajaServicio.AbrirCajaAsync(
                    _sesion.IdSucursal, _sesion.IdUsuario, 0, turno: TurnoNuevaCaja);
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al crear caja: {ex.Message}");
            }
        }

        private async Task CargarMovimientosAsync(int idCaja)
        {
            try
            {
                var movimientos = await _uow.MovimientosCaja.ObtenerPorCajaAsync(idCaja);
                
                var movDto = movimientos.Select(m => new MovimientoAuditoriaDto
                {
                    TipoOperacion = m.Tipo == 1 ? "Ingreso" : 
                                   m.Tipo == 2 ? "Egreso" : 
                                   m.Tipo == 3 ? "Apertura" : "Cierre",
                    Descripcion   = m.Concepto ?? "-",
                    Monto         = m.Monto,
                    Fecha         = m.Fecha.ToString("dd/MM HH:mm"),
                    Usuario       = m.Usuario?.Nombre ?? "Sistema",
                    Icono         = m.Tipo == 1 ? "↑" : m.Tipo == 2 ? "↓" : "•",
                    EsIngreso     = m.Tipo == 1
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
                new ColumnSeries<double>
                {
                    Values = diffs.Select(c => (double)c.DiferenciaConEfectivo).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#38BDF8")),
                    Name = "Diferencia ($)"
                }
            };
        }

        // ── Exportar ──────────────────────────────────────────────────────────
        public void ExportarExcel()
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

        // ── Volver a Reporte Admin ─────────────────────────────────────────────
        public async Task Volver()
        {
            var reporteAdmin = Caliburn.Micro.IoC.Get<ReporteAdminViewModel>();
            await Caliburn.Micro.IoC.Get<ShellViewModel>().ActivateItemAsync(reporteAdmin, CancellationToken.None);
        }
    }
}
