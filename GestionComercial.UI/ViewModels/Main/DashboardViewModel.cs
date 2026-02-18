using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;

namespace GestionComercial.UI.ViewModels.Main
{
    public class DashboardViewModel : NavigableViewModel
    {
        private decimal _totalVentasHoy;
        private int _cantidadVentasHoy;
        private decimal _totalVentasMes;
        private int _cantidadVentasMes;
        private decimal _saldoCaja;
        private string _horaAperturaCaja;
        private int _productosCriticos;
        private decimal _ticketPromedio;
        private double _margenBrutoMes;
        private int _ventasPendientes;
        private double _progresoVentasHoy;
        private double _progresoVentasMes;

        public DashboardViewModel()
        {
            Titulo = "Dashboard";
            Subtitulo = "Resumen general";
            VentasRecientes = new ObservableCollection<VentaResumenDash>();
            ProductosCriticosList = new ObservableCollection<ProductoCriticoDash>();
        }

        public string UsuarioNombre => IoC.Get<ShellViewModel>()?.UsuarioNombre ?? "";
        public string FechaHoy => DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");

        public decimal TotalVentasHoy
        {
            get => _totalVentasHoy;
            set { _totalVentasHoy = value; NotifyOfPropertyChange(() => TotalVentasHoy); }
        }
        public int CantidadVentasHoy
        {
            get => _cantidadVentasHoy;
            set { _cantidadVentasHoy = value; NotifyOfPropertyChange(() => CantidadVentasHoy); }
        }
        public decimal TotalVentasMes
        {
            get => _totalVentasMes;
            set { _totalVentasMes = value; NotifyOfPropertyChange(() => TotalVentasMes); }
        }
        public int CantidadVentasMes
        {
            get => _cantidadVentasMes;
            set { _cantidadVentasMes = value; NotifyOfPropertyChange(() => CantidadVentasMes); }
        }
        public decimal SaldoCaja
        {
            get => _saldoCaja;
            set { _saldoCaja = value; NotifyOfPropertyChange(() => SaldoCaja); }
        }
        public string HoraAperturaCaja
        {
            get => _horaAperturaCaja;
            set { _horaAperturaCaja = value; NotifyOfPropertyChange(() => HoraAperturaCaja); }
        }
        public int ProductosCriticos
        {
            get => _productosCriticos;
            set { _productosCriticos = value; NotifyOfPropertyChange(() => ProductosCriticos); }
        }
        public decimal TicketPromedio
        {
            get => _ticketPromedio;
            set { _ticketPromedio = value; NotifyOfPropertyChange(() => TicketPromedio); }
        }
        public double MargenBrutoMes
        {
            get => _margenBrutoMes;
            set { _margenBrutoMes = value; NotifyOfPropertyChange(() => MargenBrutoMes); }
        }
        public int VentasPendientes
        {
            get => _ventasPendientes;
            set { _ventasPendientes = value; NotifyOfPropertyChange(() => VentasPendientes); }
        }
        public double ProgresoVentasHoy
        {
            get => _progresoVentasHoy;
            set { _progresoVentasHoy = value; NotifyOfPropertyChange(() => ProgresoVentasHoy); }
        }
        public double ProgresoVentasMes
        {
            get => _progresoVentasMes;
            set { _progresoVentasMes = value; NotifyOfPropertyChange(() => ProgresoVentasMes); }
        }

        public ObservableCollection<VentaResumenDash> VentasRecientes { get; set; }
        public ObservableCollection<ProductoCriticoDash> ProductosCriticosList { get; set; }

        // Navegación rápida desde dashboard
        public async void IrVentas()
        {
            var shell = IoC.Get<ShellViewModel>();
            await shell.IrVentas();
        }

        public async void IrVentasCompleto() => await IoC.Get<ShellViewModel>().IrVentas();
        public async void IrProductosStock() => await IoC.Get<ShellViewModel>().IrProductos();
    }

    public class VentaResumenDash
    {
        public int IdVenta { get; set; }
        public string ClienteNombre { get; set; }
        public decimal TotalFinal { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class ProductoCriticoDash
    {
        public string Nombre { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }
}
