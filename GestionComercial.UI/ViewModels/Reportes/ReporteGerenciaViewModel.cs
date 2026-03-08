using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteVentaMensualDto
    {
        public string  Mes       { get; set; } = string.Empty;
        public decimal Ventas    { get; set; }
        public decimal Compras   { get; set; }
        public decimal Resultado { get; set; }
        public double  Margen    { get; set; }
    }

    public class ReporteProductoTopDto
    {
        public string  Nombre   { get; set; } = string.Empty;
        public int     Cantidad { get; set; }
        public decimal Ingresos { get; set; }
        public double  Margen   { get; set; }
    }

    public class ReporteVendedorDto
    {
        public string  Nombre       { get; set; } = string.Empty;
        public int     Ventas       { get; set; }
        public decimal Total        { get; set; }
        public decimal TicketProm   { get; set; }
        public int     IdUsuario    { get; set; }
        public string  UsuarioNombre { get; set; } = string.Empty;
        public string  Sucursal     { get; set; } = string.Empty;
        public int     CantidadVentas { get; set; }
        public decimal TotalVendido { get; set; }
        public decimal PromedioVenta { get; set; }
        public decimal TotalDescuentos { get; set; }
    }

    /// <summary>
    /// Reporte completo para Gerente.
    /// Incluye: ventas vs compras por mes, margen, top productos, rendimiento vendedores.
    /// TODO: Conectar con IVentaServicio, ICompraServicio, IProductoServicio.
    /// </summary>
    public class ReporteGerenciaViewModel : NavigableViewModel
    {
        public ReporteGerenciaViewModel()
        {
            Titulo    = "Reportes";
            Subtitulo = "Gerencia — visión ejecutiva";
        }

        // ── KPIs resumen ──────────────────────────────────────────────────────
        private decimal _ventasAcumuladas;
        public decimal VentasAcumuladas
        {
            get => _ventasAcumuladas;
            set { _ventasAcumuladas = value; NotifyOfPropertyChange(() => VentasAcumuladas); NotifyOfPropertyChange(() => ResultadoAcumulado); }
        }

        private decimal _comprasAcumuladas;
        public decimal ComprasAcumuladas
        {
            get => _comprasAcumuladas;
            set { _comprasAcumuladas = value; NotifyOfPropertyChange(() => ComprasAcumuladas); NotifyOfPropertyChange(() => ResultadoAcumulado); }
        }

        private double _margenPromedio;
        public double MargenPromedio
        {
            get => _margenPromedio;
            set { _margenPromedio = value; NotifyOfPropertyChange(() => MargenPromedio); }
        }

        private int _clientesNuevos;
        public int ClientesNuevos
        {
            get => _clientesNuevos;
            set { _clientesNuevos = value; NotifyOfPropertyChange(() => ClientesNuevos); }
        }

        public decimal ResultadoAcumulado => VentasAcumuladas - ComprasAcumuladas;

        // ── Periodo ───────────────────────────────────────────────────────────
        private int _anioSeleccionado = DateTime.Now.Year;
        public int AnioSeleccionado
        {
            get => _anioSeleccionado;
            set { _anioSeleccionado = value; NotifyOfPropertyChange(() => AnioSeleccionado); }
        }

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<ReporteVentaMensualDto> VentasMensuales { get; set; } = new();
        public ObservableCollection<ReporteProductoTopDto>  TopProductos    { get; set; } = new();
        public ObservableCollection<ReporteVendedorDto>     Vendedores      { get; set; } = new();

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // TODO: reemplazar con llamadas reales a servicios
                // Ejemplo:
                // var resumen = await _reporteServicio.ObtenerResumenGerenciaAsync(AnioSeleccionado);
                // VentasAcumuladas  = resumen.VentasAcumuladas;
                // ComprasAcumuladas = resumen.ComprasAcumuladas;
                // MargenPromedio    = resumen.MargenPromedio;
                // ClientesNuevos    = resumen.ClientesNuevos;
                // VentasMensuales   = new ObservableCollection<ReporteVentaMensualDto>(resumen.VentasMensuales);
                // TopProductos      = new ObservableCollection<ReporteProductoTopDto>(resumen.TopProductos);
                // Vendedores        = new ObservableCollection<ReporteVendedorDto>(resumen.Vendedores);

                await Task.CompletedTask;

                NotifyOfPropertyChange(() => VentasMensuales);
                NotifyOfPropertyChange(() => TopProductos);
                NotifyOfPropertyChange(() => Vendedores);
                NotifyOfPropertyChange(() => ResultadoAcumulado);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Actualizar() => await CargarAsync();
    }
}
