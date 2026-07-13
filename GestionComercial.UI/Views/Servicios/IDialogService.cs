using System.Threading.Tasks;

namespace GestionComercial.UI.Views.Servicios
{
    ///     /// Servicio para mostrar diálogos de confirmación, alertas y mensajes desde los ViewModels.
    public interface IDialogService
    {
        /// Muestra un mensaje informativo.
        Task MostrarInfoAsync(string titulo, string mensaje);

        /// Muestra un mensaje de error.
        Task MostrarErrorAsync(string titulo, string mensaje);

        /// Muestra un diálogo de confirmación. Retorna true si el usuario confirmó.
        Task<bool> ConfirmarAsync(string titulo, string mensaje);

        /// Muestra un diálogo de advertencia con opción de continuar o cancelar.
        Task<bool> AdvertirAsync(string titulo, string mensaje);
    }
}
