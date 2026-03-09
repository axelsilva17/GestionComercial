using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteVendedoresViewModel : NavigableViewModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        private ObservableCollection<ReporteVendedorDto> _items = new();
        public ObservableCollection<ReporteVendedorDto> Items
        {
            get => _items;
            set { _items = value; NotifyOfPropertyChange(() => Items); }
        }

        // Top vendedor
        private ReporteVendedorDto _topVendedor;
        public ReporteVendedorDto TopVendedor
        {
            get => _topVendedor;
            set { _topVendedor = value; NotifyOfPropertyChange(() => TopVendedor); }
        }

        public async Task CargarAsync()
        {
            await Task.Delay(200); // TODO: await _reporteServicio.ObtenerVendedoresAsync(FechaDesde, FechaHasta)
            CargarMock();
        }

        private void CargarMock()
        {
            

            // Top vendedor = el primero ordenado por TotalVendido
            TopVendedor = Items[0];
        }
    }
}
