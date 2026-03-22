using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GestionComercial.UI.ViewModels.Clientes
{
    public class ClienteListadoViewModel : NavigableViewModel
    {
        private readonly IClienteServicio _clienteServicio;
        private readonly ShellViewModel _shell;
        private readonly ILogger<ClienteListadoViewModel> _logger;

        public ClienteListadoViewModel(IClienteServicio clienteServicio, ShellViewModel shell, ILogger<ClienteListadoViewModel> logger)
        {
            _clienteServicio = clienteServicio;
            _shell = shell;
            _logger = logger;
            Titulo = "Clientes";
            Subtitulo = "Gestión de clientes";
        }

        // ── Métricas ──────────────────────────────────────────────────────────
        private int _totalClientes;
        public int TotalClientes
        {
            get => _totalClientes;
            set { _totalClientes = value; NotifyOfPropertyChange(() => TotalClientes); }
        }

        private int _clientesActivos;
        public int ClientesActivos
        {
            get => _clientesActivos;
            set { _clientesActivos = value; NotifyOfPropertyChange(() => ClientesActivos); }
        }

        private int _clientesInactivos;
        public int ClientesInactivos
        {
            get => _clientesInactivos;
            set { _clientesInactivos = value; NotifyOfPropertyChange(() => ClientesInactivos); }
        }

        private int _clientesConVentas;
        public int ClientesConVentas
        {
            get => _clientesConVentas;
            set { _clientesConVentas = value; NotifyOfPropertyChange(() => ClientesConVentas); }
        }

        // ── Lista ─────────────────────────────────────────────────────────────
        private ObservableCollection<ClienteDto> _clientes = new();
        public ObservableCollection<ClienteDto> Clientes
        {
            get => _clientes;
            set { _clientes = value; NotifyOfPropertyChange(() => Clientes); }
        }

        private ClienteDto _clienteSeleccionado;
        public ClienteDto ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set { _clienteSeleccionado = value; NotifyOfPropertyChange(() => ClienteSeleccionado); }
        }

        // ── Filtros ───────────────────────────────────────────────────────────
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        // ── Paginación ────────────────────────────────────────────────────────
        private int _clientesMostrados;
        public int ClientesMostrados
        {
            get => _clientesMostrados;
            set { _clientesMostrados = value; NotifyOfPropertyChange(() => ClientesMostrados); }
        }

        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; NotifyOfPropertyChange(() => PaginaActual); }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set { _totalPaginas = value; NotifyOfPropertyChange(() => TotalPaginas); }
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarAsync();

        private async Task CargarAsync()
        {
            try
            {
                IsLoading = true;
                var clientes = await _clienteServicio.ObtenerTodosAsync(_shell.IdEmpresaActual);
                Clientes = new ObservableCollection<ClienteDto>(clientes);
                TotalClientes = Clientes.Count;
                ClientesActivos = Clientes.Count(c => c.Activo);
                ClientesInactivos = Clientes.Count(c => !c.Activo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando clientes");
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task Buscar() { PaginaActual = 1; await CargarAsync(); }

        public async Task NuevoCliente()
        {
            var shell = IoC.Get<ShellViewModel>();
            var vm = IoC.Get<ClienteFormularioViewModel>();
            vm.InicializarParaCrear();
            await shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task EditarCliente()
        {
            if (ClienteSeleccionado == null) return;
            var shell = IoC.Get<ShellViewModel>();
            var vm = IoC.Get<ClienteFormularioViewModel>();
            vm.InicializarParaEditar(ClienteSeleccionado.IdCliente);
            await shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public void CerrarDetalle() => ClienteSeleccionado = null;

        public async Task DesactivarCliente()
        {
            if (ClienteSeleccionado == null) return;
            var result = MessageBox.Show($"¿Desactivar cliente {ClienteSeleccionado.Nombre}?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                await _clienteServicio.DesactivarAsync(ClienteSeleccionado.IdCliente);
                await CargarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desactivando cliente");
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Paginación ────────────────────────────────────────────────────────
        public bool CanPaginaAnterior => PaginaActual > 1;
        public bool CanPaginaSiguiente => PaginaActual < TotalPaginas;

        public async Task PaginaAnterior()
        {
            if (PaginaActual > 1) { PaginaActual--; await CargarAsync(); }
            NotifyOfPropertyChange(() => CanPaginaAnterior);
            NotifyOfPropertyChange(() => CanPaginaSiguiente);
        }

        public async Task PaginaSiguiente()
        {
            if (PaginaActual < TotalPaginas) { PaginaActual++; await CargarAsync(); }
            NotifyOfPropertyChange(() => CanPaginaAnterior);
            NotifyOfPropertyChange(() => CanPaginaSiguiente);
        }
    }
}