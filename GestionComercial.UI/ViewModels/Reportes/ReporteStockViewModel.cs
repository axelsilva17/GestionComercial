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
    public class ReporteStockViewModel : NavigableViewModel
    {
        private readonly IReporteServicio _reporteServicio;
        private readonly SesionServicio _sesion;

        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        public ReporteStockViewModel(IReporteServicio reporteServicio, SesionServicio sesion)
        {
            _reporteServicio = reporteServicio;
            _sesion = sesion;
        }

        private ObservableCollection<ReportesStockDto> _items = new();
        public ObservableCollection<ReportesStockDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        private int _totalSinStock;
        public int TotalSinStock
        {
            get => _totalSinStock;
            set { _totalSinStock = value; NotifyOfPropertyChange(() => TotalSinStock); }
        }

        private int _totalStockBajo;
        public int TotalStockBajo
        {
            get => _totalStockBajo;
            set { _totalStockBajo = value; NotifyOfPropertyChange(() => TotalStockBajo); }
        }

        public async Task CargarAsync()
        {
            try
            {
                var datos = await _reporteServicio.StockCriticoAsync(_sesion.IdEmpresa);
                var lista = datos.ToList();
                Items = new ObservableCollection<ReportesStockDto>(lista);
                TotalSinStock = lista.Count(i => i.SinStock);
                TotalStockBajo = lista.Count(i => i.StockBajo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReporteStock] Error: {ex.Message}");
            }
        }
    }
}
