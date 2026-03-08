using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReporteStockCriticoDto
    {
        public string Nombre      { get; set; } = string.Empty;
        public int    StockActual { get; set; }
        public int    StockMinimo { get; set; }
        public string Estado      => StockActual == 0 ? "Sin stock" : "Crítico";
    }

    public class ReporteCompraRecienteDto
    {
        public string  Proveedor { get; set; } = string.Empty;
        public string  Fecha     { get; set; } = string.Empty;
        public decimal Total     { get; set; }
        public int     Productos { get; set; }
    }

    /// <summary>
    /// Reporte operativo para Administrador.
    /// Incluye: ventas del mes (sin desglose financiero), stock crítico, compras recientes.
    /// Sin márgenes ni resultado neto — esos datos son exclusivos del Gerente.
    /// TODO: Conectar con IVentaServicio, IProductoServicio, ICompraServicio.
    /// </summary>
    public class ReporteAdminViewModel : NavigableViewModel
    {
        public ReporteAdminViewModel()
        {
            Titulo    = "Reportes";
            Subtitulo = "Administración — operaciones";
        }

        // ── KPIs operativos ───────────────────────────────────────────────────
        private int _cantidadVentasMes;
        public int CantidadVentasMes
        {
            get => _cantidadVentasMes;
            set { _cantidadVentasMes = value; NotifyOfPropertyChange(() => CantidadVentasMes); }
        }

        private int _productosStockCritico;
        public int ProductosStockCritico
        {
            get => _productosStockCritico;
            set { _productosStockCritico = value; NotifyOfPropertyChange(() => ProductosStockCritico); }
        }

        private int _comprasDelMes;
        public int ComprasDelMes
        {
            get => _comprasDelMes;
            set { _comprasDelMes = value; NotifyOfPropertyChange(() => ComprasDelMes); }
        }

        private int _clientesNuevos;
        public int ClientesNuevos
        {
            get => _clientesNuevos;
            set { _clientesNuevos = value; NotifyOfPropertyChange(() => ClientesNuevos); }
        }

        private int _ventasPendientes;
        public int VentasPendientes
        {
            get => _ventasPendientes;
            set { _ventasPendientes = value; NotifyOfPropertyChange(() => VentasPendientes); }
        }

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<ReporteStockCriticoDto>   StockCritico     { get; set; } = new();
        public ObservableCollection<ReporteCompraRecienteDto> ComprasRecientes { get; set; } = new();

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
                // var resumen = await _reporteServicio.ObtenerResumenAdminAsync();
                // CantidadVentasMes     = resumen.CantidadVentasMes;
                // ProductosStockCritico = resumen.ProductosStockCritico;
                // ComprasDelMes         = resumen.ComprasDelMes;
                // ClientesNuevos        = resumen.ClientesNuevos;
                // VentasPendientes      = resumen.VentasPendientes;
                // StockCritico          = new ObservableCollection<ReporteStockCriticoDto>(resumen.StockCritico);
                // ComprasRecientes      = new ObservableCollection<ReporteCompraRecienteDto>(resumen.ComprasRecientes);

                await Task.CompletedTask;

                NotifyOfPropertyChange(() => StockCritico);
                NotifyOfPropertyChange(() => ComprasRecientes);
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Actualizar() => await CargarAsync();
    }
}
