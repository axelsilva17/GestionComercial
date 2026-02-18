using System.Windows;

namespace GestionComercial.UI.Views.Servicios
{
    /// <summary>
    /// Implementación de INotificationService.
    /// Por ahora usa MessageBox. Cuando quieras toasts visuales,
    /// reemplazá el cuerpo de cada método con la librería que elijas
    /// (Ej: Hardcodet.NotifyIcon, ToastNotifications, Snackbar de MahApps, etc.)
    /// sin tocar ningún ViewModels
    /// </summary>
    public class NotificationService : INotificationService
    {
        public void MostrarExito(string mensaje)
        {
            // TODO: reemplazar con toast visual
            MessageBox.Show(mensaje, "✅ Éxito",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MostrarError(string mensaje)
        {
            // TODO: reemplazar con toast visual
            MessageBox.Show(mensaje, "❌ Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void MostrarAdvertencia(string mensaje)
        {
            // TODO: reemplazar con toast visual
            MessageBox.Show(mensaje, "⚠ Advertencia",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void MostrarInfo(string mensaje)
        {
            // TODO: reemplazar con toast visual
            MessageBox.Show(mensaje, "ℹ Información",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
