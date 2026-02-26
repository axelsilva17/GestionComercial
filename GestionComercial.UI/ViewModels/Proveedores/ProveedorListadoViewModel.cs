using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    public class ProveedorListadoViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        public ProveedorListadoViewModel(ShellViewModel shell)
        {
            _shell    = shell;
            Titulo    = "Proveedores";
            Subtitulo = "Gestión de proveedores";
        }

        // ── Métricas ──────────────────────────────────────────────────────────
        private int _totalProveedores;
        public int TotalProveedores
        {
            get => _totalProveedores;
            set { _totalProveedores = value; NotifyOfPropertyChange(() => TotalProveedores); }
        }

        private int _proveedoresActivos;
        public int ProveedoresActivos
        {
            get => _proveedoresActivos;
            set { _proveedoresActivos = value; NotifyOfPropertyChange(() => ProveedoresActivos); }
        }

        private int _proveedoresInactivos;
        public int ProveedoresInactivos
        {
            get => _proveedoresInactivos;
            set { _proveedoresInactivos = value; NotifyOfPropertyChange(() => ProveedoresInactivos); }
        }

        // ── Lista ─────────────────────────────────────────────────────────────
        private ObservableCollection<ProveedorItemDto> _proveedores = new();
        public ObservableCollection<ProveedorItemDto> Proveedores
        {
            get => _proveedores;
            set { _proveedores = value; NotifyOfPropertyChange(() => Proveedores); }
        }

        // ── Selección (sidebar) ───────────────────────────────────────────────
        private ProveedorItemDto _proveedorSeleccionado;
        public ProveedorItemDto ProveedorSeleccionado
        {
            get => _proveedorSeleccionado;
            set { _proveedorSeleccionado = value; NotifyOfPropertyChange(() => ProveedorSeleccionado); }
        }

        // ── Filtros ───────────────────────────────────────────────────────────
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; NotifyOfPropertyChange(() => TextoBusqueda); }
        }

        // ── Paginación ────────────────────────────────────────────────────────
        private int _proveedoresMostrados;
        public int ProveedoresMostrados
        {
            get => _proveedoresMostrados;
            set { _proveedoresMostrados = value; NotifyOfPropertyChange(() => ProveedoresMostrados); }
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
                await Task.Delay(200); // TODO: reemplazar con servicio real

                // Datos mock para probar la UI
                var mock = new System.Collections.Generic.List<ProveedorItemDto>
                {
                    new() { IdProveedor=1, Nombre="Distribuidora Norte S.A.",   Telefono="3794-421000", Email="ventas@dnorte.com",    Activo=true,  TotalCompras=12 },
                    new() { IdProveedor=2, Nombre="Tech Supplies Argentina",     Telefono="11-4523-9900", Email="info@techsup.com.ar", Activo=true,  TotalCompras=8  },
                    new() { IdProveedor=3, Nombre="Papelera del Litoral",        Telefono="3794-455600", Email="pedidos@papelitoral.com", Activo=true, TotalCompras=25 },
                    new() { IdProveedor=4, Nombre="Electro Import SRL",          Telefono="11-4788-0011", Email="compras@electroimport.com", Activo=false, TotalCompras=3 },
                    new() { IdProveedor=5, Nombre="Mayorista Central Corrientes", Telefono="3794-480200", Email="mayorista@central.com", Activo=true, TotalCompras=17 },
                };

                Proveedores           = new ObservableCollection<ProveedorItemDto>(mock);
                TotalProveedores      = mock.Count;
                ProveedoresActivos    = mock.Count(p => p.Activo);
                ProveedoresInactivos  = mock.Count(p => !p.Activo);
                ProveedoresMostrados  = mock.Count;
                TotalPaginas          = 1;
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public async Task Buscar()
        {
            PaginaActual = 1;
            await CargarAsync();
        }

        public async Task NuevoProveedor()
        {
            var vm = IoC.Get<ProveedorFormularioViewModel>();
            vm.InicializarParaCrear();
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task EditarProveedor()
        {
            if (ProveedorSeleccionado == null) return;
            var vm = IoC.Get<ProveedorFormularioViewModel>();
            vm.InicializarParaEditar(ProveedorSeleccionado.IdProveedor);
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public void CerrarDetalle() => ProveedorSeleccionado = null;

        public async Task DesactivarProveedor()
        {
            if (ProveedorSeleccionado == null) return;
            // TODO: await _proveedorServicio.DesactivarAsync(ProveedorSeleccionado.IdProveedor);
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
