using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Main
{
    public class DashboardViewModel : NavigableViewModel
    {
        private readonly IVentaServicio    _ventaServicio;
        private readonly IProductoServicio _productoServicio;
        private readonly ICompraServicio   _compraServicio;
        private readonly ICajaServicio     _cajaServicio;
        private readonly SesionServicio    _sesion;

        private ShellViewModel Shell => IoC.Get<ShellViewModel>();

        public string UsuarioNombre => Shell?.UsuarioNombre ?? "";
        public string UsuarioRol    => Shell?.UsuarioRol    ?? "";
        public string FechaHoy      => DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy");

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

        private bool _cajaAbierta;
        public bool CajaAbierta
        {
            get => _cajaAbierta;
            set { _cajaAbierta = value; NotifyOfPropertyChange(() => CajaAbierta); NotifyOfPropertyChange(() => CajaCerrada); }
        }
        public bool CajaCerrada => !CajaAbierta;

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<VentaResumenDto>     VentasRecientes       { get; set; } = new();
        public ObservableCollection<ProductoCriticoDash> ProductosCriticosList { get; set; } = new();

        // ── Constructor ───────────────────────────────────────────────────────
        public DashboardViewModel(
            IVentaServicio    ventaServicio,
            IProductoServicio productoServicio,
            ICompraServicio   compraServicio,
            ICajaServicio     cajaServicio,
            SesionServicio    sesion)
        {
            _ventaServicio    = ventaServicio;
            _productoServicio = productoServicio;
            _compraServicio   = compraServicio;
            _cajaServicio     = cajaServicio;
            _sesion           = sesion;
            Titulo            = "Dashboard";
        }

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
            var hoy               = DateTime.Today;
            var inicioMes         = new DateTime(hoy.Year, hoy.Month, 1);
            var inicioMesAnterior = inicioMes.AddMonths(-1);
            var finMesAnterior    = inicioMes.AddDays(-1);

            var ventasMes = (await _ventaServicio.ObtenerPorSucursalAsync(
                _sesion.IdSucursal, inicioMes, hoy)).ToList();

            TotalVentasMes = ventasMes.Sum(v => v.TotalFinal);
            TicketPromedio = ventasMes.Any() ? TotalVentasMes / ventasMes.Count : 0;

            var ventasMesAnterior = await _ventaServicio.ObtenerPorSucursalAsync(
                _sesion.IdSucursal, inicioMesAnterior, finMesAnterior);
            TotalVentasMesAnterior = ventasMesAnterior.Sum(v => v.TotalFinal);

            var comprasMes = (await _compraServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal))
                .Where(c => c.Fecha >= inicioMes && c.Fecha <= hoy).ToList();
            TotalComprasMes = comprasMes.Sum(c => c.Total);

            MargenBrutoMes = TotalVentasMes > 0
                ? (double)(ResultadoNeto / TotalVentasMes * 100) : 0;

            VentasRecientes = new ObservableCollection<VentaResumenDto>(
                ventasMes.OrderByDescending(v => v.Fecha).Take(5));

            NotifyOfPropertyChange(() => VentasRecientes);
            NotifyOfPropertyChange(() => ResultadoNeto);
            NotifyOfPropertyChange(() => CrecimientoMes);
            NotifyOfPropertyChange(() => CrecimientoPositivo);
        }

        private async Task CargarAdministrador()
        {
            var hoy       = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            var ventasMes = (await _ventaServicio.ObtenerPorSucursalAsync(
                _sesion.IdSucursal, inicioMes, hoy)).ToList();
            CantidadVentasMes = ventasMes.Count;
            VentasPendientes  = ventasMes.Count(v => v.Estado == "Pendiente");

            var stockCritico = (await _productoServicio.ObtenerStockCriticoAsync(_sesion.IdEmpresa)).ToList();
            ProductosCriticos = stockCritico.Count;
            ProductosCriticosList = new ObservableCollection<ProductoCriticoDash>(
                stockCritico.Take(8).Select(p => new ProductoCriticoDash
                {
                    Nombre      = p.Nombre,
                    StockActual = p.StockActual,
                    StockMinimo = p.StockMinimo,
                }));

            var comprasMes = (await _compraServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal))
                .Where(c => c.Fecha >= inicioMes && c.Fecha <= hoy).ToList();
            ComprasDelMes = comprasMes.Count;

            VentasRecientes = new ObservableCollection<VentaResumenDto>(
                ventasMes.OrderByDescending(v => v.Fecha).Take(5));

            NotifyOfPropertyChange(() => ProductosCriticosList);
            NotifyOfPropertyChange(() => VentasRecientes);
        }

        private async Task CargarVendedor()
        {
            var hoy = DateTime.Today;

            // Caja abierta
            var caja = await _cajaServicio.ObtenerCajaAbiertaAsync(_sesion.IdSucursal);
            if (caja != null)
            {
                CajaAbierta      = true;
                SaldoCaja        = (decimal)caja.MontoFinal;
                HoraAperturaCaja = caja.FechaApertura.ToString("HH:mm");
            }
            else
            {
                CajaAbierta      = false;
                SaldoCaja        = 0;
                HoraAperturaCaja = "--:--";
            }

            // Ventas de hoy del vendedor
            var ventasHoy = (await _ventaServicio.ObtenerPorSucursalAsync(
                    _sesion.IdSucursal, hoy, hoy))
                .Where(v => v.UsuarioNombre.Contains(
                    _sesion.Nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();

            MisVentasHoy        = ventasHoy.Sum(v => v.TotalFinal);
            MiCantidadVentasHoy = ventasHoy.Count;

            // Últimas ventas propias (7 días)
            var ventasSemana = await _ventaServicio.ObtenerPorSucursalAsync(
                _sesion.IdSucursal, hoy.AddDays(-7), hoy);
            VentasRecientes = new ObservableCollection<VentaResumenDto>(
                ventasSemana.OrderByDescending(v => v.Fecha).Take(5));

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
