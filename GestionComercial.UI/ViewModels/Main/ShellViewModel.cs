using Caliburn.Micro;
using GestionComercial.UI.ViewModel.Main;
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
                // Menú principal
                NotifyOfPropertyChange(() => MostrarVentas);
                NotifyOfPropertyChange(() => MostrarCompras);
                // Catálogo
                NotifyOfPropertyChange(() => MostrarProductos);
                NotifyOfPropertyChange(() => MostrarInventario);
                // Gestión
                NotifyOfPropertyChange(() => MostrarProveedores);
                NotifyOfPropertyChange(() => MostrarReportes);
                NotifyOfPropertyChange(() => MostrarConfiguracion);
            }
        }

        public string UsuarioInicial =>
            string.IsNullOrEmpty(UsuarioNombre) ? "?" : UsuarioNombre[0].ToString().ToUpper();

        // ── Identidad de rol ──────────────────────────────────────────────────
        public bool EsGerente       => Rol == RolUsuario.Gerente;
        public bool EsAdministrador => Rol == RolUsuario.Administrador;
        public bool EsVendedor      => Rol == RolUsuario.Vendedor;

        // ── Visibilidad de módulos en el menú ─────────────────────────────────
        //
        // GERENTE:        todo visible
        // ADMINISTRADOR:  sin Ventas, sin Productos
        // VENDEDOR:       solo Dashboard, Ventas, Caja, Clientes
        //
        public bool MostrarVentas       => Rol == RolUsuario.Gerente || Rol == RolUsuario.Vendedor;
        public bool MostrarCompras      => Rol == RolUsuario.Gerente || Rol == RolUsuario.Administrador;
        public bool MostrarProductos    => Rol == RolUsuario.Gerente;
        public bool MostrarInventario   => Rol == RolUsuario.Gerente || Rol == RolUsuario.Administrador;
        public bool MostrarProveedores  => Rol == RolUsuario.Gerente || Rol == RolUsuario.Administrador;
        public bool MostrarReportes     => Rol == RolUsuario.Gerente || Rol == RolUsuario.Administrador;
        public bool MostrarConfiguracion=> Rol == RolUsuario.Gerente || Rol == RolUsuario.Administrador;

        public int IdEmpresaActual { get; internal set; }
        public int IdSucursalActual { get; internal set; }

        // Caja y Clientes los ven todos

        // ── Configurar sesión desde Login ─────────────────────────────────────
        public void ConfigurarSesion(string nombre, string rol, string sucursal)
        {
            UsuarioSucursal = sucursal;
            Rol = rol?.ToLower() switch
            {
                "gerente" or "dueno" or "owner" => RolUsuario.Gerente,
                "administrador" or "admin"       => RolUsuario.Administrador,
                _                                => RolUsuario.Vendedor,
            };
            UsuarioRol    = Rol switch
            {
                RolUsuario.Gerente       => "Gerente",
                RolUsuario.Administrador => "Administrador",
                _                        => "Vendedor",
            };
            UsuarioNombre = nombre;
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await IrDashboard();

        // ── Navegación ────────────────────────────────────────────────────────
        public async Task IrDashboard()     => await ActivateItemAsync(IoC.Get<DashboardViewModel>(),        CancellationToken.None);
        public async Task IrVentas()        => await ActivateItemAsync(IoC.Get<VentaListadoViewModel>(),     CancellationToken.None);
        public async Task IrCompras()       => await ActivateItemAsync(IoC.Get<CompraListadoViewModel>(),    CancellationToken.None);
        public async Task IrCaja()          => await ActivateItemAsync(IoC.Get<CajaViewModel>(),             CancellationToken.None);
        public async Task IrProductos()     => await ActivateItemAsync(IoC.Get<ProductoListadoViewModel>(),  CancellationToken.None);
        public async Task IrInventario()    => await ActivateItemAsync(IoC.Get<InventarioViewModel>(),       CancellationToken.None);
        public async Task IrClientes()      => await ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(),   CancellationToken.None);
        public async Task IrProveedores()   => await ActivateItemAsync(IoC.Get<ProveedorListadoViewModel>(), CancellationToken.None);
        public async Task IrConfiguracion() => await ActivateItemAsync(IoC.Get<ConfiguracionViewModel>(),    CancellationToken.None);

        // Reportes: redirige al reporte correcto según rol
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
