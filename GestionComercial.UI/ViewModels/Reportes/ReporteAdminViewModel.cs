using GestionComercial.UI.ViewModels.Base;
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
        public string Estado      => StockActual == 0 ? "Sin stock" : "Critico";
    }

    public class ReporteCompraRecienteDto
    {
        public string  Proveedor  { get; set; } = string.Empty;
        public string  Fecha      { get; set; } = string.Empty;
        public decimal Total      { get; set; }
        public int     Productos  { get; set; }
    }

    /// <summary>
    /// Reporte operativo para Administrador.
    /// Incluye: ventas del mes (sin desglose financiero), stock critico,
    /// compras recientes y clientes nuevos. Sin margenes ni resultado neto.
    /// </summary>
    public class ReporteAdminViewModel : NavigableViewModel
    {
        public ReporteAdminViewModel()
        {
            Titulo    = "Reportes";
            Subtitulo = "Administracion — operaciones";
        }

        // ── KPIs operativos (sin datos financieros sensibles) ─────────────────
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
        public ObservableCollection<ReporteStockCriticoDto>  StockCritico    { get; set; } = new();
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
                await Task.Delay(300);

                CantidadVentasMes    = 187;
                ProductosStockCritico = 5;
                ComprasDelMes        = 12;
                ClientesNuevos       = 14;
                VentasPendientes     = 3;

                StockCritico = new ObservableCollection<ReporteStockCriticoDto>
                {
                    new() { Nombre="Mouse Inalambrico",  StockActual=0, StockMinimo=3  },
                    new() { Nombre="Webcam HD 1080p",    StockActual=0, StockMinimo=4  },
                    new() { Nombre="Cable HDMI 2m",      StockActual=1, StockMinimo=5  },
                    new() { Nombre="Papel A4 Resma",     StockActual=2, StockMinimo=10 },
                    new() { Nombre="Hub USB 4 puertos",  StockActual=1, StockMinimo=2  },
                };

                ComprasRecientes = new ObservableCollection<ReporteCompraRecienteDto>
                {
                    new() { Proveedor="Tech Supplies Argentina",  Fecha="24/02/2026", Total=245_000, Productos=8  },
                    new() { Proveedor="Distribuidora Norte S.A.", Fecha="21/02/2026", Total=187_000, Productos=12 },
                    new() { Proveedor="Papelera del Litoral",     Fecha="18/02/2026", Total=98_000,  Productos=5  },
                    new() { Proveedor="Electro Import SRL",       Fecha="15/02/2026", Total=312_000, Productos=6  },
                };

                NotifyOfPropertyChange(() => StockCritico);
                NotifyOfPropertyChange(() => ComprasRecientes);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Actualizar() => await CargarAsync();
    }
}
