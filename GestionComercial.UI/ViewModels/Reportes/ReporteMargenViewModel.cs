using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteMargenViewModel : NavigableViewModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

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
            await Task.Delay(200); // TODO: await _reporteServicio.ObtenerMargenAsync(FechaDesde, FechaHasta)
            CargarMock();
        }

        private void CargarMock()
        {
            Items = new ObservableCollection<ReporteMargenDto>
            {
                new() { IdProducto = 1, ProductoNombre = "Auriculares Pro X",  Categoria = "Electrónica", PrecioCosto = 15000, PrecioVenta = 24000, MargenUnitario = 9000,  MargenPorcentaje = 37.5m, CantidadVendida = 45, MargenTotal = 405000 },
                new() { IdProducto = 2, ProductoNombre = "Mouse Inalámbrico",  Categoria = "Electrónica", PrecioCosto = 8000,  PrecioVenta = 12500, MargenUnitario = 4500,  MargenPorcentaje = 36.0m, CantidadVendida = 62, MargenTotal = 279000 },
                new() { IdProducto = 3, ProductoNombre = "Teclado Mecánico",   Categoria = "Electrónica", PrecioCosto = 22000, PrecioVenta = 34000, MargenUnitario = 12000, MargenPorcentaje = 35.3m, CantidadVendida = 28, MargenTotal = 336000 },
                new() { IdProducto = 4, ProductoNombre = "Monitor 24\"",       Categoria = "Electrónica", PrecioCosto = 85000, PrecioVenta = 120000,MargenUnitario = 35000, MargenPorcentaje = 29.2m, CantidadVendida = 12, MargenTotal = 420000 },
                new() { IdProducto = 5, ProductoNombre = "Webcam HD",          Categoria = "Electrónica", PrecioCosto = 12000, PrecioVenta = 18000, MargenUnitario = 6000,  MargenPorcentaje = 33.3m, CantidadVendida = 34, MargenTotal = 204000 },
            };
        }
    }
}
