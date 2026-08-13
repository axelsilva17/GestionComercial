using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.ViewModels.Caja;
using GestionComercial.UI.ViewModels.Clientes;
using GestionComercial.UI.ViewModels.Compras;
using GestionComercial.UI.ViewModels.Configuracion;
using GestionComercial.UI.ViewModels.Inventario;
using GestionComercial.UI.ViewModels.Productos;
using GestionComercial.UI.ViewModels.Proveedores;
using GestionComercial.UI.ViewModels.Reportes;
using GestionComercial.UI.ViewModels.Ventas;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Main
{
    public class ShellViewModel : Conductor<object>
    {
        private string     _usuarioNombre   = "";
        private string     _usuarioRol      = "";
        private string     _usuarioSucursal = "";
        private RolUsuario _rol             = RolUsuario.Vendedor;

        public string UsuarioNombre
        {
            get => _usuarioNombre;
            set { _usuarioNombre = value; NotifyOfPropertyChange(() => UsuarioNombre); NotifyOfPropertyChange(() => UsuarioInicial); }
        }
        public string UsuarioRol
        {
            get => _usuarioRol;
            set { _usuarioRol = value; NotifyOfPropertyChange(() => UsuarioRol); }
        }
        public string UsuarioSucursal
        {
            get => _usuarioSucursal;
            set { _usuarioSucursal = value; NotifyOfPropertyChange(() => UsuarioSucursal); }
        }
        public RolUsuario Rol
        {
            get => _rol;
            set
            {
                _rol = value;
                NotifyOfPropertyChange(() => Rol);
                NotifyOfPropertyChange(() => EsGerente);
                NotifyOfPropertyChange(() => EsAdministrador);
                NotifyOfPropertyChange(() => EsVendedor);
                NotifyOfPropertyChange(() => MostrarVentas);
                NotifyOfPropertyChange(() => MostrarCaja);
                NotifyOfPropertyChange(() => MostrarCompras);
                NotifyOfPropertyChange(() => MostrarCatalogo);
                NotifyOfPropertyChange(() => MostrarProductos);
                NotifyOfPropertyChange(() => MostrarInventario);
                NotifyOfPropertyChange(() => MostrarClientes);
                NotifyOfPropertyChange(() => MostrarProveedores);
                NotifyOfPropertyChange(() => MostrarReportes);
                NotifyOfPropertyChange(() => MostrarConfiguracion);
            }
        }

        public string UsuarioInicial =>
            string.IsNullOrEmpty(UsuarioNombre) ? "?" : UsuarioNombre[0].ToString().ToUpper();

        // ── Identidad ─────────────────────────────────────────────────────────
        public bool EsGerente       => Rol == RolUsuario.Gerente;
        public bool EsAdministrador => Rol == RolUsuario.Administrador;
        public bool EsVendedor      => Rol == RolUsuario.Vendedor;

        // ── Helper ────────────────────────────────────────────────────────────
        private bool HasPermission(string codigo) =>
            SesionActual.Permisos?.Contains(codigo) == true;

        // ── Visibilidad módulos (basada en permisos) ──────────────────────────
        public bool MostrarVentas       => HasPermission("Ventas.Ver");
        public bool MostrarCaja         => HasPermission("Caja.Abrir");
        public bool MostrarCompras      => HasPermission("Compras.Ver");
        public bool MostrarCatalogo     => HasPermission("Productos.Ver");
        public bool MostrarProductos    => HasPermission("Productos.Ver");
        public bool MostrarInventario   => HasPermission("Productos.Ver");
        public bool MostrarClientes     => HasPermission("Clientes.Ver");
        public bool MostrarProveedores  => HasPermission("Compras.Ver");
        public bool MostrarReportes     => HasPermission("Reportes.Ver");
        public bool MostrarConfiguracion => HasPermission("Configuracion.Ver");


        public int              IdEmpresaActual  { get; internal set; }
        public int              IdSucursalActual { get; internal set; }
        public UsuarioSesionDto SesionActual     { get; set; } = new();

        // ── Configurar sesión ─────────────────────────────────────────────────
        public void ConfigurarSesion(string nombre, string rol, string sucursal, UsuarioSesionDto sesion)
        {
            SesionActual    = sesion;
            UsuarioSucursal = sucursal;
            Rol = rol?.ToLower() switch
            {
                "gerente" or "dueno" or "owner" => RolUsuario.Gerente,
                "administrador" or "admin"       => RolUsuario.Administrador,
                _                                => RolUsuario.Vendedor,
            };
            UsuarioRol = Rol switch
            {
                RolUsuario.Gerente       => "Gerente",
                RolUsuario.Administrador => "Administrador",
                _                        => "Vendedor",
            };
            UsuarioNombre = nombre;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await IrDashboard();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar dashboard: {ex.Message}");
            }
        }

        // ── Navegación ────────────────────────────────────────────────────────
        public async Task IrDashboard()     => await ActivateItemAsync(IoC.Get<DashboardViewModel>(),        CancellationToken.None);
        public async Task IrVentas()        => await ActivateItemAsync(IoC.Get<VentaViewModel>(),            CancellationToken.None);
        public async Task IrVentasPendientes()
        {
            var vm = IoC.Get<VentaListadoViewModel>();
            vm.FiltroEstado = "Pendiente";
            await ActivateItemAsync(vm, CancellationToken.None);
        }
        public async Task IrCompras()       => await ActivateItemAsync(IoC.Get<CompraListadoViewModel>(),    CancellationToken.None);
        public async Task IrCaja()          => await ActivateItemAsync(IoC.Get<CajaViewModel>(),             CancellationToken.None);
        public async Task IrProductos()
        {
            // Resetear filtro de stock crítico al navegar normal
            var vm = IoC.Get<ProductoListadoViewModel>();
            vm.MostrarSoloStockCritico = false;
            await ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task IrProductosStockCritico()
        {
            var vm = IoC.Get<ProductoListadoViewModel>();
            vm.MostrarSoloStockCritico = true;
            await ActivateItemAsync(vm, CancellationToken.None);
        }
        public async Task IrInventario()    => await ActivateItemAsync(IoC.Get<InventarioViewModel>(),       CancellationToken.None);
        public async Task IrClientes()      => await ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(),   CancellationToken.None);
        public async Task IrProveedores()   => await ActivateItemAsync(IoC.Get<ProveedorListadoViewModel>(), CancellationToken.None);
        public async Task IrConfiguracion() => await ActivateItemAsync(IoC.Get<ConfiguracionViewModel>(),    CancellationToken.None);

        // Reportes diferenciados por rol
        public async Task IrReportes()
        {
            if (EsGerente)
                await ActivateItemAsync(IoC.Get<ReporteGerenciaViewModel>(), CancellationToken.None);
            else
                await ActivateItemAsync(IoC.Get<ReporteAdminViewModel>(),    CancellationToken.None);
        }

        public void CerrarSesion()
        {
            var login = IoC.Get<LoginViewModel>();
            var wm    = IoC.Get<IWindowManager>();
            wm.ShowWindowAsync(login);
            TryCloseAsync();
        }
    }
}
