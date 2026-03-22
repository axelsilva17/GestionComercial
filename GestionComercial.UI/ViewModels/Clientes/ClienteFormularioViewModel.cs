using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Clientes
{
    public class ClienteFormularioViewModel : NavigableViewModel
    {
        private readonly IClienteServicio _clienteServicio;
        private readonly ShellViewModel _shell;
        private readonly ILogger<ClienteFormularioViewModel> _logger;

        public ClienteFormularioViewModel(IClienteServicio clienteServicio, ShellViewModel shell, ILogger<ClienteFormularioViewModel> logger)
        {
            _clienteServicio = clienteServicio;
            _shell = shell;
            _logger = logger;
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

        public string TituloFormulario    => EsModoEdicion ? "Editar Cliente"                           : "Nuevo Cliente";
        public string SubtituloFormulario => EsModoEdicion ? "Modificá los datos del cliente"           : "Completá los datos para registrar un nuevo cliente";

        public int IdCliente
        {
            get => _idCliente;
            set => _idCliente = value;
        }
        private int _idCliente;

        // ── Campos ────────────────────────────────────────────────────────────
        private string _nombre = string.Empty;
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; NotifyOfPropertyChange(() => Nombre); NotifyOfPropertyChange(() => CanGuardar); }
        }

        private string _apellido = string.Empty;
        public string Apellido
        {
            get => _apellido;
            set { _apellido = value; NotifyOfPropertyChange(() => Apellido); NotifyOfPropertyChange(() => CanGuardar); }
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

        private string _direccion = string.Empty;
        public string Direccion
        {
            get => _direccion;
            set { _direccion = value; NotifyOfPropertyChange(() => Direccion); }
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
            Apellido      = string.Empty;
            Documento     = string.Empty;
            Telefono      = string.Empty;
            Email         = string.Empty;
            Direccion     = string.Empty;
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

        public async Task CargarClienteAsync(int idCliente)
        {
            try
            {
                IsLoading = true;
                var dto = await _clienteServicio.ObtenerPorIdAsync(idCliente);
                if (dto != null)
                {
                    IdCliente = dto.IdCliente;
                    Nombre = dto.Nombre;
                    Apellido = ""; // No existe en ClienteDto, mantener compatibilidad
                    Documento = dto.Documento.ToString();
                    Telefono = dto.Telefono.ToString();
                    Email = dto.Email;
                    Direccion = ""; // No existe en ClienteDto
                    Activo = dto.Activo;
                    EsModoEdicion = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando cliente");
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Guardar ───────────────────────────────────────────────────────────
        public async Task<bool> Guardar()
        {
            try
            {
                IsLoading = true;
                if (EsModoEdicion)
                {
                    var dto = new ClienteActualizarDto
                    {
                        Id = IdCliente,
                        Nombre = Nombre,
                        Documento = int.TryParse(Documento, out var doc) ? doc : 0,
                        Telefono = Telefono,
                        Email = Email,
                        Activo = Activo
                    };
                    await _clienteServicio.ActualizarAsync(dto);
                }
                else
                {
                    var dto = new ClienteCrearDto
                    {
                        Nombre = Nombre,
                        Documento = int.TryParse(Documento, out var doc) ? doc : 0,
                        Telefono = Telefono,
                        Email = Email,
                        IdEmpresa = _shell.IdEmpresaActual,
                        Activo = Activo
                    };
                    await _clienteServicio.CrearAsync(dto);
                }
                MessageBox.Show("Cliente guardado correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                await Volver();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando cliente");
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task Volver()
            => await _shell.ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(), CancellationToken.None);
    }
}
