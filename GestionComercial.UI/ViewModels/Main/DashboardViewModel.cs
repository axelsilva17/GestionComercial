using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Reportes;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Enumeraciones;
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
        private readonly IReporteServicio  _reporteServicio;
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
            IReporteServicio  reporteServicio,
            SesionServicio    sesion)
        {
            _ventaServicio    = ventaServicio;
            _productoServicio = productoServicio;
            _compraServicio   = compraServicio;
            _cajaServicio     = cajaServicio;
            _reporteServicio  = reporteServicio;
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

            // KPIs via IReporteServicio (SQL aggregation)
            var kpis = await _reporteServicio.KpisVentasBaseAsync(_sesion.IdEmpresa, _sesion.IdSucursal, inicioMes, hoy.AddDays(1));
            if (kpis != null)
            {
                TotalVentasMes = kpis.TotalVentasPeriodo;
                CantidadVentasMes = kpis.TotalTransacciones;
                TicketPromedio = kpis.TicketPromedio;
            }

            var kpisAnterior = await _reporteServicio.KpisVentasBaseAsync(_sesion.IdEmpresa, _sesion.IdSucursal, inicioMesAnterior, finMesAnterior);
            if (kpisAnterior != null)
            {
                TotalVentasMesAnterior = kpisAnterior.TotalVentasPeriodo;
            }

            var metricasCompras = await _compraServicio.ObtenerMetricasComprasAsync(_sesion.IdSucursal, inicioMes, hoy);
            if (metricasCompras != null)
            {
                TotalComprasMes = metricasCompras.Total;
            }

            MargenBrutoMes = TotalVentasMes > 0
                ? (double)(ResultadoNeto / TotalVentasMes * 100) : 0;

            // Recent individual sales (last 7 days) — Pagada only (exclude Pendiente/Anulada)
            var ventasRecientes = await _ventaServicio.ObtenerVentasAsync(
                _sesion.IdSucursal, hoy.AddDays(-7), hoy.AddDays(1), estado: (int)EstadoVentaEnum.Pagada);
            VentasRecientes = new ObservableCollection<VentaResumenDto>(
                ventasRecientes.OrderByDescending(v => v.Fecha).Take(5));

            NotifyOfPropertyChange(() => VentasRecientes);
            NotifyOfPropertyChange(() => ResultadoNeto);
            NotifyOfPropertyChange(() => CrecimientoMes);
            NotifyOfPropertyChange(() => CrecimientoPositivo);
        }

        private async Task CargarAdministrador()
        {
            var hoy       = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            // KPIs via IReporteServicio
            var kpis = await _reporteServicio.KpisVentasBaseAsync(_sesion.IdEmpresa, _sesion.IdSucursal, inicioMes, hoy.AddDays(1));
            if (kpis != null)
            {
                CantidadVentasMes = kpis.TotalTransacciones;
            }

            // Stock crítico via IReporteServicio.StockCriticoAsync
            var stockCritico = await _reporteServicio.StockCriticoAsync(_sesion.IdEmpresa);
            var criticos = stockCritico.Take(8).ToList();
            ProductosCriticos = stockCritico.Count();
            ProductosCriticosList = new ObservableCollection<ProductoCriticoDash>(
                criticos.Select(p => new ProductoCriticoDash
                {
                    Nombre      = p.ProductoNombre,
                    StockActual = p.StockActual,
                    StockMinimo = p.StockMinimo,
                }));

            var metricasCompras = await _compraServicio.ObtenerMetricasComprasAsync(_sesion.IdSucursal, inicioMes, hoy);
            if (metricasCompras != null)
            {
                ComprasDelMes = metricasCompras.Count;
            }

            // Recent individual sales (last 7 days) — Pagada only (exclude Pendiente/Anulada)
            var ventasRecientes = await _ventaServicio.ObtenerVentasAsync(
                _sesion.IdSucursal, hoy.AddDays(-7), hoy.AddDays(1), estado: (int)EstadoVentaEnum.Pagada);
            VentasRecientes = new ObservableCollection<VentaResumenDto>(
                ventasRecientes.OrderByDescending(v => v.Fecha).Take(5));

            NotifyOfPropertyChange(() => ProductosCriticosList);
            NotifyOfPropertyChange(() => VentasRecientes);
        }

        private async Task CargarVendedor()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[DashboardVendedor] Iniciando carga...");
                
                var hoy = DateTime.Today;
                System.Diagnostics.Debug.WriteLine($"[DashboardVendedor] Fecha: {hoy}");

                // Caja abierta
                var caja = await _cajaServicio.ObtenerCajaAbiertaAsync(_sesion.IdSucursal);
                System.Diagnostics.Debug.WriteLine($"[DashboardVendedor] Caja consultada, resultado: {caja?.Id ?? 0}");
                
                if (caja != null)
                {
                    CajaAbierta      = true;
                    SaldoCaja        = caja.MontoFinal ?? caja.MontoInicial;
                    HoraAperturaCaja = caja.FechaApertura.ToString("HH:mm");
                }
                else
                {
                    CajaAbierta      = false;
                    SaldoCaja        = 0;
                    HoraAperturaCaja = "--:--";
                }

                // Ventas de hoy del vendedor via IVentaServicio.ObtenerVentasPorVendedorAsync (nuevo método SQL)
                var ventasHoy = await _ventaServicio.ObtenerVentasPorVendedorAsync(_sesion.IdSucursal, _sesion.IdUsuario, hoy, hoy.AddDays(1));

                MisVentasHoy        = ventasHoy.Sum(v => v.TotalFinal);
                MiCantidadVentasHoy = ventasHoy.Count();

                // Últimas ventas propias (7 días)
                var ventasSemana = await _ventaServicio.ObtenerVentasPorVendedorAsync(_sesion.IdSucursal, _sesion.IdUsuario, hoy.AddDays(-7), hoy);
                VentasRecientes = new ObservableCollection<VentaResumenDto>(
                    ventasSemana.OrderByDescending(v => v.Fecha).Take(5));

                NotifyOfPropertyChange(() => VentasRecientes);
                System.Diagnostics.Debug.WriteLine("[DashboardVendedor] Carga completada exitosamente");
            }
            catch (InvalidOperationException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardVendedor] ERROR InvalidOperationException: {ex.Message}");
                throw new Exception($"Error al cargar datos del vendedor: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardVendedor] ERROR: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        // ── Navegación rápida ─────────────────────────────────────────────────
        public async void IrVentasCompleto() { try { await Shell.IrVentas(); } catch (Exception ex) { MostrarError(ex.Message); } }
        public async void IrProductosStock() { try { await Shell.IrProductosStockCritico(); } catch (Exception ex) { MostrarError(ex.Message); } }
        public async void IrCompras()        { try { await Shell.IrCompras(); } catch (Exception ex) { MostrarError(ex.Message); } }
        public async void IrReportes()       { try { await Shell.IrReportes(); } catch (Exception ex) { MostrarError(ex.Message); } }
    }

    public class ProductoCriticoDash
    {
        public string Nombre      { get; set; } = string.Empty;
        public int    StockActual { get; set; }
        public int    StockMinimo { get; set; }
    }
}