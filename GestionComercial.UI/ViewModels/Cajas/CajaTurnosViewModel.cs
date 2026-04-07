using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using CajaEntity = GestionComercial.Dominio.Entidades.Caja.Caja;

namespace GestionComercial.UI.ViewModels.Cajas
{
    public class CajaTurnosViewModel : Screen
    {
        private readonly IUnitOfWork _uow;
        private readonly SesionServicio _sesion;

        private ObservableCollection<CajaEntity> _cajas = new();
        public ObservableCollection<CajaEntity> Cajas
        {
            get => _cajas;
            set { _cajas = value; NotifyOfPropertyChange(() => Cajas); ActualizarCajasFiltradas(); }
        }

        private ObservableCollection<CajaEntity> _cajasFiltradas = new();
        public ObservableCollection<CajaEntity> CajasFiltradas
        {
            get => _cajasFiltradas;
            set { _cajasFiltradas = value; NotifyOfPropertyChange(() => CajasFiltradas); }
        }

        private CajaEntity? _cajaSeleccionada;
        public CajaEntity? CajaSeleccionada
        {
            get => _cajaSeleccionada;
            set { _cajaSeleccionada = value; NotifyOfPropertyChange(() => CajaSeleccionada); }
        }

        // ── Turnos disponibles ─────────────────────────────────────────────
        private ObservableCollection<string> _turnos = new() { "Mañana", "Tarde", "Noche" };
        public ObservableCollection<string> Turnos
        {
            get => _turnos;
            set { _turnos = value; NotifyOfPropertyChange(() => Turnos); }
        }

        private string _turnoNuevo = "Mañana";
        public string TurnoNuevo
        {
            get => _turnoNuevo;
            set { _turnoNuevo = value; NotifyOfPropertyChange(() => TurnoNuevo); }
        }

        // ── Filtro por Turno ───────────────────────────────────────────────
        private ObservableCollection<string> _filtroTurnos = new() { "Todos", "Mañana", "Tarde", "Noche" };
        public ObservableCollection<string> FiltroTurnos
        {
            get => _filtroTurnos;
            set { _filtroTurnos = value; NotifyOfPropertyChange(() => FiltroTurnos); }
        }

        private string _turnoFiltro = "Todos";
        public string TurnoFiltro
        {
            get => _turnoFiltro;
            set { _turnoFiltro = value; NotifyOfPropertyChange(() => TurnoFiltro); ActualizarCajasFiltradas(); }
        }

        public CajaTurnosViewModel(IUnitOfWork uow, SesionServicio sesion)
        {
            _uow = uow;
            _sesion = sesion;
            DisplayName = "Gestión de Cajas";
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await CargarCajasAsync();
        }

        private async Task CargarCajasAsync()
        {
            try
            {
                var cajas = (await _uow.Cajas.ObtenerTodosAsync()).ToList();
                Cajas = new ObservableCollection<CajaEntity>(cajas);
            }
            catch (Exception ex)
            {
                LogHelper.Log($"[CajaTurnos] Error al cargar cajas: {ex.Message}");
            }
        }

        private void ActualizarCajasFiltradas()
        {
            if (string.IsNullOrEmpty(TurnoFiltro) || TurnoFiltro == "Todos")
            {
                CajasFiltradas = new ObservableCollection<CajaEntity>(Cajas);
            }
            else
            {
                CajasFiltradas = new ObservableCollection<CajaEntity>(
                    Cajas.Where(c => c.Turno == TurnoFiltro));
            }
        }

        public async Task CrearCaja()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TurnoNuevo))
                {
                    MessageBox.Show("Debe seleccionar un turno.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var cajaExistente = Cajas.FirstOrDefault(c => c.EsPrimaria);
                var esPrimaria = cajaExistente == null;

                var nuevaCaja = new CajaEntity
                {
                    FechaApertura = DateTime.Now,
                    MontoInicial = 0,
                    Estado = 1,
                    Id_sucursal = _sesion.IdSucursal,
                    UsuarioApertura_id = _sesion.IdUsuario,
                    EsPrimaria = esPrimaria,
                    Turno = TurnoNuevo.Trim()
                };

                await _uow.Cajas.AgregarAsync(nuevaCaja);
                await _uow.GuardarCambiosAsync();

                await CargarCajasAsync();
                TurnoNuevo = "Mañana";

                LogHelper.Log($"[CajaTurnos] Caja creada con Turno: {nuevaCaja.Turno}, EsPrimaria: {nuevaCaja.EsPrimaria}");
            }
            catch (Exception ex)
            {
                LogHelper.Log($"[CajaTurnos] Error al crear caja: {ex.Message}");
                MessageBox.Show($"Error al crear caja: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task EliminarCaja()
        {
            if (CajaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una caja para eliminar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CajaSeleccionada.EsPrimaria)
            {
                MessageBox.Show("No se puede eliminar la caja principal.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CajaSeleccionada.Estado == 1) // Abierta
            {
                MessageBox.Show("No se puede eliminar una caja abierta. Ciérrela primero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show(
                $"¿Está seguro de eliminar la caja #{CajaSeleccionada.Id} del turno {CajaSeleccionada.Turno}?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                var cajaAEliminar = await _uow.Cajas.ObtenerPorIdAsync(CajaSeleccionada.Id);
                if (cajaAEliminar != null)
                {
                    _uow.Cajas.Eliminar(cajaAEliminar);
                    await _uow.GuardarCambiosAsync();
                }

                await CargarCajasAsync();
                LogHelper.Log($"[CajaTurnos] Caja eliminada: {CajaSeleccionada.Id}");
            }
            catch (Exception ex)
            {
                LogHelper.Log($"[CajaTurnos] Error al eliminar caja: {ex.Message}");
                MessageBox.Show($"Error al eliminar caja: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
