using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
            Titulo         = "Historial de Ventas";
            // Defecto: hoy
            FechaDesde = DateTime.Today;
            FechaHasta = DateTime.Today.AddDays(1).AddSeconds(-1);
        }

        // ── Filtros ───────────────────────────────────────────────────────────
        private DateTime _fechaDesde;
        public DateTime FechaDesde
        {
            get => _fechaDesde;
            set { _fechaDesde = value; NotifyOfPropertyChange(() => FechaDesde); }
        }

        private DateTime _fechaHasta;
        public DateTime FechaHasta
        {
            get => _fechaHasta;
            set { _fechaHasta = value; NotifyOfPropertyChange(() => FechaHasta); }
        }

        private string _filtroEstado = "Todos";
        public string FiltroEstado
        {
            get => _filtroEstado;
            set { _filtroEstado = value; NotifyOfPropertyChange(() => FiltroEstado); AplicarFiltros(); }
        }

        public ObservableCollection<string> EstadosFiltro { get; } =
            new() { "Todos", "Pendiente", "Pagada", "Anulada" };

        // ── Ventas ────────────────────────────────────────────────────────────
        private ObservableCollection<VentaResumenDto> _todasLasVentas = new();
        private ObservableCollection<VentaResumenDto> _ventas = new();
        public ObservableCollection<VentaResumenDto> Ventas
        {
            get => _ventas;
            set { _ventas = value; NotifyOfPropertyChange(() => Ventas); NotifyOfPropertyChange(() => TotalFiltrado); }
        }

        public decimal TotalFiltrado => Ventas
            .Where(v => v.Estado == "Pagada")
            .Sum(v => v.TotalFinal);

        private VentaResumenDto? _ventaSeleccionada;
        public VentaResumenDto? VentaSeleccionada
        {
            get => _ventaSeleccionada;
            set
            {
                _ventaSeleccionada = value;
                NotifyOfPropertyChange(() => VentaSeleccionada);
                NotifyOfPropertyChange(() => PuedeAnular);
                NotifyOfPropertyChange(() => PuedeVerDetalle);
            }
        }

        public bool PuedeAnular     => VentaSeleccionada?.Estado is "Pendiente" or "Pagada";
        public bool PuedeVerDetalle => VentaSeleccionada != null;

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await Buscar();

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task Buscar()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var ventas = await _ventaServicio.ObtenerPorSucursalAsync(
                    _sesion.IdSucursal, FechaDesde, FechaHasta);
                _todasLasVentas = new ObservableCollection<VentaResumenDto>(
                    ventas.OrderByDescending(v => v.Fecha));
                AplicarFiltros();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public void FiltrarHoy()
        {
            FechaDesde = DateTime.Today;
            FechaHasta = DateTime.Today.AddDays(1).AddSeconds(-1);
            _ = Buscar();
        }

        public void FiltrarEstaSemana()
        {
            var hoy = DateTime.Today;
            FechaDesde = hoy.AddDays(-(int)hoy.DayOfWeek + 1);
            FechaHasta = DateTime.Now;
            _ = Buscar();
        }

        public void FiltrarEsteMes()
        {
            FechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            FechaHasta = DateTime.Now;
            _ = Buscar();
        }

        public async Task NuevaVenta()
        {
            var vm = IoC.Get<VentaViewModel>();
            vm.NuevaVenta();
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task VerDetalle()
        {
            if (VentaSeleccionada == null) return;
            var vm = IoC.Get<ComprobanteViewModel>();
            await vm.CargarAsync(VentaSeleccionada.IdVenta, 0);
            await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task AnularVenta()
        {
            if (VentaSeleccionada == null || !PuedeAnular) return;

            var confirmacion = MessageBox.Show(
                $"¿Anular la venta #{VentaSeleccionada.IdVenta}?\n\n" +
                $"Se devolverá el stock y los ingresos de caja no se verán afectados.",
                "Confirmar anulación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes) return;

            IsLoading = true;
            LimpiarError();
            try
            {
                await _ventaServicio.CancelarAsync(VentaSeleccionada.IdVenta, "Anulación desde listado de ventas");
                await Buscar(); // Recargar lista
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void AplicarFiltros()
        {
            var filtradas = _todasLasVentas.AsEnumerable();
            if (FiltroEstado != "Todos")
                filtradas = filtradas.Where(v => v.Estado == FiltroEstado);
            Ventas = new ObservableCollection<VentaResumenDto>(filtradas);
        }
    }
}
