using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.Helpers;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Cajas
{
    public class OnDemandCajaAuditoriaViewModel : NavigableViewModel
    {
        private readonly ICajaServicio _cajaServicio;
        private readonly IUnitOfWork  _uow;
        private readonly SesionServicio _sesion;

        public OnDemandCajaAuditoriaViewModel(ICajaServicio cajaServicio, IUnitOfWork uow, SesionServicio sesion)
        {
            _cajaServicio = cajaServicio;
            _uow = uow;
            _sesion = sesion;
            Titulo = "Auditoría On-Demand";
            Subtitulo = "Consulta y exporta historial de cajas";
        }

        private DateTime _startDate = DateTime.Today.AddMonths(-1);
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; NotifyOfPropertyChange(() => StartDate); }
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; NotifyOfPropertyChange(() => EndDate); }
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

        // KPIs
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

        private string? _exportStatus;
        public string? ExportStatus
        {
            get => _exportStatus;
            set { _exportStatus = value; NotifyOfPropertyChange(() => ExportStatus); }
        }

        private bool _isExporting;
        public bool IsExporting
        {
            get => _isExporting;
            set { _isExporting = value; NotifyOfPropertyChange(() => IsExporting); }
        }

        public async Task CargarAuditoria()
        {
            if (EndDate < StartDate)
            {
                MessageBox.Show("La fecha de inicio debe ser anterior o igual a la fecha de fin.", 
                    "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;
            try
            {
                var historial = await _cajaServicio.ObtenerHistorialAsync(_sesion.IdSucursal, StartDate, EndDate.AddDays(1));
                
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

                // Obtener ventas efectivo por caja
                foreach (var caja in cajasDto)
                {
                    var ventasEfvo = await _cajaServicio.ObtenerTotalEfectivoPorCajaAsync(caja.Id);
                    caja.VentasEfectivo = ventasEfvo;
                    caja.EfectivoEnCaja = caja.MontoInicial + ventasEfvo;
                    caja.Ingresos = ventasEfvo;
                    caja.Egresos = 0;

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

                ExportStatus = $"Cargadas {TotalCajas} cajas";
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar auditoría: {ex.Message}");
                ExportStatus = $"Error: {ex.Message}";
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
                System.Diagnostics.Debug.WriteLine($"[OnDemandCajaAuditoria] Error movimientos: {ex.Message}");
            }
        }

        public async Task ExportarAuditoria()
        {
            if (Cajas == null || !Cajas.Any())
            {
                MessageBox.Show("No hay datos para exportar. Primero cargá la auditoría.", 
                    "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                IsExporting = true;
                ExportStatus = "Exportando...";
                
                // Usar el helper existente para exportar
                ExportHelper.ExportarAuditoriaCaja(Cajas.ToList());
                
                ExportStatus = $"Exportado: {StartDate:yyyy-MM-dd} a {EndDate:yyyy-MM-dd}";
            }
            catch (Exception ex)
            {
                ExportStatus = $"Error durante export: {ex.Message}";
                MessageBox.Show("Error al exportar auditoría.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsExporting = false;
            }
        }
    }
}
