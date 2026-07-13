using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;

namespace GestionComercial.UI.ViewModels.Configuracion
{
    public class ConfiguracionInicialViewModel : ViewModelBase
    {
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly SesionServicio _sesionServicio;
        private readonly IWindowManager _windowManager;

        public ConfiguracionInicialViewModel(
            IUsuarioServicio usuarioServicio,
            SesionServicio sesionServicio,
            IWindowManager windowManager)
        {
            _usuarioServicio = usuarioServicio;
            _sesionServicio = sesionServicio;
            _windowManager = windowManager;
        }

        // ── Admin ──
        private string _nombreAdmin = string.Empty;
        public string NombreAdmin
        {
            get => _nombreAdmin;
            set { _nombreAdmin = value; NotifyOfPropertyChange(() => NombreAdmin); NotifyOfPropertyChange(() => CanGuardarCommand); }
        }

        private string _emailAdmin = string.Empty;
        public string EmailAdmin
        {
            get => _emailAdmin;
            set { _emailAdmin = value; NotifyOfPropertyChange(() => EmailAdmin); NotifyOfPropertyChange(() => CanGuardarCommand); }
        }

        private string _passwordAdmin = string.Empty;
        public string PasswordAdmin
        {
            get => _passwordAdmin;
            set { _passwordAdmin = value; NotifyOfPropertyChange(() => PasswordAdmin); NotifyOfPropertyChange(() => CanGuardarCommand); }
        }

        // ── Gerente ──
        private string _nombreGerente = string.Empty;
        public string NombreGerente
        {
            get => _nombreGerente;
            set { _nombreGerente = value; NotifyOfPropertyChange(() => NombreGerente); NotifyOfPropertyChange(() => CanGuardarCommand); }
        }

        private string _emailGerente = string.Empty;
        public string EmailGerente
        {
            get => _emailGerente;
            set { _emailGerente = value; NotifyOfPropertyChange(() => EmailGerente); NotifyOfPropertyChange(() => CanGuardarCommand); }
        }

        private string _passwordGerente = string.Empty;
        public string PasswordGerente
        {
            get => _passwordGerente;
            set { _passwordGerente = value; NotifyOfPropertyChange(() => PasswordGerente); NotifyOfPropertyChange(() => CanGuardarCommand); }
        }

        // ── PasswordBox bridging (same pattern as LoginViewModel) ──
        public void SetPasswordAdmin(string password) => PasswordAdmin = password;
        public void SetPasswordGerente(string password) => PasswordGerente = password;

        // ── Validation ──
        public bool CanGuardarCommand =>
            !string.IsNullOrWhiteSpace(NombreAdmin) &&
            !string.IsNullOrWhiteSpace(EmailAdmin) &&
            !string.IsNullOrWhiteSpace(PasswordAdmin) &&
            !string.IsNullOrWhiteSpace(NombreGerente) &&
            !string.IsNullOrWhiteSpace(EmailGerente) &&
            !string.IsNullOrWhiteSpace(PasswordGerente) &&
            !IsLoading;

        // ── Save command ──
        public async Task GuardarCommand()
        {
            IsLoading = true;
            LimpiarError();
            NotifyOfPropertyChange(() => CanGuardarCommand);

            try
            {
                // Validate passwords are at least 8 characters
                if (PasswordAdmin.Length < 8)
                {
                    MostrarError("La contraseña del admin debe tener al menos 8 caracteres.");
                    return;
                }
                if (PasswordGerente.Length < 8)
                {
                    MostrarError("La contraseña del gerente debe tener al menos 8 caracteres.");
                    return;
                }

                // Create Admin user (Id_rol = 2 → Administrador, Id_sucursal = 1)
                var admin = await _usuarioServicio.CrearAsync(
                    NombreAdmin, "Sistema", EmailAdmin, PasswordAdmin,
                    idRol: 2, idSucursal: 1);

                // Create Gerente user (Id_rol = 1 → Gerente, Id_sucursal = 1)
                var gerente = await _usuarioServicio.CrearAsync(
                    NombreGerente, "Sistema", EmailGerente, PasswordGerente,
                    idRol: 1, idSucursal: 1);

                // Log in the admin user
                var sesionAdmin = new UsuarioSesionDto
                {
                    IdUsuario = admin.IdUsuario,
                    Nombre = admin.Nombre,
                    Apellido = admin.Apellido,
                    Email = admin.Email,
                    Rol = admin.Rol,
                    IdSucursal = 1,
                    Sucursal = "Casa Central",
                    IdEmpresa = 1,
                    Empresa = "Mi Empresa",
                };
                _sesionServicio.IniciarSesion(sesionAdmin);

                // Navigate to login
                var login = IoC.Get<LoginViewModel>();
                await _windowManager.ShowWindowAsync(login);
                await TryCloseAsync();
            }
            catch (Exception ex)
            {
                MostrarError($"Error al crear usuarios: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                NotifyOfPropertyChange(() => CanGuardarCommand);
            }
        }
    }
}
