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
    /// <summary>
    /// Dashboard unificado con métricas diferenciadas por rol.
    ///
    /// GERENTE:        ventas del mes, resultado neto, margen, crecimiento vs mes anterior
    /// ADMINISTRADOR:  stock crítico, compras del mes, cantidad ventas, productos a reponer
    /// VENDEDOR:       sus propias ventas del día, saldo de caja, últimas ventas realizadas
    ///
    /// TODO: Inyectar IVentaServicio, IProductoServicio, ICajaServicio y conectar CargarGerente/Admin/Vendedor.
    /// </summary>
    public class DashboardViewModel : NavigableViewModel
    {
        private ShellViewModel Shell => IoC.Get<ShellViewModel>();

        public string UsuarioNombre => Shell?.UsuarioNombre ?? "";
        public string UsuarioRol    => Shell?.UsuarioRol    ?? "";
        public string FechaHoy      => DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");

        // ── Visibilidad secciones por rol ─────────────────────────────────────
        public bool MostrarSeccionGerente       => Shell?.EsGerente       ?? false;
        public bool MostrarSeccionAdministrador => Shell?.EsAdministrador ?? false;
        public bool MostrarSeccionVendedor      => Shell?.EsVendedor      ?? false;

        public string TituloSaludo => Shell?.Rol switch
        {
            RolUsuario.Gerente       => "Resumen ejecutivo",
            RolUsuario.Administrador => "Panel de administración",
            _                        => "Panel de ventas",
        } ?? "";

        // ── KPIs Gerente ──────────────────────────────────────────────────────
        private decimal _totalVentasMes;
        public decimal TotalVentasMes
        {
            get => _totalVentasMes;
            set { _totalVentasMes = value; NotifyOfPropertyChange(() => TotalVentasMes); NotifyOfPropertyChange(() => ResultadoNeto); }
        }

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

        private decimal _totalVentasMesAnterior;
        public decimal TotalVentasMesAnterior
        {
            get => _totalVentasMesAnterior;
            set
            {
                _totalVentasMesAnterior = value;
                NotifyOfPropertyChange(() => TotalVentasMesAnterior);
                NotifyOfPropertyChange(() => CrecimientoMes);
                NotifyOfPropertyChange(() => CrecimientoPositivo);
            }
        }

        private decimal _ticketPromedio;
        public decimal TicketPromedio
        {
            get => _ticketPromedio;
            set { _ticketPromedio = value; NotifyOfPropertyChange(() => TicketPromedio); }
        }

        public decimal ResultadoNeto    => TotalVentasMes - TotalComprasMes;
        public double  CrecimientoMes   => TotalVentasMesAnterior > 0
            ? (double)((TotalVentasMes - TotalVentasMesAnterior) / TotalVentasMesAnterior * 100)
            : 0;
        public bool CrecimientoPositivo => CrecimientoMes >= 0;

        // ── KPIs Administrador ────────────────────────────────────────────────
        private int _cantidadVentasMes;
        public int CantidadVentasMes
        {
            get => _cantidadVentasMes;
            set { _cantidadVentasMes = value; NotifyOfPropertyChange(() => CantidadVentasMes); }
        }

        private int _productosCriticos;
        public int ProductosCriticos
        {
            get => _productosCriticos;
            set { _productosCriticos = value; NotifyOfPropertyChange(() => ProductosCriticos); }
        }

        private int _comprasDelMes;
        public int ComprasDelMes
        {
            get => _comprasDelMes;
            set { _comprasDelMes = value; NotifyOfPropertyChange(() => ComprasDelMes); }
        }

        private int _ventasPendientes;
        public int VentasPendientes
        {
            get => _ventasPendientes;
            set { _ventasPendientes = value; NotifyOfPropertyChange(() => VentasPendientes); }
        }

        // ── KPIs Vendedor ─────────────────────────────────────────────────────
        private decimal _misVentasHoy;
        public decimal MisVentasHoy
        {
            get => _misVentasHoy;
            set { _misVentasHoy = value; NotifyOfPropertyChange(() => MisVentasHoy); }
        }

        private int _miCantidadVentasHoy;
        public int MiCantidadVentasHoy
        {
            get => _miCantidadVentasHoy;
            set { _miCantidadVentasHoy = value; NotifyOfPropertyChange(() => MiCantidadVentasHoy); }
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

        // ── Listas compartidas ────────────────────────────────────────────────
        public ObservableCollection<VentaResumenDto>     VentasRecientes       { get; set; } = new();
        public ObservableCollection<ProductoCriticoDash> ProductosCriticosList { get; set; } = new();

        // ── Lifecycle ─────────────────────────────────────────────────────────
        public DashboardViewModel() { Titulo = "Dashboard"; }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            NotifyOfPropertyChange(() => MostrarSeccionGerente);
            NotifyOfPropertyChange(() => MostrarSeccionAdministrador);
            NotifyOfPropertyChange(() => MostrarSeccionVendedor);
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
                if (Shell?.EsGerente ?? false)
                    await CargarGerente();
                else if (Shell?.EsAdministrador ?? false)
                    await CargarAdministrador();
                else
                    await CargarVendedor();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        private async Task CargarGerente()
        {
            // TODO: var kpis = await _ventaServicio.ObtenerKpisGerenciaAsync(Shell.IdEmpresaActual);
            // TotalVentasMes         = kpis.TotalVentasMes;
            // TotalComprasMes        = kpis.TotalComprasMes;
            // MargenBrutoMes         = kpis.MargenBrutoMes;
            // TotalVentasMesAnterior = kpis.TotalVentasMesAnterior;
            // TicketPromedio         = kpis.TicketPromedio;
            // VentasRecientes        = new ObservableCollection<VentaResumenDto>(kpis.UltimasVentas);

            await Task.CompletedTask;
            NotifyOfPropertyChange(() => VentasRecientes);
            NotifyOfPropertyChange(() => ResultadoNeto);
            NotifyOfPropertyChange(() => CrecimientoMes);
            NotifyOfPropertyChange(() => CrecimientoPositivo);
        }

        private async Task CargarAdministrador()
        {
            // TODO: var kpis = await _productoServicio.ObtenerKpisAdminAsync(Shell.IdSucursalActual);
            // CantidadVentasMes     = kpis.CantidadVentasMes;
            // ProductosCriticos     = kpis.ProductosCriticos;
            // ComprasDelMes         = kpis.ComprasDelMes;
            // VentasPendientes      = kpis.VentasPendientes;
            // ProductosCriticosList = new ObservableCollection<ProductoCriticoDash>(kpis.ProductosCriticosList);

            await Task.CompletedTask;
            NotifyOfPropertyChange(() => ProductosCriticosList);
        }

        private async Task CargarVendedor()
        {
            // TODO: var kpis = await _ventaServicio.ObtenerKpisVendedorAsync(Shell.SesionActual.IdUsuario, Shell.IdSucursalActual);
            // MisVentasHoy        = kpis.MisVentasHoy;
            // MiCantidadVentasHoy = kpis.MiCantidadVentasHoy;
            // SaldoCaja           = kpis.SaldoCaja;
            // HoraAperturaCaja    = kpis.HoraAperturaCaja;
            // VentasRecientes     = new ObservableCollection<VentaResumenDto>(kpis.MisUltimasVentas);

            await Task.CompletedTask;
            NotifyOfPropertyChange(() => VentasRecientes);
        }

        // ── Navegación rápida ─────────────────────────────────────────────────
        public async void IrVentasCompleto() => await Shell.IrVentas();
        public async void IrProductosStock() => await Shell.IrProductos();
        public async void IrCompras()        => await Shell.IrCompras();
        public async void IrReportes()       => await Shell.IrReportes();
    }

    public class ProductoCriticoDash
    {
        public string Nombre      { get; set; } = string.Empty;
        public int    StockActual { get; set; }
        public int    StockMinimo { get; set; }
    }
}
