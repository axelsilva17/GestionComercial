using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class VentaListadoViewModel : NavigableViewModel
    {
        // Cap for the standalone sales-history list: the 200 most recent sales in the range
        // are what the list and the detail drawer need; loading (and rendering) more on a
        // 100k-row DB is slow without value.
        private const int ListadoTop = 200;
        // Cap for the per-client history list (most recent sales in the range).
        private const int HistorialClienteTop = 500;

        private readonly IVentaServicio _ventaServicio;
        private readonly SesionServicio _sesion;

        public VentaListadoViewModel(IVentaServicio ventaServicio, SesionServicio sesion)
        {
            _ventaServicio = ventaServicio;
            _sesion        = sesion;
            Titulo         = "Historial de Ventas";
            // Defecto: últimos 30 días (la mayoría de las DBs no tienen ventas del día actual,
            // así que "hoy" arrancaba vacío). El rango por cliente se setea desde el caller.
            FechaDesde = DateTime.Today.AddDays(-30);
            FechaHasta = DateTime.Today.AddDays(1).AddSeconds(-1);
        }

        // ── Filtro por cliente (desde sidebar de clientes) ───────────────────
        private int _clienteId;
        public int ClienteId
        {
            get => _clienteId;
            set
            {
                _clienteId = value;
                NotifyOfPropertyChange(() => ClienteId);
            }
        }

        private string _clienteNombre = string.Empty;
        public string ClienteNombre
        {
            get => _clienteNombre;
            set
            {
                _clienteNombre = value;
                NotifyOfPropertyChange(() => ClienteNombre);
                Titulo = string.IsNullOrEmpty(value)
                    ? "Historial de Ventas"
                    : $"Ventas de {value}";
                NotifyOfPropertyChange(() => Titulo);
            }
        }

        ///         /// Maneja atajos de teclado globales en el listado de ventas.
        public void HandleKeyDown(Key key, ModifierKeys modifiers)
        {
            switch (modifiers)
            {
                case ModifierKeys.Control:
                    switch (key)
                    {
                        case Key.N:
                            _ = NuevaVenta();
                            break;
                        case Key.H:
                            FiltrarHoy();
                            break;
                        case Key.S:
                            FiltrarEstaSemana();
                            break;
                        case Key.M:
                            FiltrarEsteMes();
                            break;
                    }
                    break;
                case ModifierKeys.None:
                    switch (key)
                    {
                        case Key.Enter:
                            if (VentaSeleccionada != null) _ = VerDetalle();
                            break;
                        case Key.Delete:
                            if (PuedeAnular) _ = AnularVenta();
                            break;
                    }
                    break;
            }
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
            new() { "Todos", "En proceso", "Pendiente", "Pagada", "Anulada" };

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
                NotifyOfPropertyChange(() => PuedeCobrar);
            }
        }

        public bool PuedeAnular     => VentaSeleccionada?.Estado is "En proceso" or "Pendiente" or "Pagada";
        public bool PuedeCobrar     => VentaSeleccionada?.Estado is "En proceso" or "Pendiente";
        public bool PuedeVerDetalle => VentaSeleccionada != null;

        // ── Detalle de la venta (drawer lateral) ─────────────────────────────
        // Lazy-loaded only for the selected row via ObtenerPorIdAsync; the list rows are
        // a light projection without items, so hydrating the whole list would be wasteful.
        private VentaDto? _detalleVenta;
        public VentaDto? DetalleVenta
        {
            get => _detalleVenta;
            set
            {
                _detalleVenta = value;
                NotifyOfPropertyChange(() => DetalleVenta);
            }
        }

        // ── Estado del listado ────────────────────────────────────────────────
        private bool _sinResultados;
        public bool SinResultados
        {
            get => _sinResultados;
            private set { _sinResultados = value; NotifyOfPropertyChange(() => SinResultados); }
        }

        private bool _listaCapada;
        public bool ListaCapada
        {
            get => _listaCapada;
            private set { _listaCapada = value; NotifyOfPropertyChange(() => ListaCapada); }
        }

        private string _mensajeCapa = string.Empty;
        public string MensajeCapa
        {
            get => _mensajeCapa;
            private set { _mensajeCapa = value; NotifyOfPropertyChange(() => MensajeCapa); }
        }

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
                IEnumerable<VentaResumenDto> ventas;
                int cap;
                if (ClienteId > 0)
                {
                    // Client history: light SQL projection filtered by client + date range,
                    // capped to the most recent sales. The old path loaded the whole sucursal's
                    // sales for the range and filtered the client in memory.
                    cap = HistorialClienteTop;
                    ventas = await _ventaServicio.ObtenerHistorialPorClienteAsync(
                        ClienteId, FechaDesde, FechaHasta, cap);
                }
                else
                {
                    // Standalone history: light SQL projection of the sucursal's most recent
                    // sales in the range, capped so a 30-day window on a big DB neither loads
                    // nor renders the whole range (previously the full range was materialized).
                    cap = ListadoTop;
                    ventas = await _ventaServicio.ObtenerRecientesPorSucursalAsync(
                        _sesion.IdSucursal, FechaDesde, FechaHasta, cap,
                        MapFiltroEstado(FiltroEstado));
                }

                IEnumerable<VentaResumenDto> filtradas = ventas;

                _todasLasVentas = new ObservableCollection<VentaResumenDto>(
                    filtradas.OrderByDescending(v => v.Fecha));

                // Cap hint: only truthy when the query actually hit the Take limit (== cap rows).
                ListaCapada = _todasLasVentas.Count == cap;
                MensajeCapa = ListaCapada ? $"Mostrando las últimas {cap} ventas" : string.Empty;

                // Cerrar el drawer si la venta seleccionada ya no está en el resultado.
                if (DetalleVenta != null && _todasLasVentas.All(v => v.IdVenta != DetalleVenta.IdVenta))
                    DetalleVenta = null;

                AplicarFiltros();
                SinResultados = !TieneError && Ventas.Count == 0;
            }
            catch (Exception ex)
            {
                SinResultados = false;
                MostrarError(ex.Message);
            }
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
            // Semana que empieza en lunes. El cálculo naive `-DayOfWeek + 1` fallaba los domingos
            // (DayOfWeek == 0 → Desde se iba al lunes SIGUIENTE, rango vacío).
            var hoy = DateTime.Today;
            var diasDesdeLunes = ((int)hoy.DayOfWeek + 6) % 7;
            FechaDesde = hoy.AddDays(-diasDesdeLunes);
            FechaHasta = DateTime.Now;
            _ = Buscar();
        }

        public void FiltrarEsteMes()
        {
            FechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            FechaHasta = DateTime.Now;
            _ = Buscar();
        }

        public void FiltrarUltimos30Dias()
        {
            FechaDesde = DateTime.Today.AddDays(-30);
            FechaHasta = DateTime.Today.AddDays(1).AddSeconds(-1);
            _ = Buscar();
        }

        public async Task CargarDetalleVentaAsync(VentaResumenDto? venta)
        {
            // Toggle: re-seleccionar la misma venta (o selección nula) cierra el drawer.
            if (venta == null || DetalleVenta?.IdVenta == venta.IdVenta)
            {
                DetalleVenta = null;
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                // Lazy load: solo se hidratan los ítems de la venta seleccionada.
                var detalle = await _ventaServicio.ObtenerPorIdAsync(venta.IdVenta);
                // Stale-response guard: si mientras cargaba el usuario seleccionó otra venta,
                // descartar la respuesta vieja (evita mostrar el detalle de la venta A con la B seleccionada).
                if (VentaSeleccionada?.IdVenta != venta.IdVenta) return;
                DetalleVenta = detalle;
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public void CerrarDetalle() => DetalleVenta = null;

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

        public async Task CobrarVenta()
        {
            if (VentaSeleccionada == null || !PuedeCobrar) return;

            var confirmacion = MessageBox.Show(
                $"¿Cobrar la venta #{VentaSeleccionada.IdVenta}?\n\n" +
                $"Se registrará como pago en efectivo por ${VentaSeleccionada.TotalFinal:N2}.",
                "Confirmar cobro",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmacion != MessageBoxResult.Yes) return;

            IsLoading = true;
            LimpiarError();
            try
            {
                await _ventaServicio.CobrarVentaAsync(VentaSeleccionada.IdVenta);
                await Buscar(); // Recargar lista
            }
            catch (Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
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
            // Recalcular aquí también: cambiar el filtro de estado puede dejar la lista vacía
            // (o al revés, dejar un overlay "sin resultados" tapando datos).
            SinResultados = !TieneError && Ventas.Count == 0;
        }

        private static int? MapFiltroEstado(string? filtro) => filtro switch
        {
            "En proceso" => (int)EstadoVentaEnum.EnProceso,
            "Pendiente"  => (int)EstadoVentaEnum.Pendiente,
            "Pagada"     => (int)EstadoVentaEnum.Pagada,
            "Anulada"    => (int)EstadoVentaEnum.Anulada,
            _            => null // "Todos" or null → all states
        };

        public async Task Volver()
        {
            await IoC.Get<ShellViewModel>()
                     .IrDashboard();
        }
    }
}
