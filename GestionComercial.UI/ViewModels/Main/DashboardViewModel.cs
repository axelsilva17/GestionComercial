using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Main
{
    public class DashboardViewModel : NavigableViewModel
    {
        // ── Rol (leído del Shell) ─────────────────────────────────────────────
        private ShellViewModel Shell => IoC.Get<ShellViewModel>();
        public string UsuarioNombre  => Shell?.UsuarioNombre ?? "";
        public string UsuarioRol     => Shell?.UsuarioRol    ?? "";
        public string FechaHoy       => DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");

        // Visibilidad de secciones según rol
        public bool MostrarFinanzas   => Shell?.EsGerente ?? false;
        public bool MostrarOperaciones => Shell?.Rol != RolUsuario.Vendedor;
        public string TituloSaludo    => Shell?.Rol switch
        {
            RolUsuario.Gerente       => "Resumen ejecutivo",
            RolUsuario.Administrador => "Panel de administración",
            _                        => "Panel de ventas",
        } ?? "";

        // ── KPIs principales ──────────────────────────────────────────────────
        private decimal _totalVentasHoy;
        public decimal TotalVentasHoy
        {
            get => _totalVentasHoy;
            set { _totalVentasHoy = value; NotifyOfPropertyChange(() => TotalVentasHoy); }
        }

        private int _cantidadVentasHoy;
        public int CantidadVentasHoy
        {
            get => _cantidadVentasHoy;
            set { _cantidadVentasHoy = value; NotifyOfPropertyChange(() => CantidadVentasHoy); }
        }

        private decimal _totalVentasMes;
        public decimal TotalVentasMes
        {
            get => _totalVentasMes;
            set { _totalVentasMes = value; NotifyOfPropertyChange(() => TotalVentasMes); }
        }

        private int _cantidadVentasMes;
        public int CantidadVentasMes
        {
            get => _cantidadVentasMes;
            set { _cantidadVentasMes = value; NotifyOfPropertyChange(() => CantidadVentasMes); }
        }

        private decimal _saldoCaja;
        public decimal SaldoCaja
        {
            get => _saldoCaja;
            set { _saldoCaja = value; NotifyOfPropertyChange(() => SaldoCaja); }
        }

        private string _horaAperturaCaja = "--:--";
        public string HoraAperturaCaja
        {
            get => _horaAperturaCaja;
            set { _horaAperturaCaja = value; NotifyOfPropertyChange(() => HoraAperturaCaja); }
        }

        private int _productosCriticos;
        public int ProductosCriticos
        {
            get => _productosCriticos;
            set { _productosCriticos = value; NotifyOfPropertyChange(() => ProductosCriticos); }
        }

        private int _ventasPendientes;
        public int VentasPendientes
        {
            get => _ventasPendientes;
            set { _ventasPendientes = value; NotifyOfPropertyChange(() => VentasPendientes); }
        }

        private int _ventasCanceladas;
        public int VentasCanceladas
        {
            get => _ventasCanceladas;
            set { _ventasCanceladas = value; NotifyOfPropertyChange(() => VentasCanceladas); }
        }

        // ── Solo Gerente ──────────────────────────────────────────────────────
        private decimal _totalComprasMes;
        public decimal TotalComprasMes
        {
            get => _totalComprasMes;
            set { _totalComprasMes = value; NotifyOfPropertyChange(() => TotalComprasMes); NotifyOfPropertyChange(() => ResultadoNeto); }
        }

        private double _margenBrutoMes;
        public double MargenBrutoMes
        {
            get => _margenBrutoMes;
            set { _margenBrutoMes = value; NotifyOfPropertyChange(() => MargenBrutoMes); }
        }

        private decimal _ticketPromedio;
        public decimal TicketPromedio
        {
            get => _ticketPromedio;
            set { _ticketPromedio = value; NotifyOfPropertyChange(() => TicketPromedio); }
        }

        private decimal _totalVentasMesAnterior;
        public decimal TotalVentasMesAnterior
        {
            get => _totalVentasMesAnterior;
            set { _totalVentasMesAnterior = value; NotifyOfPropertyChange(() => TotalVentasMesAnterior); NotifyOfPropertyChange(() => CrecimientoMes); NotifyOfPropertyChange(() => CrecimientoPositivo); }
        }

        public decimal ResultadoNeto => TotalVentasMes - TotalComprasMes;
        public double CrecimientoMes => TotalVentasMesAnterior > 0
            ? (double)((TotalVentasMes - TotalVentasMesAnterior) / TotalVentasMesAnterior * 100)
            : 0;
        public bool CrecimientoPositivo => CrecimientoMes >= 0;

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<VentaResumenDto>     VentasRecientes       { get; set; } = new();
        public ObservableCollection<ProductoCriticoDash> ProductosCriticosList { get; set; } = new();

        // ── Lifecycle ─────────────────────────────────────────────────────────
        public DashboardViewModel() { Titulo = "Dashboard"; }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            NotifyOfPropertyChange(() => MostrarFinanzas);
            NotifyOfPropertyChange(() => MostrarOperaciones);
            NotifyOfPropertyChange(() => TituloSaludo);
            NotifyOfPropertyChange(() => UsuarioNombre);
            NotifyOfPropertyChange(() => UsuarioRol);
            await CargarAsync();
        }

        private async Task CargarAsync()
        {
            IsLoading = true;
            try
            {
                await Task.Delay(300); // TODO: servicios reales

                // Mock datos según rol
                TotalVentasHoy     = 184500;
                CantidadVentasHoy  = 12;
                TotalVentasMes     = 2350000;
                CantidadVentasMes  = 187;
                SaldoCaja          = 98200;
                HoraAperturaCaja   = "08:30";
                ProductosCriticos  = 5;
                VentasPendientes   = 3;
                VentasCanceladas   = 2;

                // Solo Gerente
                TotalComprasMes        = 1420000;
                MargenBrutoMes         = 39.6;
                TicketPromedio         = 12566;
                TotalVentasMesAnterior = 2180000;

                VentasRecientes = new ObservableCollection<VentaResumenDto>
                {
                    new() { IdVenta=1042, ClienteNombre="Juan Pérez",       TotalFinal=24500, Fecha=DateTime.Now.AddMinutes(-8)  },
                    new() { IdVenta=1041, ClienteNombre="María González",   TotalFinal=8900,  Fecha=DateTime.Now.AddMinutes(-32) },
                    new() { IdVenta=1040, ClienteNombre="Carlos Rodríguez", TotalFinal=45200, Fecha=DateTime.Now.AddMinutes(-61) },
                    new() { IdVenta=1039, ClienteNombre="Ana Martínez",     TotalFinal=12300, Fecha=DateTime.Now.AddMinutes(-95) },
                    new() { IdVenta=1038, ClienteNombre="Roberto Sánchez",  TotalFinal=6700,  Fecha=DateTime.Now.AddMinutes(-140)},
                };

                ProductosCriticosList = new ObservableCollection<ProductoCriticoDash>
                {
                    new() { Nombre="Cable HDMI 2m",     StockActual=1, StockMinimo=5  },
                    new() { Nombre="Mouse Inalámbrico", StockActual=0, StockMinimo=3  },
                    new() { Nombre="Papel A4 Resma",    StockActual=2, StockMinimo=10 },
                    new() { Nombre="Hub USB 4 puertos", StockActual=1, StockMinimo=2  },
                    new() { Nombre="Webcam HD 1080p",   StockActual=0, StockMinimo=4  },
                };

                NotifyOfPropertyChange(() => VentasRecientes);
                NotifyOfPropertyChange(() => ProductosCriticosList);
                NotifyOfPropertyChange(() => ResultadoNeto);
                NotifyOfPropertyChange(() => CrecimientoMes);
                NotifyOfPropertyChange(() => CrecimientoPositivo);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Navegación rápida ─────────────────────────────────────────────────
        public async void IrVentasCompleto() => await Shell.IrVentas();
        public async void IrProductosStock() => await Shell.IrProductos();
        public async void IrCompras()        => await Shell.IrCompras();
        public async void IrReportes()       => await Shell.IrReportes();
    }

    // ── DTO auxiliar ─────────────────────────────────────────────────────────
    public class ProductoCriticoDash
    {
        public string Nombre      { get; set; } = string.Empty;
        public int    StockActual  { get; set; }
        public int    StockMinimo  { get; set; }
    }
}
