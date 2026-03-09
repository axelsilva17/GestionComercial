using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Clientes
{
    public class ClienteListadoViewModel : NavigableViewModel
    {
        // Sin parámetro en constructor — ShellViewModel se resuelve en el momento
        // de navegar usando IoC.Get<>() para evitar problemas con SimpleContainer

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
        private ObservableCollection<ClienteItemDto> _clientes = new();
        public ObservableCollection<ClienteItemDto> Clientes
        {
            get => _clientes;
            set { _clientes = value; NotifyOfPropertyChange(() => Clientes); }
        }

        private ClienteItemDto _clienteSeleccionado;
        public ClienteItemDto ClienteSeleccionado
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
            IsLoading = true;
            LimpiarError();

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
            // TODO: await _clienteServicio.DesactivarAsync(ClienteSeleccionado.IdCliente);
            await CargarAsync();
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