using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.ViewModels.Ventas;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Clientes
{
    public class ClienteFormularioViewModel : FormularioEntidadViewModel
    {
        private readonly IClienteServicio _clienteServicio;
        private readonly ILogger<ClienteFormularioViewModel> _logger;

        public ClienteFormularioViewModel(IClienteServicio clienteServicio, ShellViewModel shell, ILogger<ClienteFormularioViewModel> logger)
            : base(shell)
        {
            _clienteServicio = clienteServicio;
            _logger = logger;
        }

        public VentaViewModel? VentaOrigen { get; set; }

        // ── Títulos ───────────────────────────────────────────────────────────
        public override string TituloFormulario    => EsModoEdicion ? "Editar Cliente"                     : "Nuevo Cliente";
        public override string SubtituloFormulario => EsModoEdicion ? "Modificá los datos del cliente"     : "Completá los datos para registrar un nuevo cliente";

        // ── Campos propios de Cliente ──────────────────────────────────────────
        private int _idCliente;
        public int IdCliente
        {
            get => _idCliente;
            set => _idCliente = value;
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

        private string _direccion = string.Empty;
        public string Direccion
        {
            get => _direccion;
            set { _direccion = value; NotifyOfPropertyChange(() => Direccion); }
        }

        // ── Validación ────────────────────────────────────────────────────────
        public override bool CanGuardar => !string.IsNullOrWhiteSpace(Nombre)
                                        && !string.IsNullOrWhiteSpace(Documento)
                                        && EmailValido
                                        && !IsLoading;

        // ── Inicialización ────────────────────────────────────────────────────
        public void InicializarParaCrear()
        {
            EsModoEdicion = false;
            _idCliente    = 0;
            Apellido      = string.Empty;
            Documento     = string.Empty;
            Direccion     = string.Empty;
            LimpiarCamposComunes();
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
                    Nombre    = dto.Nombre;
                    Apellido  = "";
                    Documento = dto.Documento.ToString();
                    Telefono  = dto.Telefono.ToString();
                    Email     = dto.Email;
                    Direccion = "";
                    Activo    = dto.Activo;
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
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MostrarError("El nombre del cliente es obligatorio.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Documento))
            {
                MostrarError("El documento del cliente es obligatorio.");
                return false;
            }
            if (!int.TryParse(Documento, out var docValidado) || docValidado <= 0)
            {
                MostrarError("Ingresá un número de documento válido.");
                return false;
            }

            try
            {
                IsLoading = true;
                LimpiarError();
                if (EsModoEdicion)
                {
                    var dto = new ClienteActualizarDto
                    {
                        Id        = IdCliente,
                        Nombre    = Nombre,
                        Documento = int.TryParse(Documento, out var doc) ? doc : 0,
                        Telefono  = Telefono,
                        Email     = Email,
                        Activo    = Activo
                    };
                    await _clienteServicio.ActualizarAsync(dto);
                }
                else
                {
                    var dto = new ClienteCrearDto
                    {
                        Nombre    = Nombre,
                        Documento = int.TryParse(Documento, out var doc) ? doc : 0,
                        Telefono  = Telefono,
                        Email     = Email,
                        IdEmpresa = Shell.IdEmpresaActual,
                        Activo    = Activo
                    };
                    await _clienteServicio.CrearAsync(dto);
                }
                MessageBox.Show("Cliente guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
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
        {
            if (VentaOrigen != null)
                await Shell.ActivateItemAsync(VentaOrigen, CancellationToken.None);
            else
                await Shell.ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(), CancellationToken.None);
        }
    }
}
