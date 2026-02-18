using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Caja;
using GestionComercial.UI.ViewModels.Clientes;
using GestionComercial.UI.ViewModels.Compras;
using GestionComercial.UI.ViewModels.Configuracion;
using GestionComercial.UI.ViewModels.Productos;
using GestionComercial.UI.ViewModels.Proveedores;
using GestionComercial.UI.ViewModels.Reportes;
using GestionComercial.UI.ViewModels.Ventas;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GestionComercial.UI.ViewModels.Main
{
    public class ShellViewModel : Conductor<object>
    {
        private string _usuarioNombre = "Administrador";
        private string _usuarioRol    = "Admin";
        private string _usuarioSucursal = "Casa Central";

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

        // Inicial para el avatar del sidebar
        public string UsuarioInicial =>
            string.IsNullOrEmpty(UsuarioNombre) ? "?" : UsuarioNombre[0].ToString().ToUpper();

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await IrDashboard();
        }
  

        public async Task IrDashboard()    => await ActivateItemAsync(IoC.Get<DashboardViewModel>(),             CancellationToken.None);
        public async Task IrVentas()       => await ActivateItemAsync(IoC.Get<VentaListadoViewModel>(),          CancellationToken.None);

        
        public async Task IrCompras()      => await ActivateItemAsync(IoC.Get<CompraListadoViewModel>(),         CancellationToken.None);
    
        public async Task IrCaja()         => await ActivateItemAsync(IoC.Get<CajaViewModel>(),                  CancellationToken.None);
        public async Task IrProductos()    => await ActivateItemAsync(IoC.Get<ProductoListadoViewModel>(),       CancellationToken.None);
        public async Task IrInventario()   => await ActivateItemAsync(IoC.Get<MovimientoCajaViewModel>(),        CancellationToken.None);
        public async Task IrClientes()     => await ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(),        CancellationToken.None);
        public async Task IrProveedores()  => await ActivateItemAsync(IoC.Get<ProveedorListadoViewModel>(),      CancellationToken.None);
        public async Task IrReportes()     => await ActivateItemAsync(IoC.Get<ReportesViewModel>(),              CancellationToken.None);
        public async Task IrConfiguracion()=> await ActivateItemAsync(IoC.Get<ConfiguracionViewModel>(),         CancellationToken.None);

        public void CerrarSesion()
        {
            var login = IoC.Get<LoginViewModel>();
            var wm    = IoC.Get<IWindowManager>();
            wm.ShowWindowAsync(login);
            TryCloseAsync();
        }
    }
}
