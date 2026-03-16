using System.Threading.Tasks;

namespace GestionComercial.UI.Views.Servicios
{
    /// <summary>
    /// Servicio para mostrar diálogos de confirmación, alertas y mensajes desde los ViewModels.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>Muestra un mensaje informativo.</summary>
        Task MostrarInfoAsync(string titulo, string mensaje);

        /// <summary>Muestra un mensaje de error.</summary>
        Task MostrarErrorAsync(string titulo, string mensaje);

        /// <summary>Muestra un diálogo de confirmación. Retorna true si el usuario confirmó.</summary>
        Task<bool> ConfirmarAsync(string titulo, string mensaje);

        /// <summary>Muestra un diálogo de advertencia con opción de continuar o cancelar.</summary>
        Task<bool> AdvertirAsync(string titulo, string mensaje);
    }
}
