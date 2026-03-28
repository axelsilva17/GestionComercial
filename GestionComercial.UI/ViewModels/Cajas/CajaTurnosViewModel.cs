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
            set { _cajas = value; NotifyOfPropertyChange(() => Cajas); }
        }

        private CajaEntity? _cajaSeleccionada;
        public CajaEntity? CajaSeleccionada
        {
            get => _cajaSeleccionada;
            set { _cajaSeleccionada = value; NotifyOfPropertyChange(() => CajaSeleccionada); }
        }

        private string _turnoNuevo = "General";
        public string TurnoNuevo
        {
            get => _turnoNuevo;
            set { _turnoNuevo = value; NotifyOfPropertyChange(() => TurnoNuevo); }
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

        public async Task CrearCaja()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TurnoNuevo))
                {
                    MessageBox.Show("Debe ingresar un turno.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                TurnoNuevo = "General";

                LogHelper.Log($"[CajaTurnos] Caja creada con Turno: {nuevaCaja.Turno}, EsPrimaria: {nuevaCaja.EsPrimaria}");
            }
            catch (Exception ex)
            {
                LogHelper.Log($"[CajaTurnos] Error al crear caja: {ex.Message}");
                MessageBox.Show($"Error al crear caja: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task CerrarCaja()
        {
            if (CajaSeleccionada == null)
            {
                MessageBox.Show("Seleccione una caja.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CajaSeleccionada.EsPrimaria)
            {
                MessageBox.Show("No se puede cerrar la caja principal. Cree otra caja y transfiera el estado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                CajaSeleccionada.Estado = 2;
                CajaSeleccionada.FechaCierre = DateTime.Now;
                _uow.Cajas.Actualizar(CajaSeleccionada);
                await _uow.GuardarCambiosAsync();

                await CargarCajasAsync();
            }
            catch (Exception ex)
            {
                LogHelper.Log($"[CajaTurnos] Error al cerrar caja: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
