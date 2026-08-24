using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteVendedoresViewModel : NavigableViewModel
    {
        private readonly IReporteServicio _reporteServicio;
        private readonly SesionServicio _sesion;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public ReporteVendedoresViewModel(IReporteServicio reporteServicio, SesionServicio sesion)
        {
            _reporteServicio = reporteServicio;
            _sesion = sesion;
        }

        private ObservableCollection<ReporteVendedorDto> _items = new();
        public ObservableCollection<ReporteVendedorDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private ReporteVendedorDto _topVendedor;
        public ReporteVendedorDto TopVendedor
        {
            get => _topVendedor;
            set { _topVendedor = value; NotifyOfPropertyChange(() => TopVendedor); }
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _reporteServicio.VentasPorVendedorAsync(
                    _sesion.IdSucursal, FechaDesde, FechaHasta);
                var lista = datos.OrderByDescending(v => v.TotalVendido).ToList();
                Items = new ObservableCollection<ReporteVendedorDto>(lista);
                TopVendedor = lista.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReporteVendedores] Error: {ex.Message}");
            }
        }
    }
}
