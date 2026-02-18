using Caliburn.Micro;
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

namespace GestionComercial.UI.ViewModels.Main
{
    public class ShellViewModel : Conductor<object>
    {
        public string UsuarioNombre { get; set; } = "Administrador";
        public string UsuarioRol { get; set; } = "Admin";
        public string UsuarioSucursal { get; set; } = "Casa Central";

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await IrDashboard();
        }

        public async Task IrDashboard()
        {
            await ActivateItemAsync(IoC.Get<DashboardViewModel>(), CancellationToken.None);
        }

        public async Task IrVentas()
        {
            await ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);
        }

        public async Task IrCompras()
        {
            await ActivateItemAsync(IoC.Get<CompraListadoViewModel>(), CancellationToken.None);
        }

        public async Task IrCaja()
        {
            await ActivateItemAsync(IoC.Get<CajaViewModel>(), CancellationToken.None);
        }

        public async Task IrProductos()
        {
            await ActivateItemAsync(IoC.Get<ProductoListadoViewModel>(), CancellationToken.None);
        }

        public async Task IrInventario()
        {
            await ActivateItemAsync(IoC.Get<ProductoListadoViewModel>(), CancellationToken.None);
        }

        public async Task IrClientes()
        {
            await ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(), CancellationToken.None);
        }

        public async Task IrProveedores()
        {
            await ActivateItemAsync(IoC.Get<ProveedorListadoViewModel>(), CancellationToken.None);
        }

        public async Task IrReportes()
        {
            await ActivateItemAsync(IoC.Get<ReportesViewModel>(), CancellationToken.None);
        }

        public async Task IrConfiguracion()
        {
            await ActivateItemAsync(IoC.Get<ConfiguracionViewModel>(), CancellationToken.None);
        }

        public async Task CerrarSesion()
        {
            var windowManager = IoC.Get<IWindowManager>();
            var loginVM = IoC.Get<LoginViewModel>();
            await windowManager.ShowWindowAsync(loginVM);
            await TryCloseAsync();
        }
    }
}
