using GestionComercial.UI.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    // ── DTOs locales ─────────────────────────────────────────────────────────
    public class ReporteVentaMensualDto
    {
        public string Mes         { get; set; } = string.Empty;
        public decimal Ventas     { get; set; }
        public decimal Compras    { get; set; }
        public decimal Resultado  { get; set; }
        public double  Margen     { get; set; }
    }

    public class ReporteProductoTopDto
    {
        public string  Nombre      { get; set; } = string.Empty;
        public int     Cantidad    { get; set; }
        public decimal Ingresos    { get; set; }
        public double  Margen      { get; set; }
    }

    public class ReporteVendedorDto
    {
        public string  Nombre      { get; set; } = string.Empty;
        public int     Ventas      { get; set; }
        public decimal Total       { get; set; }
        public decimal TicketProm  { get; set; }
        public int IdUsuario { get; internal set; }
        public string UsuarioNombre { get; internal set; }
        public string Sucursal { get; internal set; }
        public int CantidadVentas { get; internal set; }
        public int TotalVendido { get; internal set; }
        public decimal PromedioVenta { get; internal set; }
        public int TotalDescuentos { get; internal set; }
    }

    /// <summary>
    /// Reporte completo para Gerente/Dueño.
    /// Incluye: ventas vs compras por mes, margen, top productos, rendimiento vendedores,
    /// clientes nuevos, resultado neto acumulado.
    /// </summary>
    public class ReporteGerenciaViewModel : NavigableViewModel
    {
        public ReporteGerenciaViewModel()
        {
            Titulo    = "Reportes";
            Subtitulo = "Gerencia — vision ejecutiva";
        }

        // ── KPIs resumen ──────────────────────────────────────────────────────
        private decimal _ventasAcumuladas;
        public decimal VentasAcumuladas
        {
            get => _ventasAcumuladas;
            set { _ventasAcumuladas = value; NotifyOfPropertyChange(() => VentasAcumuladas); }
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

        // ── Periodo seleccionado ──────────────────────────────────────────────
        private int _anioSeleccionado = DateTime.Now.Year;
        public int AnioSeleccionado
        {
            get => _anioSeleccionado;
            set { _anioSeleccionado = value; NotifyOfPropertyChange(() => AnioSeleccionado); }
        }

        // ── Listas ────────────────────────────────────────────────────────────
        public ObservableCollection<ReporteVentaMensualDto> VentasMensuales  { get; set; } = new();
        public ObservableCollection<ReporteProductoTopDto>  TopProductos     { get; set; } = new();
        public ObservableCollection<ReporteVendedorDto>     Vendedores       { get; set; } = new();

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

                VentasAcumuladas  = 14_280_000;
                ComprasAcumuladas = 8_650_000;
                MargenPromedio    = 39.4;
                ClientesNuevos    = 47;

                VentasMensuales = new ObservableCollection<ReporteVentaMensualDto>
                {
                    new() { Mes="Ago", Ventas=1_820_000, Compras=1_100_000, Resultado=720_000,  Margen=39.6 },
                    new() { Mes="Sep", Ventas=2_050_000, Compras=1_240_000, Resultado=810_000,  Margen=39.5 },
                    new() { Mes="Oct", Ventas=1_980_000, Compras=1_200_000, Resultado=780_000,  Margen=39.4 },
                    new() { Mes="Nov", Ventas=2_450_000, Compras=1_480_000, Resultado=970_000,  Margen=39.6 },
                    new() { Mes="Dic", Ventas=3_630_000, Compras=2_190_000, Resultado=1_440_000,Margen=39.7 },
                    new() { Mes="Ene", Ventas=2_350_000, Compras=1_440_000, Resultado=910_000,  Margen=38.7 },
                };

                TopProductos = new ObservableCollection<ReporteProductoTopDto>
                {
                    new() { Nombre="Monitor 24\"",          Cantidad=84,  Ingresos=1_680_000, Margen=42.1 },
                    new() { Nombre="Teclado Mecanico",      Cantidad=123, Ingresos=983_000,   Margen=38.5 },
                    new() { Nombre="Mouse Inalambrico",     Cantidad=201, Ingresos=804_000,   Margen=44.2 },
                    new() { Nombre="Silla Ergonomica",      Cantidad=31,  Ingresos=775_000,   Margen=35.8 },
                    new() { Nombre="Hub USB 4 puertos",     Cantidad=312, Ingresos=624_000,   Margen=41.0 },
                };

                Vendedores = new ObservableCollection<ReporteVendedorDto>
                {
                    new() { Nombre="Juan Perez",   Ventas=312, Total=3_890_000, TicketProm=12_468 },
                    new() { Nombre="Ana Martinez", Ventas=287, Total=3_580_000, TicketProm=12_474 },
                    new() { Nombre="Luis Torres",  Ventas=241, Total=2_980_000, TicketProm=12_365 },
                };

                NotifyOfPropertyChange(() => VentasMensuales);
                NotifyOfPropertyChange(() => TopProductos);
                NotifyOfPropertyChange(() => Vendedores);
                NotifyOfPropertyChange(() => ResultadoAcumulado);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Actualizar() => await CargarAsync();
    }
}
