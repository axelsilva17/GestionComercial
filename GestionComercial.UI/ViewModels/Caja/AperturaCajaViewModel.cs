using Caliburn.Micro;
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

        // ── Selección de Caja y Turno ─────────────────────────────────────────
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

        private ObservableCollection<string> _turnos = new() { "General", "Mañana", "Tarde", "Noche" };
        public ObservableCollection<string> Turnos
        {
            get => _turnos;
            set { _turnos = value; NotifyOfPropertyChange(() => Turnos); }
        }

        private string _turnoSeleccionado = "General";
        public string TurnoSeleccionado
        {
            get => _turnoSeleccionado;
            set { _turnoSeleccionado = value; NotifyOfPropertyChange(() => TurnoSeleccionado); }
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                // Verificar si ya hay una caja abierta — si sí, volver atrás
                var cajaAbierta = await _cajaServicio.ObtenerCajaAbiertaAsync(_sesion.IdSucursal);
                if (cajaAbierta != null)
                {
                    await Cancelar();
                    return;
                }

                // Cargar cajas disponibles para seleccionar
                var todasCajas = (await _uow.Cajas.ObtenerTodosAsync()).ToList();
                CajasDisponibles = new ObservableCollection<CajaDisponibleDto>(
                    todasCajas.Select(c => new CajaDisponibleDto
                    {
                        Id = c.Id,
                        Nombre = c.EsPrimaria ? $"Caja {c.Id} (Principal)" : $"Caja {c.Id} - {c.Turno ?? "General"}",
                        Turno = c.Turno ?? "General",
                    }));

                if (CajasDisponibles.Any())
                    CajaSeleccionada = CajasDisponibles.First();

                UltimoCierre  = DateTime.Now;
                SaldoAnterior = 0;
            }
            catch (Exception ex)
            {
                // No bloqueamos la apertura si falla cargar datos históricos
                MostrarError($"No se pudo cargar el último cierre: {ex.Message}");
                UltimoCierre  = DateTime.Now;
                SaldoAnterior = 0;
            }
            finally { IsLoading = false; }
        }

        public async Task Confirmar()
        {
            if (CajaSeleccionada == null)
            {
                MostrarError("Seleccioná una caja.");
                return;
            }

            if (!decimal.TryParse(
                    MontoInicial.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var monto) || monto < 0)
            {
                MostrarError("Ingresá un monto inicial válido.");
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                var caja = await _cajaServicio.AbrirCajaAsync(
                    _sesion.IdSucursal, _sesion.IdUsuario, monto);

                _sesion.IdCajaActual = caja.Id;
                await Cancelar();
            }
            catch (Exception ex) 
            { 
                // Mostrar error en MessageBox para debug
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
    }
}
