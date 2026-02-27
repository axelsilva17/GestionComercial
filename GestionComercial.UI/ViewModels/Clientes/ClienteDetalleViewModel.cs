using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Clientes
{
    public class ClienteDetalleViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;

        public ClienteDetalleViewModel(ShellViewModel shell) { _shell = shell; }

        private int _idCliente;

        private ClienteDto _cliente;
        public ClienteDto Cliente
        {
            get => _cliente;
            set { _cliente = value; NotifyOfPropertyChange(() => Cliente); }
        }

        public void Inicializar(int idCliente)
        {
            _idCliente = idCliente;
            _ = CargarAsync();
        }

        private async Task CargarAsync()
        {
            IsLoading = true;
            LimpiarError();
            try
            {
                await Task.Delay(150);
                // TODO: Cliente = await _clienteServicio.ObtenerAsync(_idCliente);
            }
            catch (System.Exception ex) { MostrarError(ex.Message); }
            finally { IsLoading = false; }
        }

        public async Task Editar()
        {
            var vm = IoC.Get<ClienteFormularioViewModel>();
            vm.InicializarParaEditar(_idCliente);
            await _shell.ActivateItemAsync(vm, CancellationToken.None);
        }

        public async Task Volver()
            => await _shell.ActivateItemAsync(IoC.Get<ClienteListadoViewModel>(), CancellationToken.None);
    }
}
