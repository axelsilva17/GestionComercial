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
    public class ReporteRotacionViewModel : NavigableViewModel
    {
        private readonly IReporteServicio _reporteServicio;
        private readonly SesionServicio _sesion;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public ReporteRotacionViewModel(IReporteServicio reporteServicio, SesionServicio sesion)
        {
            _reporteServicio = reporteServicio;
            _sesion = sesion;
        }

        private ObservableCollection<ReporteRotacionDto> _items = new();
        public ObservableCollection<ReporteRotacionDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _reporteServicio.RotacionProductosAsync(
                    _sesion.IdEmpresa, FechaDesde, FechaHasta, top: 200);
                Items = new ObservableCollection<ReporteRotacionDto>(datos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReporteRotacion] Error: {ex.Message}");
            }
        }
    }
}
