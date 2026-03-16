using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteRotacionViewModel : NavigableViewModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        private ObservableCollection<ReporteRotacionDto> _items = new();
        public ObservableCollection<ReporteRotacionDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        public async Task CargarAsync()
        {
            await Task.Delay(200); // TODO: await _reporteServicio.ObtenerRotacionAsync(FechaDesde, FechaHasta)
            CargarMock();
        }

        private void CargarMock()
        {
           
        }
    }
}
