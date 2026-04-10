using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    public class ProveedorFormularioViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;
        private readonly IProveedorServicio _proveedorServicio;
        private readonly SesionServicio _sesion;

        public ProveedorFormularioViewModel(ShellViewModel shell, IProveedorServicio proveedorServicio, SesionServicio sesion)
        {
            _shell = shell;
            _proveedorServicio = proveedorServicio;
            _sesion = sesion;
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
            set { _email = value; NotifyOfPropertyChange(() => Email); ValidarEmail(); }
        }

        private bool _activo = true;
        public bool Activo
        {
            get => _activo;
            set { _activo = value; NotifyOfPropertyChange(() => Activo); }
        }

        // ── Validación básica de email ────────────────────────────────────────
        private bool _emailInvalido;
        public bool EmailInvalido
        {
            get => _emailInvalido;
            set { _emailInvalido = value; NotifyOfPropertyChange(() => EmailInvalido); }
        }

        public bool EmailValido => string.IsNullOrWhiteSpace(Email) || (Email.Contains("@") && Email.Contains("."));
        
        private void ValidarEmail()
        {
            if (!string.IsNullOrWhiteSpace(Email) && !EmailValido)
                EmailInvalido = true;
            else
                EmailInvalido = false;
            NotifyOfPropertyChange(() => CanGuardar);
        }

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
            EmailInvalido = false;
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
                var dto = await _proveedorServicio.ObtenerPorIdAsync(idProveedor);
                if (dto != null)
                {
                    Nombre   = dto.Nombre;
                    Telefono = dto.Telefono ?? string.Empty;
                    Email    = dto.Email ?? string.Empty;
                    Activo   = dto.Activo;
                    EmailInvalido = false;
                }
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
                if (EsModoEdicion)
                {
                    // Actualizar proveedor existente
                    var proveedor = await _proveedorServicio.ObtenerPorIdAsync(_idProveedor);
                    if (proveedor != null)
                    {
                        proveedor.Nombre = Nombre;
                        proveedor.Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono;
                        proveedor.Email = string.IsNullOrWhiteSpace(Email) ? null : Email;
                        proveedor.Activo = Activo;
                        await _proveedorServicio.ActualizarAsync(proveedor);
                    }
                }
                else
                {
                    // Crear nuevo proveedor
                    var nuevoProveedor = new Proveedor
                    {
                        Nombre = Nombre,
                        Telefono = string.IsNullOrWhiteSpace(Telefono) ? null : Telefono,
                        Email = string.IsNullOrWhiteSpace(Email) ? null : Email,
                        Id_empresa = _sesion.IdEmpresa,
                        Activo = true
                    };
                    await _proveedorServicio.CrearAsync(nuevoProveedor);
                }
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