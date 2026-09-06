using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReportesVentasViewModel : NavigableViewModel
    {
        private readonly IReporteServicio _reporteServicio;
        private readonly SesionServicio _sesion;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public ReportesVentasViewModel(IReporteServicio reporteServicio, SesionServicio sesion)
        {
            _reporteServicio = reporteServicio;
            _sesion = sesion;
        }

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
            try
            {
                var idEmpresa = _sesion.IdEmpresa;
                var idSucursal = _sesion.IdSucursal;

                var tareas = new List<Task>
                {
                    CargarKpis(idEmpresa, idSucursal),
                    CargarVentasPorDia(idEmpresa),
                    CargarVentasPorMetodo(idSucursal),
                    CargarVentasPorSucursal(idEmpresa),
                };

                await Task.WhenAll(tareas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReportesVentas] Error: {ex.Message}");
            }
        }

        private async Task CargarKpis(int idEmpresa, int idSucursal)
        {
            Kpis = await _reporteServicio.KpisVentasBaseAsync(idEmpresa, idSucursal, FechaDesde, FechaHasta);
        }

        private async Task CargarVentasPorDia(int idEmpresa)
        {
            var datos = await _reporteServicio.VentasPorDiaAsync(idEmpresa, FechaDesde, FechaHasta);
            VentasPorDia = new ObservableCollection<VentaPorDiaDto>(datos.Select(d => new VentaPorDiaDto
            {
                Dia = DateTime.TryParse(d.Dia, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var diaVal)
                    ? diaVal.ToString("dd/MM") : d.Dia,
                Total = d.Total,
                Cantidad = d.Cantidad,
            }));
        }

        private async Task CargarVentasPorMetodo(int idSucursal)
        {
            var datos = await _reporteServicio.MetodosPagoUtilizadosAsync(idSucursal, FechaDesde, FechaHasta);
            VentasPorMetodo = new ObservableCollection<VentaPorMetodoDto>(
                datos.Select(m => new VentaPorMetodoDto
                {
                    MetodoNombre = m.Metodo,
                    Total = m.Total,
                    Cantidad = m.Cantidad,
                }));
        }

        private async Task CargarVentasPorSucursal(int idEmpresa)
        {
            var datos = await _reporteServicio.VentasPorSucursalAsync(idEmpresa, FechaDesde, FechaHasta);
            VentasPorSucursal = new ObservableCollection<VentaPorSucursalDto>(datos);
        }
    }
}
