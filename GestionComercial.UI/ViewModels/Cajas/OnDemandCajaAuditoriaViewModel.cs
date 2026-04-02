using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using System.Windows;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.UI.Helpers;

namespace GestionComercial.UI.ViewModels.Cajas
{
    public class OnDemandCajaAuditoriaViewModel : Screen
    {
        private readonly IUnitOfWork _uow;

        public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-1);
        public DateTime EndDate { get; set; } = DateTime.Today;

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

        public OnDemandCajaAuditoriaViewModel(IUnitOfWork uow)
        {
            _uow = uow;
            DisplayName = "Auditoria On-Demand";
        }

        public async Task ExportarAuditoria()
        {
            if (EndDate < StartDate)
            {
                MessageBox.Show("La fecha de inicio debe ser anterior o igual a la fecha de fin.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                IsExporting = true;
                ExportStatus = "Export en progreso...";
                // Llamar al servicio de aplicación de auditoría para obtener los datos consolidados
                var auditoriaApp = IoC.Get<IAuditoriaAppService>();
                var datos = await auditoriaApp.ObtenerAuditoriaCompletaCajaAsync(StartDate, EndDate);

                // Exportar usando el helper de Excel con las listas ya deserializadas
                ExportHelper.ExportarAuditoriaCompleto(
                    datos.AuditoriaCajas,
                    datos.MovimientosCaja,
                    null,
                    null,
                    StartDate,
                    EndDate
                );
                ExportStatus = $"Export realizado: {StartDate:yyyy-MM-dd} a {EndDate:yyyy-MM-dd}";
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
