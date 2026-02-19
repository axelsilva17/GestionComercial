using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class VentaListadoViewModel : NavigableViewModel
    {
        public VentaListadoViewModel()
        {
            Titulo    = "Ventas";
            Subtitulo = "Historial de ventas";
            Ventas    = new ObservableCollection<VentaResumenDto>();
        }

        // ── Métricas ──────────────────────────────────────────────────────────
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

        // ── Filtros ───────────────────────────────────────────────────────────
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }

        private string _filtroEstado = string.Empty;
        public string FiltroEstado
        {
            get => _filtroEstado;
            set { _filtroEstado = value; NotifyOfPropertyChange(() => FiltroEstado); }
        }

        // ── Paginación ────────────────────────────────────────────────────────
        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set
            {
                _paginaActual = value;
                NotifyOfPropertyChange(() => PaginaActual);
                NotifyOfPropertyChange(() => CanPaginaAnterior);
                NotifyOfPropertyChange(() => CanPaginaSiguiente);
            }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set
            {
                _totalPaginas = value;
                NotifyOfPropertyChange(() => TotalPaginas);
                NotifyOfPropertyChange(() => CanPaginaSiguiente);
            }
        }

        private int _ventasMostradas;
        public int VentasMostradas
        {
            get => _ventasMostradas;
            set { _ventasMostradas = value; NotifyOfPropertyChange(() => VentasMostradas); }
        }

        private int _totalVentas;
        public int TotalVentas
        {
            get => _totalVentas;
            set { _totalVentas = value; NotifyOfPropertyChange(() => TotalVentas); }
        }

        // ── Venta seleccionada ────────────────────────────────────────────────
        private VentaResumenDto _ventaSeleccionada;
        public VentaResumenDto VentaSeleccionada
        {
            get => _ventaSeleccionada;
            set { _ventaSeleccionada = value; NotifyOfPropertyChange(() => VentaSeleccionada); }
        }

        public ObservableCollection<VentaResumenDto> Ventas { get; set; }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task NuevaVenta()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaViewModel>(), CancellationToken.None);
        }

        public async Task Buscar()
        {
            // TODO: cargar desde BD con filtros
            await Task.CompletedTask;
        }

        public void CerrarDetalle() => VentaSeleccionada = null;
        public void VerDetalle()    { /* TODO: navegar a VentaDetalleViewModel */ }
        public void ImprimirVenta() { /* TODO: servicio de impresión */ }
        public void CancelarVenta() { /* TODO: confirmar y cancelar */ }

        public bool CanPaginaAnterior  => PaginaActual > 1;
        public bool CanPaginaSiguiente => PaginaActual < TotalPaginas;

        public void PaginaAnterior()
        {
            if (PaginaActual > 1) PaginaActual--;
        }

        public void PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas) PaginaActual++;
        }
    }
}
