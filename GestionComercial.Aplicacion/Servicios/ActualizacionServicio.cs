using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ActualizacionServicio : IActualizacionServicio
    {
        public string ObtenerVersionActual()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version?.ToString(3) ?? "1.0.0";
        }

        public string? BuscarActualizacionEnRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta) || !Directory.Exists(ruta))
                return null;

            var archivos = Directory.GetFiles(ruta, "*.dll", SearchOption.AllDirectories);
            foreach (var archivo in archivos)
            {
                if (Path.GetFileName(archivo).StartsWith("GestionComercial", StringComparison.OrdinalIgnoreCase))
                    return archivo;
            }

            var zips = Directory.GetFiles(ruta, "*.zip", SearchOption.TopDirectoryOnly);
            if (zips.Length > 0)
                return zips[0];

            return null;
        }

        public Task<bool> AplicarActualizacionAsync(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                    return Task.FromResult(false);

                var appDir = AppDomain.CurrentDomain.BaseDirectory;
                var backupDir = Path.Combine(appDir, "Updates", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                Directory.CreateDirectory(backupDir);

                var nombreArchivo = Path.GetFileName(rutaArchivo);
                var destino = Path.Combine(appDir, nombreArchivo);

                if (File.Exists(destino))
                {
                    File.Copy(destino, Path.Combine(backupDir, nombreArchivo), true);
                }

                File.Copy(rutaArchivo, destino, true);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public List<string> ObtenerCambios(string rutaArchivo)
        {
            var cambios = new List<string>();
            var directorio = Path.GetDirectoryName(rutaArchivo);
            if (directorio == null) return cambios;

            var changelog = Path.Combine(directorio, "CHANGELOG.txt");
            if (File.Exists(changelog))
            {
                cambios.AddRange(File.ReadAllLines(changelog));
            }
            else
            {
                cambios.Add("No se encontró archivo CHANGELOG.txt en el paquete de actualización.");
            }

            return cambios;
        }
    }
}
