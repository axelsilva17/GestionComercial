using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    public class ProveedorFormularioViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        public ProveedorFormularioViewModel(ShellViewModel shell)
        {
            _shell = shell;
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

        public string TituloFormulario    => EsModoEdicion ? "Editar Proveedor"           : "Nuevo Proveedor";
        public string SubtituloFormulario => EsModoEdicion ? "Modificá los datos del proveedor" : "Completá los datos para registrar un nuevo proveedor";

        private int _idProveedor;

        // ── Campos ───────────────────────────────────────────────────────────
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
            set { _email = value; NotifyOfPropertyChange(() => Email); NotifyOfPropertyChange(() => CanGuardar); }
        }

        private bool _activo = true;
        public bool Activo
        {
            get => _activo;
            set { _activo = value; NotifyOfPropertyChange(() => Activo); }
        }

        // ── Validación básica de email ────────────────────────────────────────
        public bool EmailValido => string.IsNullOrWhiteSpace(Email) || Email.Contains("@");
        public bool CanGuardar  => !string.IsNullOrWhiteSpace(Nombre) && EmailValido && !IsLoading;

        // ── Inicialización ────────────────────────────────────────────────────
        public void InicializarParaCrear()
        {
            EsModoEdicion = false;
            _idProveedor  = 0;
            Nombre        = string.Empty;
            Telefono      = string.Empty;
            Email         = string.Empty;
            Activo        = true;
            LimpiarError();
        }

        public void InicializarParaEditar(int idProveedor)
        {
            EsModoEdicion = true;
            _idProveedor  = idProveedor;
            LimpiarError();
            _ = CargarProveedorAsync(idProveedor);
        }

        private async Task CargarProveedorAsync(int idProveedor)
        {
            IsLoading = true;
            try
            {
                await Task.Delay(150); // TODO: var dto = await _proveedorServicio.ObtenerAsync(idProveedor);
                // Nombre   = dto.Nombre;
                // Telefono = dto.Telefono;
                // Email    = dto.Email;
                // Activo   = dto.Activo;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Guardar ───────────────────────────────────────────────────────────
        public async void Guardar()
        {
            if (!CanGuardar) return;
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(400); // TODO: llamar al servicio
                await Volver();
            }
            catch (System.Exception ex) { MostrarError($"Error al guardar: {ex.Message}"); }
            finally { IsLoading = false; }
        }

        public async Task Volver()
        {
            var listado = IoC.Get<ProveedorListadoViewModel>();
            await _shell.ActivateItemAsync(listado, CancellationToken.None);
        }
    }
}
