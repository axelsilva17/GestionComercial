using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Clientes
{
    public class ClienteDetalleViewModel : NavigableViewModel
    {
        private readonly ShellViewModel _shell;
        private readonly IClienteServicio _clienteServicio;

        public ClienteDetalleViewModel(ShellViewModel shell, IClienteServicio clienteServicio)
        {
            _shell = shell;
            _clienteServicio = clienteServicio;
        }

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
                Cliente = await _clienteServicio.ObtenerPorIdAsync(_idCliente);
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
