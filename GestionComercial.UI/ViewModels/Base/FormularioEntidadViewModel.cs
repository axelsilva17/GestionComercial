using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;

namespace GestionComercial.UI.ViewModels
{
    public abstract class FormularioEntidadViewModel : NavigableViewModel
    {
        protected readonly ShellViewModel Shell;

        protected FormularioEntidadViewModel(ShellViewModel shell)
        {
            Shell = shell;
        }

        // ── Modo ──────────────────────────────────────────────────────────────
        private bool _esModoEdicion;
        public bool EsModoEdicion
        {
            get => _esModoEdicion;
            set
            {
                _esModoEdicion = value;
                NotifyOfPropertyChange(() => EsModoEdicion);
                NotifyOfPropertyChange(() => TituloFormulario);
                NotifyOfPropertyChange(() => SubtituloFormulario);
            }
        }

        public abstract string TituloFormulario { get; }
        public abstract string SubtituloFormulario { get; }

        // ── Campos comunes ────────────────────────────────────────────────────
        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; NotifyOfPropertyChange(() => Nombre); NotifyOfPropertyChange(() => CanGuardar); }
        }

        private string _telefono = string.Empty;
        public string Telefono
        {
            get => _telefono;
            set { _telefono = value; NotifyOfPropertyChange(() => Telefono); }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { _email = value; NotifyOfPropertyChange(() => Email); NotifyOfPropertyChange(() => EmailValido); NotifyOfPropertyChange(() => CanGuardar); }
        }

        private bool _activo = true;
        public bool Activo
        {
            get => _activo;
            set { _activo = value; NotifyOfPropertyChange(() => Activo); NotifyOfPropertyChange(() => CanGuardar); }
        }

        // ── Validación email ──────────────────────────────────────────────────
        public bool EmailValido  => string.IsNullOrWhiteSpace(Email) || Email.Contains("@");
        public bool EmailInvalido => !EmailValido;

        // ── Guardar ───────────────────────────────────────────────────────────
        public abstract bool CanGuardar { get; }

        protected void LimpiarCamposComunes()
        {
            Nombre   = string.Empty;
            Telefono = string.Empty;
            Email    = string.Empty;
            Activo   = true;
            LimpiarError();
        }
    }
}
