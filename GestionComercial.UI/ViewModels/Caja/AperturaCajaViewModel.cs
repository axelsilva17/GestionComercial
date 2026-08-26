using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Caja
{
    public class AperturaCajaViewModel : NavigableViewModel
    {
        private readonly ICajaServicio  _cajaServicio;
        private readonly IUnitOfWork     _uow;
        private readonly SesionServicio _sesion;

        public AperturaCajaViewModel(ICajaServicio cajaServicio, IUnitOfWork uow, SesionServicio sesion)
        {
            _cajaServicio = cajaServicio;
            _uow          = uow;
            _sesion       = sesion;
        }

        public DateTime FechaHoy => DateTime.Now;

        private string _sucursalNombre = "Casa Central";
        public string SucursalNombre
        {
            get => _sucursalNombre;
            set { _sucursalNombre = value; NotifyOfPropertyChange(() => SucursalNombre); }
        }

        private DateTime _ultimoCierre;
        public DateTime UltimoCierre
        {
            get => _ultimoCierre;
            set { _ultimoCierre = value; NotifyOfPropertyChange(() => UltimoCierre); }
        }

        private decimal _saldoAnterior;
        public decimal SaldoAnterior
        {
            get => _saldoAnterior;
            set { _saldoAnterior = value; NotifyOfPropertyChange(() => SaldoAnterior); }
        }

        private string _montoInicial = string.Empty;
        public string MontoInicial
        {
            get => _montoInicial;
            set { _montoInicial = value; NotifyOfPropertyChange(() => MontoInicial); }
        }

        // ── Paso 1: Selección de Turno ───────────────────────────────────────
        private TurnoCajaEnum? _turnoSeleccionado;
        public TurnoCajaEnum? TurnoSeleccionado
        {
            get => _turnoSeleccionado;
            set
            {
                _turnoSeleccionado = value;
                NotifyOfPropertyChange(() => TurnoSeleccionado);
                NotifyOfPropertyChange(() => MostrarPaso1);
                NotifyOfPropertyChange(() => MostrarPaso2);
                if (value.HasValue)
                    _ = CargarCajasPorTurnoAsync(value.Value);
            }
        }

        public bool MostrarPaso1 => TurnoSeleccionado == null;
        public bool MostrarPaso2 => TurnoSeleccionado != null;

        public void SeleccionarTurno(string turnoStr)
        {
            TurnoSeleccionado = TurnoCajaEnumExtensions.FromString(turnoStr);
        }

        // ── Paso 2: Selección de Caja ────────────────────────────────────────
        private ObservableCollection<CajaDisponibleDto> _cajasDisponibles = new();
        public ObservableCollection<CajaDisponibleDto> CajasDisponibles
        {
            get => _cajasDisponibles;
            set { _cajasDisponibles = value; NotifyOfPropertyChange(() => CajasDisponibles); }
        }

        private CajaDisponibleDto? _cajaSeleccionada;
        public CajaDisponibleDto? CajaSeleccionada
        {
            get => _cajaSeleccionada;
            set { _cajaSeleccionada = value; NotifyOfPropertyChange(() => CajaSeleccionada); }
        }

        public string TurnoSeleccionadoDisplay => TurnoSeleccionado?.ToDisplayString() ?? "";

        /// <summary>
        /// Vuelve al paso 1 (seleccionar otro turno).
        /// </summary>
        public void VolverATurnos()
        {
            TurnoSeleccionado = null;
            CajaSeleccionada = null;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // Verificar si ya hay una caja abierta en cualquier turno
                var cajaAbierta = await _cajaServicio.ObtenerCajaAbiertaAsync(_sesion.IdSucursal);
                if (cajaAbierta != null)
                {
                    await Cancelar();
                    return;
                }

                // Cargar datos históricos (saldo anterior)
                await CargarUltimoCierreAsync();
            }
            catch (Exception ex)
            {
                MostrarError($"No se pudo cargar el último cierre: {ex.Message}");
                UltimoCierre  = DateTime.Now;
                SaldoAnterior = 0;
            }
            finally { IsLoading = false; }
        }

        private async Task CargarCajasPorTurnoAsync(TurnoCajaEnum turno)
        {
            try
            {
                IsLoading = true;
                var turnoStr = turno.ToDisplayString();
                var cajas = await _uow.Cajas.ObtenerCajasPorTurnoAsync(_sesion.IdSucursal, turnoStr);

                CajasDisponibles = new ObservableCollection<CajaDisponibleDto>(
                    cajas.Select(c => new CajaDisponibleDto
                    {
                        Id = c.Id,
                        Nombre = c.EsPrimaria ? $"Caja {c.Id} (Principal)" : $"Caja {c.Id}",
                        Turno = c.Turno ?? turnoStr,
                        EsPrimaria = c.EsPrimaria,
                    }));

                if (CajasDisponibles.Any())
                    CajaSeleccionada = CajasDisponibles.First();

                NotifyOfPropertyChange(() => TurnoSeleccionadoDisplay);
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar cajas: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        private async Task CargarUltimoCierreAsync()
        {
            try
            {
                var historial = await _cajaServicio.ObtenerHistorialAsync(
                    _sesion.IdSucursal,
                    DateTime.Now.AddDays(-30),
                    DateTime.Now);

                var ultimaCajaCerrada = historial
                    .Where(c => !c.EstaAbierta)
                    .OrderByDescending(c => c.FechaApertura)
                    .FirstOrDefault();

                if (ultimaCajaCerrada != null)
                {
                    UltimoCierre  = ultimaCajaCerrada.FechaCierre ?? ultimaCajaCerrada.FechaApertura;
                    SaldoAnterior = (decimal)ultimaCajaCerrada.MontoFinal;
                }
                else
                {
                    UltimoCierre  = DateTime.Now;
                    SaldoAnterior = 0;
                }
            }
            catch
            {
                UltimoCierre  = DateTime.Now;
                SaldoAnterior = 0;
            }
        }

        public async Task Confirmar()
        {
            if (CajaSeleccionada == null)
            {
                MostrarError("Seleccioná una caja.");
                return;
            }

            decimal monto;
            if (string.IsNullOrWhiteSpace(MontoInicial))
            {
                monto = SaldoAnterior;
            }
            else if (!decimal.TryParse(
                    MontoInicial.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out monto) || monto < 0)
            {
                MostrarError("Ingresá un monto inicial válido.");
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                var caja = await _cajaServicio.AbrirCajaAsync(
                    _sesion.IdSucursal,
                    _sesion.IdUsuario,
                    monto,
                    turno: TurnoSeleccionado,
                    esPrimaria: CajaSeleccionada?.EsPrimaria ?? false);

                _sesion.IdCajaActual = caja.Id;
                _sesion.TurnoActual = TurnoSeleccionado?.ToDisplayString();
                await Cancelar();
            }
            catch (Exception ex)
            {
                var msg = $"ERROR:\n\n{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.InnerException?.Message}";
                System.Windows.MessageBox.Show(msg, "ERROR AL ABRIR CAJA",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }

        public async Task Cancelar()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<CajaViewModel>(), CancellationToken.None);
        }
    }

    public class CajaDisponibleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Turno { get; set; } = "General";
        public bool EsPrimaria { get; set; }
    }
}
