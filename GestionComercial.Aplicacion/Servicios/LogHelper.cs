using System;
using System.IO;

namespace GestionComercial.Aplicacion.Servicios
{
    public static class LogHelper
    {
        private static readonly string LogPath;

        static LogHelper()
        {
            // Escribe en el escritorio del usuario
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            LogPath = Path.Combine(desktopPath, "caja-debug.log");
        }

        public static void Log(string mensaje)
        {
            try
            {
                var linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensaje}";
                File.AppendAllText(LogPath, linea + Environment.NewLine);
            }
            catch
            {
                // Ignora errores de logging
            }
        }

        public static void LogError(string mensaje, Exception ex)
        {
            var linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {mensaje}: {ex.InnerException?.Message ?? ex.Message}";
            try
            {
                File.AppendAllText(LogPath, linea + Environment.NewLine);
            }
            catch
            {
                // Ignora errores de logging
            }
        }
    }
}
