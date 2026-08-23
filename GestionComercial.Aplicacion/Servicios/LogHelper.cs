using System;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Wrapper de debug logging que escribe a Debug output.
    /// Los call sites mantienen la API; el output va al Debug Output de VS.
    /// </summary>
    public static class LogHelper
    {
        public static void Log(string mensaje)
        {
            System.Diagnostics.Debug.WriteLine(mensaje);
        }

        public static void LogError(string mensaje, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"{mensaje}: {ex.Message}");
        }
    }
}
