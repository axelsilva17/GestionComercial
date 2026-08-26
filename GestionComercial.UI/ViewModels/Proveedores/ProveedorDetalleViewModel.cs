using Caliburn.Micro;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    public class ProveedorDetalleViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;
        private readonly IProveedorServicio _proveedorServicio;

        public ProveedorDetalleViewModel(ShellViewModel shell, IProveedorServicio proveedorServicio)
        {
            _shell = shell;
            _proveedorServicio = proveedorServicio;
        }

        private int _idProveedor;

        private Proveedor _proveedor;
        public Proveedor Proveedor
        {
            get => _proveedor;
            set { _proveedor = value; NotifyOfPropertyChange(() => Proveedor); }
        }

        public void Inicializar(int idProveedor)
        {
            _idProveedor = idProveedor;
            _ = CargarAsync();
        }

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                Proveedor = await _proveedorServicio.ObtenerPorIdAsync(_idProveedor);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Editar()
        {
            var vm = IoC.Get<ProveedorFormularioViewModel>();
            vm.InicializarParaEditar(_idProveedor);
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task Volver()
        {
            await _shell.ActivateItemAsync(IoC.Get<ProveedorListadoViewModel>(), CancellationToken.None);
        }
    }
}
