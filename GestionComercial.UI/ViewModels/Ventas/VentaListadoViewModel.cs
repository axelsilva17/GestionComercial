using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class VentaListadoViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        private decimal _totalVentasHoy;
        private int _cantidadVentasHoy;
        private decimal _totalVentasMes;
        private int _cantidadVentasMes;
        private int _ventasPendientes;
        private int _ventasCanceladas;
        private string _textoBusqueda;
        private DateTime? _fechaDesde;
        private DateTime? _fechaHasta;
        private string _filtroEstado;
        private int _paginaActual = 1;
        private int _totalPaginas = 1;
        private int _ventasMostradas;
        private int _totalVentas;
        private VentaResumenDto _ventaSeleccionada;

        public VentaListadoViewModel(ShellViewModel shell)
        {
            _shell = shell;
            Titulo = "Ventas";
            Subtitulo = "Historial de ventas";
            Ventas = new ObservableCollection<VentaResumenDto>();
        }

        // ── Métricas ──────────────────────────────────────────────────
        public decimal TotalVentasHoy
        {
            get => _totalVentasHoy;
            set { _totalVentasHoy = value; NotifyOfPropertyChange(() => TotalVentasHoy); }
        }
        public int CantidadVentasHoy
        {
            get => _cantidadVentasHoy;
            set { _cantidadVentasHoy = value; NotifyOfPropertyChange(() => CantidadVentasHoy); }
        }
        public decimal TotalVentasMes
        {
            get => _totalVentasMes;
            set { _totalVentasMes = value; NotifyOfPropertyChange(() => TotalVentasMes); }
        }
        public int CantidadVentasMes
        {
            get => _cantidadVentasMes;
            set { _cantidadVentasMes = value; NotifyOfPropertyChange(() => CantidadVentasMes); }
        }
        public int VentasPendientes
        {
            get => _ventasPendientes;
            set { _ventasPendientes = value; NotifyOfPropertyChange(() => VentasPendientes); }
        }
        public int VentasCanceladas
        {
            get => _ventasCanceladas;
            set { _ventasCanceladas = value; NotifyOfPropertyChange(() => VentasCanceladas); }
        }

        // ── Filtros ───────────────────────────────────────────────────
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }
        public string FiltroEstado
        {
            get => _filtroEstado;
            set { _filtroEstado = value; NotifyOfPropertyChange(() => FiltroEstado); }
        }

        // ── Paginación ────────────────────────────────────────────────
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
        public int VentasMostradas
        {
            get => _ventasMostradas;
            set { _ventasMostradas = value; NotifyOfPropertyChange(() => VentasMostradas); }
        }
        public int TotalVentas
        {
            get => _totalVentas;
            set { _totalVentas = value; NotifyOfPropertyChange(() => TotalVentas); }
        }

        // ── Sidebar — venta seleccionada ──────────────────────────────
        public VentaResumenDto VentaSeleccionada
        {
            get => _ventaSeleccionada;
            set
            {
                _ventaSeleccionada = value;
                NotifyOfPropertyChange(() => VentaSeleccionada);
            }
        }

        public ObservableCollection<VentaResumenDto> Ventas { get; set; }

        // ── Comandos ──────────────────────────────────────────────────
        public void Buscar() { /* TODO: cargar desde BD con filtros */ }

        public async void NuevaVenta()
        {
            var vm = IoC.Get<VentaViewModel>();
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public void CerrarDetalle() => VentaSeleccionada = null;

        public void VerDetalle() { /* TODO: navegar a VentaDetalleViewModel */ }

        public void ImprimirVenta() { /* TODO: servicio de impresión */ }

        public void CancelarVenta() { /* TODO: confirmar y cancelar */ }

        public bool CanPaginaAnterior => PaginaActual > 1;
        public void PaginaAnterior()
        {
            if (PaginaActual > 1) PaginaActual--;
        }

        public bool CanPaginaSiguiente => PaginaActual < TotalPaginas;
        public void PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas) PaginaActual++;
        }
    }

    public class VentaResumenDto
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteInicial => string.IsNullOrEmpty(ClienteNombre) ? "?" : ClienteNombre[0].ToString().ToUpper();
        public string UsuarioNombre { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal TotalDescuento { get; set; }
        public decimal TotalFinal { get; set; }
        public string Estado { get; set; }
    }
}
