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
            Items = new ObservableCollection<ReporteRotacionDto>
            {
                new() { IdProducto = 1, ProductoNombre = "Mouse Inalámbrico", Categoria = "Electrónica", StockActual = 3,  CantidadVendida = 62, CantidadComprada = 80, IndiceRotacion = 8.2m, UltimaVenta = DateTime.Today.AddDays(-1),  UltimaCompra = DateTime.Today.AddDays(-20) },
                new() { IdProducto = 2, ProductoNombre = "Auriculares Pro X", Categoria = "Electrónica", StockActual = 8,  CantidadVendida = 45, CantidadComprada = 60, IndiceRotacion = 5.6m, UltimaVenta = DateTime.Today.AddDays(-2),  UltimaCompra = DateTime.Today.AddDays(-15) },
                new() { IdProducto = 3, ProductoNombre = "Teclado Mecánico",  Categoria = "Electrónica", StockActual = 25, CantidadVendida = 28, CantidadComprada = 40, IndiceRotacion = 3.5m, UltimaVenta = DateTime.Today.AddDays(-5),  UltimaCompra = DateTime.Today.AddDays(-10) },
                new() { IdProducto = 4, ProductoNombre = "Monitor 24\"",      Categoria = "Electrónica", StockActual = 45, CantidadVendida = 12, CantidadComprada = 20, IndiceRotacion = 1.2m, UltimaVenta = DateTime.Today.AddDays(-8),  UltimaCompra = DateTime.Today.AddDays(-5)  },
                new() { IdProducto = 5, ProductoNombre = "Webcam HD",         Categoria = "Electrónica", StockActual = 15, CantidadVendida = 34, CantidadComprada = 50, IndiceRotacion = 4.3m, UltimaVenta = DateTime.Today.AddDays(-3),  UltimaCompra = DateTime.Today.AddDays(-12) },
            };
        }
    }
}
