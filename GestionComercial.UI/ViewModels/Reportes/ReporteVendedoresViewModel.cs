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
            Items = new ObservableCollection<ReporteVendedorDto>
            {
                new() { IdUsuario = 1, UsuarioNombre = "García, Juan",      Sucursal = "Casa Central",   CantidadVentas = 98, TotalVendido = 485000, PromedioVenta = 4948.98m, TotalDescuentos = 15520 },
                new() { IdUsuario = 2, UsuarioNombre = "López, María",      Sucursal = "Sucursal Norte", CantidadVentas = 76, TotalVendido = 320000, PromedioVenta = 4210.53m, TotalDescuentos = 16320 },
                new() { IdUsuario = 3, UsuarioNombre = "Martínez, Carlos",  Sucursal = "Casa Central",   CantidadVentas = 65, TotalVendido = 280000, PromedioVenta = 4307.69m, TotalDescuentos = 7840  },
                new() { IdUsuario = 4, UsuarioNombre = "Rodríguez, Ana",    Sucursal = "Sucursal Sur",   CantidadVentas = 58, TotalVendido = 165000, PromedioVenta = 2844.83m, TotalDescuentos = 12210 },
            };

            // Top vendedor = el primero ordenado por TotalVendido
            TopVendedor = Items[0];
        }
    }
}
