using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Caja;
using GestionComercial.UI.ViewModels.Clientes;
using GestionComercial.UI.ViewModels.Compras;
using GestionComercial.UI.ViewModels.Configuracion;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.ViewModels.Productos;
using GestionComercial.UI.ViewModels.Proveedores;
using GestionComercial.UI.ViewModels.Reportes;
using GestionComercial.UI.ViewModels.Ventas;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.Views.Servicios
{
    /// <summary>
    /// Implementación de INavigationService.
    /// Delega la navegación al ShellViewModel (Conductor) de Caliburn.
    /// Los ViewModels inyectan esta interfaz en lugar de depender del Shell directamente.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly ShellViewModel _shell;

        public NavigationService(ShellViewModel shell)
        {
            _shell = shell;
        }

        public async Task IrDashboardAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<DashboardViewModel>(), CancellationToken.None);

        public async Task IrVentasAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);

        public async Task IrComprasAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<CompraListadoViewModel>(), CancellationToken.None);

        public async Task IrCajaAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<CajaViewModel>(), CancellationToken.None);

        public async Task IrProductosAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<ProductoListadoViewModel>(), CancellationToken.None);

        public async Task IrClientesAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(), CancellationToken.None);

        public async Task IrProveedoresAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<ProveedorListadoViewModel>(), CancellationToken.None);

        public async Task IrReportesAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<ReportesViewModel>(), CancellationToken.None);

        public async Task IrConfiguracionAsync() =>
            await _shell.ActivateItemAsync(IoC.Get<ConfiguracionViewModel>(), CancellationToken.None);
    }
}