using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Caja;
using System;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Caja
{
    public class RegistrarIngresoEgresoViewModel : Screen
    {
        private readonly ICajaServicio  _cajaServicio;
        private readonly SesionServicio _sesion;
        private readonly Action<bool>   _onClose;

        public RegistrarIngresoEgresoViewModel(
            ICajaServicio cajaServicio,
            SesionServicio sesion,
            Action<bool> onClose)
        {
            _cajaServicio = cajaServicio;
            _sesion       = sesion;
            _onClose      = onClose;
            
            EsIngreso = true;
            Titulo    = "Registrar Ingreso";
        }

        // ── Resultado del diálogo ─────────────────────────────────────────────
        public bool? Result { get; private set; }

        // ── Título dinámico ──────────────────────────────────────────────────────
        private string _titulo = "Registrar Ingreso";
        public string Titulo
        {
            get => _titulo;
            set { _titulo = value; NotifyOfPropertyChange(() => Titulo); }
        }

        // ── Tipo de movimiento ─────────────────────────────────────────────────
        private bool _esIngreso = true;
        public bool EsIngreso
        {
            get => _esIngreso;
            set
            {
                _esIngreso = value;
                NotifyOfPropertyChange(() => EsIngreso);
                NotifyOfPropertyChange(() => EsEgreso);
                Titulo = value ? "Registrar Ingreso" : "Registrar Egreso";
            }
        }

        public bool EsEgreso => !EsIngreso;

        // ── Monto ──────────────────────────────────────────────────────────────
        private string _monto = string.Empty;
        public string Monto
        {
            get => _monto;
            set { _monto = value; NotifyOfPropertyChange(() => Monto); }
        }

        // ── Descripción ────────────────────────────────────────────────────────
        private string _descripcion = string.Empty;
        public string Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; NotifyOfPropertyChange(() => Descripcion); }
        }

        // ── Validación ────────────────────────────────────────────────────────
        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; NotifyOfPropertyChange(() => ErrorMessage); }
        }

        public bool TieneError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool IsLoading   { get; private set; }

        // ── Acciones ───────────────────────────────────────────────────────────
        public async Task Guardar()
        {
            // Validar monto
            if (!decimal.TryParse(
                    Monto.Replace(",", "."),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var montoDecimal) || montoDecimal <= 0)
            {
                ErrorMessage = "Ingresá un monto válido mayor a cero.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                ErrorMessage = "La descripción es obligatoria.";
                return;
            }

            IsLoading   = true;
            ErrorMessage = string.Empty;

            try
            {
                if (_sesion.IdCajaActual == null)
                {
                    ErrorMessage = "No hay una caja abierta.";
                    return;
                }

                // Crear movimiento de caja (Ingreso = 1, Egreso = 2)
                var tipoMovimiento = EsIngreso 
                    ? TipoMovimientoCajaEnum.Ingreso 
                    : TipoMovimientoCajaEnum.Egreso;

                await _cajaServicio.RegistrarMovimientoAsync(
                    _sesion.IdCajaActual.Value,
                    tipoMovimiento,
                    montoDecimal,
                    Descripcion);

                Result = true;
                _onClose?.Invoke(true);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al guardar: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void Cancelar()
        {
            Result = false;
            _onClose?.Invoke(false);
        }
    }
}
