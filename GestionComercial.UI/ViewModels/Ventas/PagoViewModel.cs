using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class PagoViewModel : NavigableViewModel
    {
        private readonly IVentaServicio _ventaServicio;
        private readonly IUnitOfWork    _uow;
        private readonly SesionServicio _sesion;

        private int _idVenta;

        public PagoViewModel(IVentaServicio ventaServicio, IUnitOfWork uow, SesionServicio sesion)
        {
            _ventaServicio = ventaServicio;
            _uow           = uow;
            _sesion        = sesion;
            Titulo         = "Cobrar Venta";
        }

        /// <summary>
        /// Maneja atajos de teclado globales en la vista de pago.
        /// </summary>
        public void HandleKeyDown(Key key, ModifierKeys modifiers)
        {
            if (modifiers != ModifierKeys.None) return;

            switch (key)
            {
                case Key.F1:
                    AgregarEfectivo();
                    break;
                case Key.F2:
                    AgregarDebito();
                    break;
                case Key.F3:
                    AgregarCredito();
                    break;
                case Key.F4:
                    AgregarQR();
                    break;
                case Key.F5:
                    CompletarConEfectivo();
                    break;
                case Key.F6:
                    if (PuedeCobrar) _ = Confirmar();
                    break;
                case Key.Escape:
                    _ = Cancelar();
                    break;
            }
        }

        // ── Datos de la venta ─────────────────────────────────────────────────
        private string _clienteNombre = "Consumidor Final";
        public string ClienteNombre
        {
            get => _clienteNombre;
            set { _clienteNombre = value; NotifyOfPropertyChange(() => ClienteNombre); }
        }

        private decimal _totalVenta;
        public decimal TotalVenta
        {
            get => _totalVenta;
            set { _totalVenta = value; NotifyOfPropertyChange(() => TotalVenta); RecalcularVuelto(); }
        }

        private decimal _totalPagado;
        public decimal TotalPagado
        {
            get => _totalPagado;
            set
            {
                _totalPagado = value;
                NotifyOfPropertyChange(() => TotalPagado);
                NotifyOfPropertyChange(() => Faltante);
                NotifyOfPropertyChange(() => HayFaltante);
                NotifyOfPropertyChange(() => PuedeCobrar);
                RecalcularVuelto();
                // Notificaciones explícitas para asegurar que el binding se actualice
                NotifyOfPropertyChange(() => Vuelto);
                NotifyOfPropertyChange(() => HayVuelto);
            }
        }

        public decimal Faltante => Math.Max(TotalVenta - TotalPagado, 0);

        private decimal _vuelto;
        public decimal Vuelto
        {
            get => _vuelto;
            set { _vuelto = value; NotifyOfPropertyChange(() => Vuelto); NotifyOfPropertyChange(() => HayVuelto); }
        }
        public bool HayVuelto => Vuelto > 0;

        public bool HayFaltante => Faltante > 0;

        public bool PuedeCobrar => TotalPagado >= TotalVenta && Pagos.Any();

        // ── Métodos de pago disponibles (combo) ───────────────────────────────
        private ObservableCollection<PagoItemDto> _metodosPago = new();
        public ObservableCollection<PagoItemDto> MetodosPago
        {
            get => _metodosPago;
            set { _metodosPago = value; NotifyOfPropertyChange(() => MetodosPago); }
        }

        private PagoItemDto? _metodoSeleccionado;
        public PagoItemDto? MetodoSeleccionado
        {
            get => _metodoSeleccionado;
            set { _metodoSeleccionado = value; NotifyOfPropertyChange(() => MetodoSeleccionado); }
        }

        private string _montoIngresado = string.Empty;
        public string MontoIngresado
        {
            get => _montoIngresado;
            set { _montoIngresado = value; NotifyOfPropertyChange(() => MontoIngresado); }
        }

        // ── Líneas de pago seleccionadas ──────────────────────────────────────
        private ObservableCollection<PagoLineaVm> _pagos = new();
        public ObservableCollection<PagoLineaVm> Pagos
        {
            get => _pagos;
            set { _pagos = value; NotifyOfPropertyChange(() => Pagos); }
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarMetodosAsync();

        private async Task CargarMetodosAsync()
        {
            try
            {
                var sucursal  = await _uow.Sucursales.ObtenerPorIdAsync(_sesion.IdSucursal);
                if (sucursal == null)
                {
                    MostrarError("No se encontró la sucursal configurada.");
                    return;
                }
                var idEmpresa = sucursal.Id_empresa;
                if (idEmpresa <= 0)
                {
                    MostrarError("ID de empresa inválido.");
                    return;
                }
                var metodos   = await _uow.MetodosPago.ObtenerTodosPorEmpresaAsync(idEmpresa);
                if (metodos == null)
                {
                    MetodosPago = new ObservableCollection<PagoItemDto>();
                    return;
                }

                MetodosPago = new ObservableCollection<PagoItemDto>(
                    metodos.Select(m => new PagoItemDto
                    {
                        IdMetodoPago = m.Id,
                        NombreMetodo = m.Nombre,
                        EsEfectivo   = m.EsEfectivo == true,
                        Monto        = 0,
                    }));

                MetodoSeleccionado = MetodosPago.FirstOrDefault(m => m.EsEfectivo)
                                  ?? MetodosPago.FirstOrDefault();

                // Precompletar con el total en el campo de monto
                MontoIngresado = TotalVenta.ToString("F2");
            }
            catch (Exception ex)
            {
                var mensaje = $"Error al cargar métodos de pago: {ex.Message}";
                MostrarError(mensaje);
                System.Windows.MessageBox.Show(mensaje, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>Llamado desde VentaViewModel antes de navegar.</summary>
        public void InicializarConVenta(int idVenta, string clienteNombre, decimal totalFinal)
        {
            _idVenta       = idVenta;
            ClienteNombre  = clienteNombre;
            TotalVenta     = totalFinal;
            Pagos          = new();
            MontoIngresado = totalFinal.ToString("F2");
            RecalcularVuelto();
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public void AgregarPago()
        {
            if (MetodoSeleccionado == null) { MostrarError("Seleccioná un método de pago."); return; }

            var texto = MontoIngresado.Replace(",", ".");
            if (!decimal.TryParse(texto,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var monto) || monto <= 0)
            {
                MostrarError("Ingresá un monto válido mayor a 0.");
                return;
            }

            LimpiarError();

            // Si el método ya existe, sumar el monto
            var existente = Pagos.FirstOrDefault(p => p.IdMetodoPago == MetodoSeleccionado.IdMetodoPago);
            if (existente != null)
            {
                var idx = Pagos.IndexOf(existente);
                Pagos[idx] = new PagoLineaVm
                {
                    IdMetodoPago = existente.IdMetodoPago,
                    NombreMetodo = existente.NombreMetodo,
                    EsEfectivo   = existente.EsEfectivo,
                    Monto        = existente.Monto + monto,
                };
            }
            else
            {
                Pagos.Add(new PagoLineaVm
                {
                    IdMetodoPago = MetodoSeleccionado.IdMetodoPago,
                    NombreMetodo = MetodoSeleccionado.NombreMetodo,
                    EsEfectivo   = MetodoSeleccionado.EsEfectivo,
                    Monto        = monto,
                });
            }

            MontoIngresado = string.Empty;
            RecalcularTotalPagado();
        }

        public void QuitarPago(PagoLineaVm linea)
        {
            if (linea == null) return;
            Pagos.Remove(linea);
            RecalcularTotalPagado();
        }

        /// <summary>Agrega la diferencia faltante en efectivo con un clic.</summary>
        public void CompletarConEfectivo()
        {
            if (Faltante <= 0) return;
            var efectivo = MetodosPago.FirstOrDefault(m => m.EsEfectivo);
            if (efectivo == null) return;
            MetodoSeleccionado = efectivo;
            MontoIngresado     = Faltante.ToString("F2");
            AgregarPago();
        }

        /// <summary>
        /// Agrega pago en efectivo (botón rápido).
        /// Usa el total de la venta si no hay pagos, sino usa el faltante.
        /// </summary>
        public void AgregarEfectivo()
        {
            var efectivo = MetodosPago.FirstOrDefault(m => m.EsEfectivo);
            if (efectivo == null) { MostrarError("No hay método de pago en efectivo configurado."); return; }
            MetodoSeleccionado = efectivo;
            MontoIngresado = Faltante > 0 ? Faltante.ToString("F2") : TotalVenta.ToString("F2");
            AgregarPago();
        }

        /// <summary>
        /// Agrega pago con tarjeta de débito (botón rápido).
        /// </summary>
        public void AgregarDebito()
        {
            var debito = MetodosPago.FirstOrDefault(m =>
                m.NombreMetodo.Contains("Débito", StringComparison.OrdinalIgnoreCase) ||
                m.NombreMetodo.Contains("Debito", StringComparison.OrdinalIgnoreCase));
            if (debito == null) { MostrarError("No hay método de pago débito configurado."); return; }
            MetodoSeleccionado = debito;
            var monto = Faltante > 0 ? Faltante : TotalVenta;
            System.Diagnostics.Debug.WriteLine($"[PagoVM] AgregarDebito: TotalVenta={TotalVenta}, Faltante={Faltante}, monto a pagar={monto}");
            MontoIngresado = monto.ToString("F2");
            AgregarPago();
        }

        /// <summary>
        /// Agrega pago con tarjeta de crédito (botón rápido).
        /// </summary>
        public void AgregarCredito()
        {
            var credito = MetodosPago.FirstOrDefault(m =>
                m.NombreMetodo.Contains("Crédito", StringComparison.OrdinalIgnoreCase) ||
                m.NombreMetodo.Contains("Credito", StringComparison.OrdinalIgnoreCase));
            if (credito == null) { MostrarError("No hay método de pago crédito configurado."); return; }
            MetodoSeleccionado = credito;
            var monto = Faltante > 0 ? Faltante : TotalVenta;
            System.Diagnostics.Debug.WriteLine($"[PagoVM] AgregarCredito: TotalVenta={TotalVenta}, Faltante={Faltante}, monto a pagar={monto}");
            MontoIngresado = monto.ToString("F2");
            AgregarPago();
        }

        /// <summary>
        /// Agrega pago con QR (botón rápido).
        /// </summary>
        public void AgregarQR()
        {
            var qr = MetodosPago.FirstOrDefault(m =>
                m.NombreMetodo.Contains("QR", StringComparison.OrdinalIgnoreCase) ||
                m.NombreMetodo.Contains("Transferencia", StringComparison.OrdinalIgnoreCase));
            if (qr == null) { MostrarError("No hay método de pago QR configurado."); return; }
            MetodoSeleccionado = qr;
            var monto = Faltante > 0 ? Faltante : TotalVenta;
            System.Diagnostics.Debug.WriteLine($"[PagoVM] AgregarQR: TotalVenta={TotalVenta}, Faltante={Faltante}, monto a pagar={monto}");
            MontoIngresado = monto.ToString("F2");
            AgregarPago();
        }

        public async Task Confirmar()
        {
            System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Iniciando confirmación de pago...");
            System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] _idVenta={_idVenta}, Vuelto={Vuelto}, TotalPagado={TotalPagado}, TotalVenta={TotalVenta}");
            
            if (!PuedeCobrar) { MostrarError("El monto no cubre el total."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                var pagosDto = Pagos.Select(p => new PagoItemDto
                {
                    IdMetodoPago = p.IdMetodoPago,
                    Monto        = p.Monto,
                    EsEfectivo   = p.EsEfectivo,
                }).ToList();

                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] Pagos: {pagosDto.Count}, Total: {pagosDto.Sum(p => p.Monto)}");

                // Verificar null antes de llamar al servicio
                if (_idVenta <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] ERROR: ID de venta inválido");
                    MostrarError("ID de venta inválido.");
                    return;
                }

                if (pagosDto == null || !pagosDto.Any())
                {
                    System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] ERROR: No hay pagos registrados");
                    MostrarError("No hay pagos registrados.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] Llamando a RegistrarPagoAsync para venta #{_idVenta}...");
                await _ventaServicio.RegistrarPagoAsync(_idVenta, pagosDto);
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Pago registrado exitosamente");

                // Ir al comprobante
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Navegando a ComprobanteViewModel...");
                var vm = IoC.Get<ComprobanteViewModel>();
                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] ComprobanteVM obtenido: {vm != null}");
                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] Llamando CargarAsync con idVenta={_idVenta}, Vuelto={Vuelto}");
                await vm.CargarAsync(_idVenta, Vuelto);
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] CargarAsync completado");
                await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Proceso completado");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] ERROR: {ex}");
                var mensaje = $"Error al confirmar el pago: {ex.Message}";
                MostrarError(mensaje);
                System.Windows.MessageBox.Show(mensaje, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }

        public async Task Cancelar()
        {
            // La venta queda en Pendiente — el operador puede anularla desde el historial
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaViewModel>(), CancellationToken.None);
        }

        private void RecalcularTotalPagado()
        {
            TotalPagado = Pagos.Sum(p => p.Monto);
            NotifyOfPropertyChange(() => PuedeCobrar);
        }

        private void RecalcularVuelto()
            => Vuelto = TotalPagado > TotalVenta ? TotalPagado - TotalVenta : 0;

        // ── Popup Historial (same as VentaViewModel) ─────────────────────────────
        private bool _mostrarHistorial;
        public bool MostrarHistorial
        {
            get => _mostrarHistorial;
            set { _mostrarHistorial = value; NotifyOfPropertyChange(() => MostrarHistorial); }
        }

        private ObservableCollection<VentaResumenDto> _historialVentas = new();
        public ObservableCollection<VentaResumenDto> HistorialVentas
        {
            get => _historialVentas;
            set { _historialVentas = value; NotifyOfPropertyChange(() => HistorialVentas); }
        }

        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set { SetProperty(ref _fechaDesde, value); }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set { SetProperty(ref _fechaHasta, value); }
        }

        private string _dniClienteFiltro = string.Empty;
        public string DniClienteFiltro
        {
            get => _dniClienteFiltro;
            set { SetProperty(ref _dniClienteFiltro, value); }
        }

        private int? _estadoVentaFiltro;
        public int? EstadoVentaFiltro
        {
            get => _estadoVentaFiltro;
            set { SetProperty(ref _estadoVentaFiltro, value); }
        }

        /// <summary>
        /// Carga el historial de ventas (reutiliza la lógica de VentaViewModel).
        /// </summary>
        public async Task CargarHistorialAsync()
        {
            try
            {
                var desde = FechaDesde ?? DateTime.Today.AddDays(-30);
                var hasta = FechaHasta ?? DateTime.Today.AddDays(1).AddSeconds(-1);

                IEnumerable<VentaResumenDto> ventas;

                if (FechaDesde.HasValue || FechaHasta.HasValue || !string.IsNullOrWhiteSpace(DniClienteFiltro) || EstadoVentaFiltro.HasValue)
                {
                    ventas = await _ventaServicio.ObtenerVentasAsync(
                        _sesion.IdSucursal, FechaDesde, FechaHasta, DniClienteFiltro, EstadoVentaFiltro);
                }
                else
                {
                    ventas = await _ventaServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal, desde, hasta);
                }

                var listaVentas = ventas.Take(50).ToList();
                HistorialVentas = new ObservableCollection<VentaResumenDto>(listaVentas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PagoVM] CargarHistorialAsync ERROR: {ex.Message}");
            }
        }

        /// <summary>
        /// Filtra el historial aplicando los filtros activos.
        /// </summary>
        public void FiltrarHistorial()
        {
            _ = CargarHistorialAsync();
        }

        /// <summary>
        /// Cierra el popup de historial.
        /// </summary>
        public void CerrarHistorial()
        {
            MostrarHistorial = false;
        }

        private bool _ventaCompletada;
        public bool VentaCompletada
        {
            get => _ventaCompletada;
            set { _ventaCompletada = value; NotifyOfPropertyChange(() => VentaCompletada); }
        }
    }

    public class PagoLineaVm
    {
        public int     IdMetodoPago { get; set; }
        public string  NombreMetodo { get; set; } = string.Empty;
        public bool    EsEfectivo   { get; set; }
        public decimal Monto        { get; set; }
    }
}
