using System;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Log de depuración temporal. Se mantiene la API para no romper call sites,
    /// pero ya no escribe en disco (no crea archivos).
    /// </summary>
    public static class LogHelper
    {
        public static void Log(string mensaje)
        {
            // No-op: el logging de depuración ya no es necesario
        }

        public static void LogError(string mensaje, Exception ex)
        {
            // No-op: el logging de depuración ya no es necesario
        }
    }
}
