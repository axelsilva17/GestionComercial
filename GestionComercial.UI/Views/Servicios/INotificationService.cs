namespace GestionComercial.UI.Views.Servicios
{
    ///     /// Servicio para mostrar notificaciones/toasts no bloqueantes en la UI.
    public interface INotificationService
    {
        /// Muestra una notificación de éxito (verde).
        void MostrarExito(string mensaje);

        /// Muestra una notificación de error (roja).
        void MostrarError(string mensaje);

        /// Muestra una notificación de advertencia (amarilla).
        void MostrarAdvertencia(string mensaje);

        /// Muestra una notificación informativa (azul).
        void MostrarInfo(string mensaje);
    }
}
