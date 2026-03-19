using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class AuditoriaPopupViewModel : NavigableViewModel
    {
        private readonly IAuditoriaAppService _auditoriaService;

        public AuditoriaPopupViewModel(IAuditoriaAppService auditoriaService)
        {
            _auditoriaService = auditoriaService;
            Titulo = "Auditoría de Caja";
            FechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            FechaHasta = DateTime.Today;
        }

        // ── Filtros ───────────────────────────────────────────────────────────
        private DateTime _fechaDesde;
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }

        private DateTime _fechaHasta;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<AuditoriaLogDto> AuditoriaCajas { get; set; } = new();
        public ObservableCollection<AuditoriaLogDto> MovimientosCaja { get; set; } = new();

        // ── Tab activa ───────────────────────────────────────────────────────
        private int _tabActiva;
        public int TabActiva
        {
            get => _tabActiva;
            set { _tabActiva = value; NotifyOfPropertyChange(() => TabActiva); }
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAuditoriaAsync();

        // ── Métodos ──────────────────────────────────────────────────────────
        public async Task CargarAuditoriaAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var resultado = await _auditoriaService.ObtenerAuditoriaCompletaCajaAsync(
                    FechaDesde, FechaHasta);

                AuditoriaCajas = new ObservableCollection<AuditoriaLogDto>(resultado.AuditoriaCajas);
                MovimientosCaja = new ObservableCollection<AuditoriaLogDto>(resultado.MovimientosCaja);

                NotifyOfPropertyChange(() => AuditoriaCajas);
                NotifyOfPropertyChange(() => MovimientosCaja);
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

        public async Task Filtrar()
            => await CargarAuditoriaAsync();

        public async Task Cerrar()
        {
            await TryCloseAsync();
        }
    }
}
