using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
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
        private readonly IVentaServicio _ventaServicio;
        private readonly SesionServicio _sesion;

        public VentaListadoViewModel(IVentaServicio ventaServicio, SesionServicio sesion)
        {
            _ventaServicio = ventaServicio;
            _sesion        = sesion;
            Titulo         = "Ventas";
            Subtitulo      = "Historial de ventas";
            Ventas         = new ObservableCollection<VentaResumenDto>();
            FechaDesde = DateTime.Today.AddYears(-2);
            FechaHasta     = DateTime.Today;
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

        private VentaResumenDto _ventaSeleccionada;
        public VentaResumenDto VentaSeleccionada
        {
            get => _ventaSeleccionada;
            set { _ventaSeleccionada = value; NotifyOfPropertyChange(() => VentaSeleccionada); }
        }

        public ObservableCollection<VentaResumenDto> Ventas { get; set; }

        // ── Ciclo de vida ─────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarDatos();

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task NuevaVenta()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaViewModel>(), CancellationToken.None);
        }

        public async Task Buscar() => await CargarDatos();

        public void CerrarDetalle() => VentaSeleccionada = null;

        public async Task CancelarVenta()
        {
            if (VentaSeleccionada == null) return;
            IsLoading = true;
            try
            {
                await _ventaServicio.CancelarAsync(VentaSeleccionada.IdVenta);
                await CargarDatos();
            }
            catch (Exception ex)
            {
               ErrorMessage = ex.Message;
            }
            finally { IsLoading = false; }
        }

        public bool CanPaginaAnterior  => PaginaActual > 1;
        public bool CanPaginaSiguiente => PaginaActual < TotalPaginas;

        public void PaginaAnterior()  { if (CanPaginaAnterior)  PaginaActual--; _ = CargarDatos(); }
        public void PaginaSiguiente() { if (CanPaginaSiguiente) PaginaActual++; _ = CargarDatos(); }

        // ── Carga de datos ────────────────────────────────────────────────────
        private async Task CargarDatos()
        {
            IsLoading = true;
            try
            {
                var desde  = FechaDesde ?? DateTime.Today.AddDays(-30);
                var hasta  = FechaHasta ?? DateTime.Today;
                var todas  = await _ventaServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal, desde, hasta);

                // Filtro por texto si aplica
                var filtradas = string.IsNullOrWhiteSpace(TextoBusqueda)
                    ? todas
                    : todas.Where(v => v.ClienteNombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
                                    || v.IdVenta.ToString().Contains(TextoBusqueda));

                // Filtro por estado
                if (!string.IsNullOrWhiteSpace(FiltroEstado))
                    filtradas = filtradas.Where(v => v.Estado == FiltroEstado);

                var lista = filtradas.ToList();

                // Métricas
                var hoy = DateTime.Today;
                var mes = new DateTime(hoy.Year, hoy.Month, 1);

                var ventasHoy = lista.Where(v => v.Fecha.Date == hoy).ToList();
                var ventasMes = lista.Where(v => v.Fecha >= mes).ToList();

                TotalVentasHoy    = ventasHoy.Sum(v => v.TotalFinal);
                CantidadVentasHoy = ventasHoy.Count;
                TotalVentasMes    = ventasMes.Sum(v => v.TotalFinal);
                CantidadVentasMes = ventasMes.Count;
                VentasPendientes  = lista.Count(v => v.Estado == "Pendiente");
                VentasCanceladas  = lista.Count(v => v.Estado == "Anulada");

                // Paginación
                const int porPagina = 20;
                TotalVentas    = lista.Count;
                TotalPaginas   = Math.Max(1, (int)Math.Ceiling(lista.Count / (double)porPagina));
                PaginaActual   = Math.Min(PaginaActual, TotalPaginas);

                var pagina = lista
                    .Skip((PaginaActual - 1) * porPagina)
                    .Take(porPagina)
                    .ToList();

                VentasMostradas = pagina.Count;
                Ventas.Clear();
                foreach (var v in pagina) Ventas.Add(v);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally { IsLoading = false; }
        }
    }
}
