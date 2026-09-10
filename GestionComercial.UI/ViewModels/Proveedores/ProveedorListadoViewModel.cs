using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Compras;
using GestionComercial.UI.ViewModels.Main;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    public class ProveedorListadoViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;
        private readonly IProveedorServicio _proveedorServicio;
        private readonly SesionServicio _sesion;

        public ProveedorListadoViewModel(ShellViewModel shell, IProveedorServicio proveedorServicio, SesionServicio sesion)
        {
            _shell = shell;
            _proveedorServicio = proveedorServicio;
            _sesion = sesion;
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
            set 
            { 
                _textoBusqueda = value; 
                NotifyOfPropertyChange(() => TextoBusqueda);
                PaginaActual = 1;
                _ = CargarAsync();
            }
        }

        // Filtro de estado (0=Todos, 1=Activos, 2=Inactivos)
        private int _filtroEstado = 0;
        public int FiltroEstado
        {
            get => _filtroEstado;
            set 
            { 
                _filtroEstado = value; 
                NotifyOfPropertyChange(() => FiltroEstado);
                PaginaActual = 1;
                _ = CargarAsync();
            }
        }

        public string FiltroEstadoTexto => FiltroEstado switch
        {
            1 => "Activos",
            2 => "Inactivos",
            _ => "Todos"
        };

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
                // Cargar proveedores desde el servicio
                var todosProveedores = await _proveedorServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                
                // Aplicar filtros
                var filtered = todosProveedores.AsEnumerable();
                
                // Filtro por estado
                if (FiltroEstado == 1)
                    filtered = filtered.Where(p => p.Activo);
                else if (FiltroEstado == 2)
                    filtered = filtered.Where(p => !p.Activo);
                
                // Filtro por búsqueda de nombre
                if (!string.IsNullOrWhiteSpace(TextoBusqueda))
                    filtered = filtered.Where(p => p.Nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase));
                
                var lista = filtered.Select(p => new ProveedorItemDto
                {
                    IdProveedor = p.Id,
                    Nombre = p.Nombre,
                    Telefono = p.Telefono ?? string.Empty,
                    Email = p.Email ?? string.Empty,
                    Activo = p.Activo,
                    TotalCompras = p.CantidadCompras
                }).ToList();
                
                Proveedores           = new ObservableCollection<ProveedorItemDto>(lista);
                TotalProveedores      = lista.Count;
                ProveedoresActivos    = lista.Count(p => p.Activo);
                ProveedoresInactivos  = lista.Count(p => !p.Activo);
                ProveedoresMostrados  = lista.Count;
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

            var resultado = MessageBox.Show(
                $"¿Está seguro que desea desactivar al proveedor '{ProveedorSeleccionado.Nombre}'?",
                "Confirmar desactivación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado != MessageBoxResult.Yes) return;

            try
            {
                await _proveedorServicio.DesactivarAsync(ProveedorSeleccionado.IdProveedor);
                await CargarAsync();
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
        }

        public async Task ActivarProveedor()
        {
            if (ProveedorSeleccionado == null) return;
            try
            {
                var proveedor = await _proveedorServicio.ObtenerPorIdAsync(ProveedorSeleccionado.IdProveedor);
                if (proveedor != null)
                {
                    proveedor.Activo = true;
                    await _proveedorServicio.ActualizarAsync(proveedor);
                    await CargarAsync();
                }
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
        }

        public async Task VerProveedor()
        {
            if (ProveedorSeleccionado == null) return;
            var vm = IoC.Get<ProveedorDetalleViewModel>();
            vm.Inicializar(ProveedorSeleccionado.IdProveedor);
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task VerCompras()
        {
            if (ProveedorSeleccionado == null) return;
            
            var compraListado = IoC.Get<CompraListadoViewModel>();
            
            // Use default date range (last 30 days) and filter by provider only
            // To show all purchases, we'd need to adjust the default or add a "Sin filtro" option
            
            // Precargar proveedores para poder aplicar el filtro
            var todosProveedores = await compraListado.CargarProveedoresAsync();
            
            var proveedorEnCompras = todosProveedores
                .FirstOrDefault(p => p.IdProveedor == ProveedorSeleccionado.IdProveedor);
            
            if (proveedorEnCompras != null)
            {
                compraListado.ProveedorFiltro = proveedorEnCompras;
            }
            
            await _shell.ActivateItemAsync(compraListado, CancellationToken.None);
        }

        public async Task HacerCompra()
        {
            if (ProveedorSeleccionado == null) return;

            var compra = IoC.Get<CompraViewModel>();
            // La vista Nueva Compra marca a este proveedor como seleccionado
            compra.PreSeleccionarProveedor(ProveedorSeleccionado);
            await _shell.ActivateItemAsync(compra, CancellationToken.None);
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
