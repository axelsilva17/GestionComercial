using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Reportes
{
    public class ReportesViewModel : NavigableViewModel
    {
        public override string Titulo => "Reportes";
        public override string Subtitulo => "Análisis y estadísticas";

        // ── Filtros globales ─────────────────────────────────────────────────
        private DateTime _fechaDesde = DateTime.Today.AddDays(-30);
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }

        private DateTime _fechaHasta = DateTime.Today;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }

        // ── ViewModels hijos ─────────────────────────────────────────────────
        public ReportesVentasViewModel    Ventas     { get; }
        public ReporteMargenViewModel     Margen     { get; }
        public ReporteRotacionViewModel   Rotacion   { get; }
        public ReporteStockViewModel      Stock      { get; }
        public ReporteVendedoresViewModel Vendedores { get; }

        // ── Tab activo ───────────────────────────────────────────────────────
        private int _tabActivo = 0;
        public int TabActivo
        {
            get => _tabActivo;
            set { _tabActivo = value; NotifyOfPropertyChange(() => TabActivo); }
        }

        public ReportesViewModel(
            ReportesVentasViewModel    ventas,
            ReporteMargenViewModel     margen,
            ReporteRotacionViewModel   rotacion,
            ReporteStockViewModel      stock,
            ReporteVendedoresViewModel vendedores)
        {
            Ventas     = ventas;
            Margen     = margen;
            Rotacion   = rotacion;
            Stock      = stock;
            Vendedores = vendedores;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await AplicarFiltros();

        public async Task AplicarFiltros()
        {
            IsLoading = true;
            try
            {
                Ventas.FechaDesde     = FechaDesde; Ventas.FechaHasta     = FechaHasta;
                Margen.FechaDesde     = FechaDesde; Margen.FechaHasta     = FechaHasta;
                Rotacion.FechaDesde   = FechaDesde; Rotacion.FechaHasta   = FechaHasta;
                Stock.FechaDesde      = FechaDesde; Stock.FechaHasta      = FechaHasta;
                Vendedores.FechaDesde = FechaDesde; Vendedores.FechaHasta = FechaHasta;

                await Task.WhenAll(
                    Ventas.CargarAsync(),
                    Margen.CargarAsync(),
                    Rotacion.CargarAsync(),
                    Stock.CargarAsync(),
                    Vendedores.CargarAsync()
                );
            }
            finally { IsLoading = false; }
        }

        public void SeleccionarUltimos7Dias()  { FechaDesde = DateTime.Today.AddDays(-7);  FechaHasta = DateTime.Today; }
        public void SeleccionarUltimos30Dias() { FechaDesde = DateTime.Today.AddDays(-30); FechaHasta = DateTime.Today; }
        public void SeleccionarEsteMes()       { FechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); FechaHasta = DateTime.Today; }

        public async Task ExportarExcel()
        {
            IsLoading = true;
            await Task.Delay(500); // TODO: ClosedXML según TabActivo
            IsLoading = false;
        }
    }
}
