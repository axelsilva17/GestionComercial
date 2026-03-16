using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Clientes
{
    public class ClienteFormularioViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        public ClienteFormularioViewModel(ShellViewModel shell) { _shell = shell; }

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

        public string TituloFormulario    => EsModoEdicion ? "Editar Cliente"                           : "Nuevo Cliente";
        public string SubtituloFormulario => EsModoEdicion ? "Modificá los datos del cliente"           : "Completá los datos para registrar un nuevo cliente";

        private int _idCliente;

        // ── Campos ────────────────────────────────────────────────────────────
        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; NotifyOfPropertyChange(() => Nombre); NotifyOfPropertyChange(() => CanGuardar); }
        }

        private string _documento = string.Empty;
        public string Documento
        {
            get => _documento;
            set { _documento = value; NotifyOfPropertyChange(() => Documento); NotifyOfPropertyChange(() => CanGuardar); }
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
            set { _activo = value; NotifyOfPropertyChange(() => Activo); }
        }

        // ── Validación ────────────────────────────────────────────────────────
        public bool EmailValido => string.IsNullOrWhiteSpace(Email) || Email.Contains("@");
        public bool CanGuardar  => !string.IsNullOrWhiteSpace(Nombre)
                                && !string.IsNullOrWhiteSpace(Documento)
                                && EmailValido
                                && !IsLoading;

        // ── Inicialización ────────────────────────────────────────────────────
        public void InicializarParaCrear()
        {
            EsModoEdicion = false;
            _idCliente    = 0;
            Nombre        = string.Empty;
            Documento     = string.Empty;
            Telefono      = string.Empty;
            Email         = string.Empty;
            Activo        = true;
            LimpiarError();
        }

        public void InicializarParaEditar(int idCliente)
        {
            EsModoEdicion = true;
            _idCliente    = idCliente;
            LimpiarError();
            _ = CargarClienteAsync(idCliente);
        }

        private async Task CargarClienteAsync(int idCliente)
        {
            IsLoading = true;
            try
            {
                await Task.Delay(150);
                // TODO: var dto = await _clienteServicio.ObtenerAsync(idCliente);
                // Nombre    = dto.Nombre;
                // Documento = dto.Documento.ToString();
                // Telefono  = dto.Telefono;
                // Email     = dto.Email;
                // Activo    = dto.Activo;
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
                await Task.Delay(400); // TODO: llamar servicio
                // if (EsModoEdicion)
                //     await _clienteServicio.ActualizarAsync(new ClienteActualizarDto { ... });
                // else
                //     await _clienteServicio.CrearAsync(new ClienteCrearDto { ... });
                await Volver();
            }
            catch (System.Exception ex) { MostrarError($"Error al guardar: {ex.Message}"); }
            finally { IsLoading = false; }
        }

        public async Task Volver()
            => await _shell.ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(), CancellationToken.None);
    }
}
