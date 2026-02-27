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
    public class ClienteListadoViewModel(ShellViewModel shell) : NavigableViewModel
    {
        private readonly ShellViewModel _shell = shell;

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
            try
            {
                await Task.Delay(200); // TODO: var result = await _clienteServicio.ListarAsync(...)

                var mock = new System.Collections.Generic.List<ClienteItemDto>
                {
                    new() { IdCliente=1, Nombre="Juan Pérez",       Documento=28456789, Telefono="3794-421000", Email="juan.perez@gmail.com",    Activo=true,  TotalVentas=14 },
                    new() { IdCliente=2, Nombre="María González",   Documento=32100456, Telefono="3794-455200", Email="mgonzalez@hotmail.com",    Activo=true,  TotalVentas=6  },
                    new() { IdCliente=3, Nombre="Carlos Rodríguez", Documento=25789012, Telefono="11-4523-0011",Email="carlos.r@empresa.com",     Activo=true,  TotalVentas=22 },
                    new() { IdCliente=4, Nombre="Ana Martínez",     Documento=40123789, Telefono="3794-480100", Email="ana.martinez@yahoo.com",   Activo=false, TotalVentas=1  },
                    new() { IdCliente=5, Nombre="Roberto Sánchez",  Documento=18900345, Telefono="3794-499200", Email="roberto.s@gmail.com",      Activo=true,  TotalVentas=9  },
                    new() { IdCliente=6, Nombre="Laura Fernández",  Documento=35678901, Telefono="11-4788-5566",Email="lfernandez@outlook.com",   Activo=true,  TotalVentas=0  },
                    new() { IdCliente=7, Nombre="Diego López",      Documento=29345678, Telefono="3794-412300", Email="dlopez@gmail.com",         Activo=false, TotalVentas=3  },
                };

                Clientes           = new ObservableCollection<ClienteItemDto>(mock);
                TotalClientes      = mock.Count;
                ClientesActivos    = mock.Count(c => c.Activo);
                ClientesInactivos  = mock.Count(c => !c.Activo);
                ClientesConVentas  = mock.Count(c => c.TotalVentas > 0);
                ClientesMostrados  = mock.Count;
                TotalPaginas       = 1;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task Buscar() { PaginaActual = 1; await CargarAsync(); }

        public async Task NuevoCliente()
        {
            var vm = IoC.Get<ClienteFormularioViewModel>();
            vm.InicializarParaCrear();
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task EditarCliente()
        {
            if (ClienteSeleccionado == null) return;
            var vm = IoC.Get<ClienteFormularioViewModel>();
            vm.InicializarParaEditar(ClienteSeleccionado.IdCliente);
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public void CerrarDetalle() => ClienteSeleccionado = null;

        public async Task DesactivarCliente()
        {
            if (ClienteSeleccionado == null) return;
            // TODO: await _clienteServicio.DesactivarAsync(ClienteSeleccionado.IdCliente);
            await CargarAsync();
        }

        // ── Paginación ────────────────────────────────────────────────────────
        public bool CanPaginaAnterior  => PaginaActual > 1;
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
