using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReportesVentasViewModel : NavigableViewModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        private KpiGeneralDto _kpis = new();
        public KpiGeneralDto Kpis
        {
            get => _kpis;
            set { _kpis = value; NotifyOfPropertyChange(() => Kpis); }
        }

        private ObservableCollection<VentaPorDiaDto> _ventasPorDia = new();
        public ObservableCollection<VentaPorDiaDto> VentasPorDia
        {
            get => _ventasPorDia;
            set { _ventasPorDia = value; NotifyOfPropertyChange(() => VentasPorDia); }
        }

        private ObservableCollection<VentaPorMetodoDto> _ventasPorMetodo = new();
        public ObservableCollection<VentaPorMetodoDto> VentasPorMetodo
        {
            get => _ventasPorMetodo;
            set { _ventasPorMetodo = value; NotifyOfPropertyChange(() => VentasPorMetodo); }
        }

        private ObservableCollection<VentaPorSucursalDto> _ventasPorSucursal = new();
        public ObservableCollection<VentaPorSucursalDto> VentasPorSucursal
        {
            get => _ventasPorSucursal;
            set { _ventasPorSucursal = value; NotifyOfPropertyChange(() => VentasPorSucursal); }
        }

        public async Task CargarAsync()
        {
            await Task.Delay(200); // TODO: await _ventasServicio.ObtenerReporteAsync(FechaDesde, FechaHasta)
            CargarMock();
        }

        private void CargarMock()
        {

        }
    }
}
