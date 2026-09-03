using GestionComercial.Dominio.Interfaces.Servicios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.Aplicacion.Servicios
{
    public class DiagnosticoServicio : IDiagnosticoServicio
    {
        private readonly IDiagnosticoContext _diagnosticoContext;

        private const long UmbralTamanoDbBytes = 500L * 1024 * 1024;
        private const string RetencionLogsDias = "-90 days";

        public DiagnosticoServicio(IDiagnosticoContext diagnosticoContext)
        {
            _diagnosticoContext = diagnosticoContext;
        }

        public async Task<DiagnosticoResultDto> EjecutarAuditoriaCompletaAsync(int idEmpresa)
        {
            var stats = await ObtenerEstadisticasAsync();
            var integridad = await VerificarIntegridadAsync();

            var result = new DiagnosticoResultDto
            {
                IntegridadOk = integridad.IntegridadOk,
                MensajeIntegridad = integridad.MensajeIntegridad,
                Stats = stats,
                Warnings = integridad.Warnings
            };

            if (stats != null && stats.TamanoDbBytes > UmbralTamanoDbBytes)
            {
                result.Warnings.Add(new LogItemDto
                {
                    Tipo = "Advertencia",
                    Mensaje = $"La base de datos supera los 500 MB ({stats.TamanoFormateado}). Considerá optimizar.",
                    Fecha = DateTime.UtcNow
                });
            }

            return result;
        }

        public async Task<DiagnosticoResultDto> VerificarIntegridadAsync()
        {
            var result = new DiagnosticoResultDto();
            try
            {
                var problemas = new List<string>();
                using var reader = await _diagnosticoContext.EjecutarReaderAsync("PRAGMA quick_check");
                while (await reader.ReadAsync())
                {
                    var resultado = reader.GetString(0)?.Trim();
                    if (!resultado.Equals("ok", StringComparison.OrdinalIgnoreCase))
                        problemas.Add(resultado);
                }
                result.IntegridadOk = problemas.Count == 0;
                result.MensajeIntegridad = result.IntegridadOk
                    ? "Integridad verificada correctamente."
                    : $"Error de integridad: {string.Join("; ", problemas)}";
                result.DetalleIntegridad = result.IntegridadOk ? "OK" : string.Join("; ", problemas);

                if (!result.IntegridadOk)
                {
                    result.Warnings.Add(new LogItemDto
                    {
                        Tipo = "Error",
                        Mensaje = result.MensajeIntegridad,
                        Fecha = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                result.IntegridadOk = false;
                result.MensajeIntegridad = $"Error al verificar integridad: {ex.Message}";
                result.Warnings.Add(new LogItemDto
                {
                    Tipo = "Error",
                    Mensaje = result.MensajeIntegridad,
                    Fecha = DateTime.UtcNow
                });
            }

            return result;
        }

        private async Task<StatsDbDto> ObtenerEstadisticasAsync()
        {
            var sql = @"SELECT
  (SELECT COUNT(*) FROM Venta) AS Ventas,
  (SELECT COUNT(*) FROM Producto) AS Productos,
  (SELECT COUNT(*) FROM Cliente) AS Clientes,
  (SELECT COUNT(*) FROM Compra) AS Compras,
  (SELECT COUNT(*) FROM Proveedor) AS Proveedores,
  (SELECT COUNT(*) FROM MetodoPago) AS MetodosPago,
  (SELECT COUNT(*) FROM Categoria) AS Categorias";
            using var reader = await _diagnosticoContext.EjecutarReaderAsync(sql);
            var stats = new StatsDbDto();
            if (await reader.ReadAsync())
            {
                stats.TotalVentas = reader.GetInt32(0);
                stats.TotalProductos = reader.GetInt32(1);
                stats.TotalClientes = reader.GetInt32(2);
                stats.TotalCompras = reader.GetInt32(3);
                stats.TotalProveedores = reader.GetInt32(4);
                stats.TotalMetodosPago = reader.GetInt32(5);
                stats.TotalCategorias = reader.GetInt32(6);
            }

            try
            {
                stats.TamanoDbBytes = await _diagnosticoContext.ObtenerTamanoDbAsync();
                stats.TamanoFormateado = FormatearTamano(stats.TamanoDbBytes);
            }
            catch
            {
                stats.TamanoFormateado = "No disponible";
            }

            return stats;
        }

        public async Task OptimizarAsync()
        {
            await _diagnosticoContext.EjecutarRawSqlAsync("PRAGMA optimize;");
        }

        public async Task ReindexarAsync()
        {
            await _diagnosticoContext.EjecutarRawSqlAsync("REINDEX;");
        }

        public async Task LimpiarAsync()
        {
            await _diagnosticoContext.EjecutarRawSqlAsync(
                $"DELETE FROM MantenimientoLog WHERE FechaAlta < datetime('now', '{RetencionLogsDias}');");
        }

        private static string FormatearTamano(long bytes)
        {
            string[] sufijos = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double tam = bytes;
            while (tam >= 1024 && i < sufijos.Length - 1)
            {
                tam /= 1024;
                i++;
            }
            return $"{tam:F1} {sufijos[i]}";
        }
    }
}
