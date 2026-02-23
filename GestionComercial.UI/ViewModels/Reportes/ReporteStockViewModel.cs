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
            Items = new ObservableCollection<ReportesStockDto>
            {
                new() { IdProducto = 1, ProductoNombre = "Mouse Inalámbrico",  Categoria = "Electrónica", Sucursal = "Casa Central",   StockActual = 0,  StockMinimo = 10 },
                new() { IdProducto = 2, ProductoNombre = "Auriculares Pro X",  Categoria = "Electrónica", Sucursal = "Casa Central",   StockActual = 3,  StockMinimo = 10 },
                new() { IdProducto = 3, ProductoNombre = "Cable HDMI 2m",      Categoria = "Accesorios",  Sucursal = "Sucursal Norte", StockActual = 5,  StockMinimo = 15 },
                new() { IdProducto = 4, ProductoNombre = "Teclado Mecánico",   Categoria = "Electrónica", Sucursal = "Sucursal Norte", StockActual = 25, StockMinimo = 5  },
                new() { IdProducto = 5, ProductoNombre = "Monitor 24\"",       Categoria = "Electrónica", Sucursal = "Sucursal Sur",   StockActual = 45, StockMinimo = 3  },
                new() { IdProducto = 6, ProductoNombre = "Webcam HD",          Categoria = "Electrónica", Sucursal = "Casa Central",   StockActual = 0,  StockMinimo = 8  },
            };

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
