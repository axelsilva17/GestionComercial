using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteMargenViewModel : NavigableViewModel
    {
        private readonly IReporteServicio _reporteServicio;
        private readonly SesionServicio _sesion;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public ReporteMargenViewModel(IReporteServicio reporteServicio, SesionServicio sesion)
        {
            _reporteServicio = reporteServicio;
            _sesion = sesion;
        }

        private ObservableCollection<ReporteMargenDto> _items = new();
        public ObservableCollection<ReporteMargenDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private string _busqueda = string.Empty;
        public string Busqueda
        {
            get => _busqueda;
            set { _busqueda = value; NotifyOfPropertyChange(() => Busqueda); }
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _reporteServicio.MargenPorProductoAsync(
                    _sesion.IdEmpresa, FechaDesde, FechaHasta, top: 200);
                Items = new ObservableCollection<ReporteMargenDto>(datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReporteMargen] Error: {ex.Message}");
            }
        }
    }
}
