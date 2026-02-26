using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Proveedores
{
    /// <summary>
    /// ViewModel de solo lectura — se activa al hacer clic en una fila del listado
    /// cuando se quiere ver el detalle completo en pantalla propia (no sidebar).
    /// </summary>
    public class ProveedorDetalleViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        public ProveedorDetalleViewModel(ShellViewModel shell)
        {
            _shell = shell;
        }

        private int _idProveedor;

        // ── Datos ─────────────────────────────────────────────────────────────
        private ProveedorDto _proveedor;
        public ProveedorDto Proveedor
        {
            get => _proveedor;
            set { _proveedor = value; NotifyOfPropertyChange(() => Proveedor); }
        }

        // ── Inicialización ────────────────────────────────────────────────────
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
                await Task.Delay(150); // TODO: Proveedor = await _proveedorServicio.ObtenerAsync(_idProveedor);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
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
