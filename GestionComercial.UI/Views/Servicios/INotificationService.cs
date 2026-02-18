namespace GestionComercial.UI.Views.Servicios
{
    /// <summary>
    /// Servicio para mostrar notificaciones/toasts no bloqueantes en la UI.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>Muestra una notificación de éxito (verde).</summary>
        void MostrarExito(string mensaje);

        /// <summary>Muestra una notificación de error (roja).</summary>
        void MostrarError(string mensaje);

        /// <summary>Muestra una notificación de advertencia (amarilla).</summary>
        void MostrarAdvertencia(string mensaje);

        /// <summary>Muestra una notificación informativa (azul).</summary>
        void MostrarInfo(string mensaje);
    }
}
