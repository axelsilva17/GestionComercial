using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteStockViewModel : NavigableViewModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        private ObservableCollection<ReportesStockDto> _items = new();
        public ObservableCollection<ReportesStockDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        // Totales resumen
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
            await Task.Delay(200); // TODO: await _reporteServicio.ObtenerStockAsync()
            CargarMock();
        }

        private void CargarMock()
        {
           

            // Calcular totales usando las propiedades calculadas del DTO
            TotalSinStock  = 0;
            TotalStockBajo = 0;
            foreach (var item in Items)
            {
                if (item.SinStock)  TotalSinStock++;
                else if (item.StockBajo) TotalStockBajo++;
            }
        }
    }
}
