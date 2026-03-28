using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using CajaEntidad = GestionComercial.Dominio.Entidades.Caja.Caja;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Caja;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Cajas
{
    public class CajaTurnosViewModel : NavigableViewModel
    {
        private readonly ICajaServicio  _cajaServicio;
        private readonly SesionServicio _sesion;

        public CajaTurnosViewModel(ICajaServicio cajaServicio, SesionServicio sesion)
        {
            _cajaServicio = cajaServicio;
            _sesion       = sesion;
            Titulo        = "Caja Turnos";
            Subtitulo     = "Gestión de turnos de caja";
        }

        // ── Propiedades ─────────────────────────────────────────────────────

        private ObservableCollection<CajaEntidad> _cajas = new();
        public ObservableCollection<CajaEntidad> Cajas
        {
            get => _cajas;
            set { _cajas = value; NotifyOfPropertyChange(() => Cajas); }
        }

        private CajaEntidad? _cajaSeleccionada;
        public CajaEntidad? CajaSeleccionada
        {
            get => _cajaSeleccionada;
            set
            {
                _cajaSeleccionada = value;
                NotifyOfPropertyChange(() => CajaSeleccionada);
                NotifyOfPropertyChange(() => CajaSeleccionadaEsPrimaria);
                NotifyOfPropertyChange(() => CajaSeleccionadaNoPrimaria);
            }
        }

        public bool CajaSeleccionadaEsPrimaria => CajaSeleccionada?.EsPrimaria == true;
        public bool CajaSeleccionadaNoPrimaria => CajaSeleccionada?.EsPrimaria != true;

        private string _turnoNuevo = string.Empty;
        public string TurnoNuevo
        {
            get => _turnoNuevo;
            set { _turnoNuevo = value; NotifyOfPropertyChange(() => TurnoNuevo); }
        }

        // ── Lifecycle ───────────────────────────────────────────────────────

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarCajasAsync();

        // ── Carga de datos ──────────────────────────────────────────────────

        private async Task CargarCajasAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                var desde = DateTime.Today.AddDays(-30);
                var hasta = DateTime.Now;
                var lista = await _cajaServicio.ObtenerHistorialAsync(_sesion.IdSucursal, desde, hasta);
                Cajas = new ObservableCollection<CajaEntidad>(lista);
            }
            catch (Exception ex)
            {
                MostrarError($"Error al cargar cajas: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        // ── Acciones ────────────────────────────────────────────────────────

        public async Task CrearCaja()
        {
            if (string.IsNullOrWhiteSpace(TurnoNuevo))
            {
                MostrarError("Ingresá un nombre de turno.");
                return;
            }

            IsLoading = true;
            LimpiarError();
            try
            {
                // Crear caja con montoInicial=0 (turno secundario, no primaria)
                var caja = await _cajaServicio.AbrirCajaAsync(
                    _sesion.IdSucursal, _sesion.IdUsuario, 0);

                // Setear propiedades de turno
                caja.Turno = TurnoNuevo.Trim();
                caja.EsPrimaria = false;

                TurnoNuevo = string.Empty;
                await CargarCajasAsync();

                System.Windows.MessageBox.Show(
                    $"Caja '{caja.Turno}' creada correctamente (ID: {caja.Id}).",
                    "Caja creada",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error al crear caja: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }

        public async Task Actualizar()
            => await CargarCajasAsync();

        public async Task IrACaja()
        {
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<CajaViewModel>(), CancellationToken.None);
        }
    }
}
