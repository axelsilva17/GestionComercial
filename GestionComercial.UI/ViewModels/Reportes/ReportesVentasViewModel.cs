using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReportesVentasViewModel : NavigableViewModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        private KpiGeneralDto _kpis = new();
        public KpiGeneralDto Kpis
        {
            get => _kpis;
            set { _kpis = value; NotifyOfPropertyChange(() => Kpis); }
        }

        private ObservableCollection<VentaPorDiaDto> _ventasPorDia = new();
        public ObservableCollection<VentaPorDiaDto> VentasPorDia
        {
            get => _ventasPorDia;
            set { _ventasPorDia = value; NotifyOfPropertyChange(() => VentasPorDia); }
        }

        private ObservableCollection<VentaPorMetodoDto> _ventasPorMetodo = new();
        public ObservableCollection<VentaPorMetodoDto> VentasPorMetodo
        {
            get => _ventasPorMetodo;
            set { _ventasPorMetodo = value; NotifyOfPropertyChange(() => VentasPorMetodo); }
        }

        private ObservableCollection<VentaPorSucursalDto> _ventasPorSucursal = new();
        public ObservableCollection<VentaPorSucursalDto> VentasPorSucursal
        {
            get => _ventasPorSucursal;
            set { _ventasPorSucursal = value; NotifyOfPropertyChange(() => VentasPorSucursal); }
        }

        public async Task CargarAsync()
        {
            await Task.Delay(200); // TODO: await _ventasServicio.ObtenerReporteAsync(FechaDesde, FechaHasta)
            CargarMock();
        }

        private void CargarMock()
        {
            Kpis = new KpiGeneralDto
            {
                TotalVentasPeriodo         = 1_250_000,
                TotalVentasPeriodoAnterior = 980_000,
                MargenPromedio             = 32.5m,
                ProductosBajoStock         = 8,
                TotalTransacciones         = 342,
                TicketPromedio             = 3654.97m,
                MejorProducto              = "Auriculares Pro X",
                MejorVendedor              = "García, Juan"
            };

            VentasPorDia = new ObservableCollection<VentaPorDiaDto>
            {
                new() { Dia = "Lun", Total = 85000,  Cantidad = 12 },
                new() { Dia = "Mar", Total = 120000, Cantidad = 18 },
                new() { Dia = "Mié", Total = 95000,  Cantidad = 14 },
                new() { Dia = "Jue", Total = 145000, Cantidad = 22 },
                new() { Dia = "Vie", Total = 180000, Cantidad = 28 },
                new() { Dia = "Sáb", Total = 210000, Cantidad = 35 },
                new() { Dia = "Dom", Total = 75000,  Cantidad = 10 },
            };

            VentasPorMetodo = new ObservableCollection<VentaPorMetodoDto>
            {
                new() { MetodoNombre = "Efectivo",      Total = 480000, Cantidad = 120, Icono = "💵" },
                new() { MetodoNombre = "Débito",        Total = 380000, Cantidad = 95,  Icono = "💳" },
                new() { MetodoNombre = "Crédito",       Total = 290000, Cantidad = 72,  Icono = "🏦" },
                new() { MetodoNombre = "Transferencia", Total = 100000, Cantidad = 25,  Icono = "📲" },
            };

            VentasPorSucursal = new ObservableCollection<VentaPorSucursalDto>
            {
                new() { SucursalNombre = "Casa Central",   Total = 620000, Cantidad = 155, Porcentaje = 49.6m },
                new() { SucursalNombre = "Sucursal Norte", Total = 380000, Cantidad = 98,  Porcentaje = 30.4m },
                new() { SucursalNombre = "Sucursal Sur",   Total = 250000, Cantidad = 89,  Porcentaje = 20.0m },
            };
        }
    }
}
