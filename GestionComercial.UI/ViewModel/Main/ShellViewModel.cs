using Caliburn.Micro;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.UI.ViewModels.Main
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            // Cargar la vista de login al iniciar
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await ActivateItemAsync(new LoginViewModel(this), cancellationToken);
        }
    }
}