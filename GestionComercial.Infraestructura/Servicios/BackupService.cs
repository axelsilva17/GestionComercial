using GestionComercial.Dominio.Interfaces.Servicios;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.Infraestructura.Servicios
{
    public class BackupService : IBackupService
    {
        private readonly string _connectionString;
        private readonly string _carpetaBackupsPorDefecto;

        private string? _rutaDbCacheada;

        public BackupService(string connectionString)
        {
            _connectionString = connectionString;

            // Carpeta de backups por defecto: baseDirectory/Backups
            _carpetaBackupsPorDefecto = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Backups");
        }

        private string RutaBaseDeDatos
        {
            get
            {
                if (_rutaDbCacheada == null)
                {
                    // Parsear "Data Source=..." del connection string SQLite
                    var builder = new SqliteConnectionStringBuilder(_connectionString);
                    _rutaDbCacheada = builder.DataSource;

                    // Si es relativa, resolver contra base directory
                    if (!Path.IsPathRooted(_rutaDbCacheada))
                    {
                        _rutaDbCacheada = Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory,
                            _rutaDbCacheada);
                    }
                }

                return _rutaDbCacheada;
            }
        }

        public async Task<BackupResult> RealizarBackupAsync(string? nombreOpcional = null)
        {
            try
            {
                var config = await ObtenerConfiguracionAsync();
                var carpetaDestino = string.IsNullOrEmpty(config.CarpetaDestino)
                    ? _carpetaBackupsPorDefecto
                    : config.CarpetaDestino;

                if (!Directory.Exists(carpetaDestino))
                    Directory.CreateDirectory(carpetaDestino);

                // Nombre del backup: GestionComercial_Backup_11-05-2026_16-28_manual.zip
                // Formato argentino: dd-MM-yyyy
                var timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm");
                var sufijo = string.IsNullOrEmpty(nombreOpcional) ? "" : $"_{nombreOpcional.Replace(" ", "_")}";
                var nombreArchivo = $"GestionComercial_Backup_{timestamp}{sufijo}";
                var rutaTemporalDb = Path.Combine(carpetaDestino, $"{nombreArchivo}.db");
                var rutaComprimida = Path.Combine(carpetaDestino, $"{nombreArchivo}.zip");

                var rutaDb = RutaBaseDeDatos;

                if (!File.Exists(rutaDb))
                {
                    return new BackupResult
                    {
                        Success = false,
                        ErrorMessage = $"No se encontró la base de datos en: {rutaDb}"
                    };
                }

                // ============================================================
                // ESTRATEGIA DE BACKUP MEJORADA PARA SQLITE
                // ============================================================
                // Problema: El DbContext mantiene conexiones abiertas.
                // Solución: 
                // 1. Intentar BackupDatabase API primero (mejor práctica)
                // 2. Si falla por bloqueo, usar WAL checkpoint + File.Copy
                // 3. Agregar reintentos con delay
                // ============================================================

                bool backupExitoso = false;
                Exception? ultimaExcepcion = null;

                // Intentar hasta 3 veces con delay
                for (int intento = 1; intento <= 3; intento++)
                {
                    try
                    {
                        // Método 1: BackupDatabase API (el más seguro)
                        // Usamos Pooling=false para asegurarnos de cerrar conexiones
                        var conexionOrigenStr = new SqliteConnectionStringBuilder(_connectionString)
                        {
                            Mode = SqliteOpenMode.ReadOnly,
                            Pooling = false,
                            Cache = SqliteCacheMode.Shared
                        }.ToString();

                        using (var origen = new SqliteConnection(conexionOrigenStr))
                        using (var destino = new SqliteConnection($"Data Source={rutaTemporalDb};Mode=ReadWriteCreate;Pooling=false;"))
                        {
                            await origen.OpenAsync();
                            await destino.OpenAsync();

                            // Hacer checkpoint antes del backup (para WAL mode)
                            using (var cmdCheckpoint = origen.CreateCommand())
                            {
                                cmdCheckpoint.CommandText = "PRAGMA wal_checkpoint(PASSIVE);";
                                await cmdCheckpoint.ExecuteNonQueryAsync();
                            }

                            // Backup sincrónico (BackupDatabase no tiene versión async)
                            origen.BackupDatabase(destino);

                            // Cerrar explícitamente
                            destino.Close();
                            origen.Close();
                        }

                        // Forzar liberación de conexiones
                        SqliteConnection.ClearAllPools();

                        // Esperar un poco para que el SO libere el archivo
                        Thread.Sleep(100);

                        // Verificar que el archivo existe y se puede acceder
                        if (File.Exists(rutaTemporalDb))
                        {
                            // Intentar abrir el archivo para ver si está libre
                            using (var fs = new FileStream(rutaTemporalDb, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                fs.Close();
                            }
                            backupExitoso = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ultimaExcepcion = ex;

                        // Limpiar si quedo un archivo parcial
                        try { if (File.Exists(rutaTemporalDb)) File.Delete(rutaTemporalDb); } catch { }

                        // Si no es el último intento, esperar y reintentar
                        if (intento < 3)
                        {
                            // Forzar limpieza de pools
                            SqliteConnection.ClearAllPools();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            // Esperar más en cada intento
                            Thread.Sleep(200 * intento);
                        }
                    }
                }

                // Si BackupDatabase falló 3 veces, intentar con File.Copy como último recurso
                if (!backupExitoso)
                {
                    try
                    {
                        // Método 2: File.Copy (solo si la DB no tiene transacciones activas)
                        // Primero intentar checkpoint
                        using (var conexion = new SqliteConnection(new SqliteConnectionStringBuilder(_connectionString)
                        {
                            Pooling = false,
                            Cache = SqliteCacheMode.Shared
                        }.ToString()))
                        {
                            await conexion.OpenAsync();
                            using (var cmd = conexion.CreateCommand())
                            {
                                cmd.CommandText = "PRAGMA wal_checkpoint(TRUNCATE);";
                                await cmd.ExecuteNonQueryAsync();
                            }
                            conexion.Close();
                        }

                        SqliteConnection.ClearAllPools();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Thread.Sleep(300);

                        // Copiar el archivo
                        File.Copy(rutaDb, rutaTemporalDb, overwrite: true);

                        // Verificar
                        if (File.Exists(rutaTemporalDb))
                        {
                            backupExitoso = true;
                        }
                    }
                    catch (Exception exFile)
                    {
                        // Si todo falla, devolver error
                        return new BackupResult
                        {
                            Success = false,
                            ErrorMessage = $"No se pudo realizar el backup después de múltiples intentos.\n" +
                                          $"Último error: {ultimaExcepcion?.Message ?? exFile.Message}"
                        };
                    }
                }

                if (!backupExitoso)
                {
                    return new BackupResult
                    {
                        Success = false,
                        ErrorMessage = "No se pudo crear el archivo de backup."
                    };
                }

                // ============================================================
                // COMPRIMIR el backup a ZIP
                // ============================================================
                // Importante: Asegurarse de que el archivo .db NO esté bloqueado
                // ============================================================

                // Forzar limpieza una vez más
                SqliteConnection.ClearAllPools();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(100);

                // Comprimir usando FileStream explícito para control
                using (var zipStream = new FileStream(rutaComprimida, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    var entry = zip.CreateEntry($"{nombreArchivo}.db", CompressionLevel.Optimal);

                    // Leer el .db y escribirlo al zip
                    using (var entryStream = entry.Open())
                    using (var dbStream = new FileStream(rutaTemporalDb, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await dbStream.CopyToAsync(entryStream);
                    }
                }

                // Ahora deberíamos poder borrar el .db temporal
                try
                {
                    // Esperar un poco más y forzar GC nuevamente
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Thread.Sleep(50);

                    if (File.Exists(rutaTemporalDb))
                    {
                        File.Delete(rutaTemporalDb);
                    }
                }
                catch
                {
                    // Si no podemos borrarlo, no es el fin del mundo - el zip ya está creado
                    // Intentaremos borrarlo en la próxima ejecución o lo ignoramos
                }

                // Aplicar política de retención
                await AplicarRetencionAsync(config, carpetaDestino);

                var archivoFinal = new FileInfo(rutaComprimida);

                return new BackupResult
                {
                    Success = true,
                    RutaBackup = rutaComprimida,
                    TamanoBytes = archivoFinal.Length
                };
            }
            catch (Exception ex)
            {
                return new BackupResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<IReadOnlyCollection<BackupInfo>> ObtenerBackupsAsync()
        {
            var config = await ObtenerConfiguracionAsync();
            var carpeta = string.IsNullOrEmpty(config.CarpetaDestino)
                ? _carpetaBackupsPorDefecto
                : config.CarpetaDestino;

            if (!Directory.Exists(carpeta))
                return Array.Empty<BackupInfo>();

            var archivos = new DirectoryInfo(carpeta)
                .GetFiles("GestionComercial_Backup_*.zip")
                .OrderByDescending(f => f.CreationTime)
                .Select(f => new BackupInfo
                {
                    FileName = f.Name,
                    FullPath = f.FullName,
                    FileSizeBytes = f.Length,
                    CreatedAt = f.CreationTime
                })
                .ToList();

            return archivos;
        }

        public Task<bool> EliminarBackupAsync(string rutaCompleta)
        {
            try
            {
                if (File.Exists(rutaCompleta))
                {
                    File.Delete(rutaCompleta);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<BackupAutoConfig> ObtenerConfiguracionAsync()
        {
            var rutaConfig = RutaConfiguracion();

            if (!File.Exists(rutaConfig))
            {
                return Task.FromResult(new BackupAutoConfig
                {
                    Enabled = false,
                    CantidadMaximaBackups = 7,
                    CarpetaDestino = _carpetaBackupsPorDefecto
                });
            }

            try
            {
                var json = File.ReadAllText(rutaConfig);
                var config = JsonSerializer.Deserialize<BackupAutoConfig>(json);

                if (config == null)
                    throw new InvalidDataException();

                if (string.IsNullOrEmpty(config.CarpetaDestino))
                    config.CarpetaDestino = _carpetaBackupsPorDefecto;

                return Task.FromResult(config);
            }
            catch
            {
                // Si hay error de parsing, devolver defaults
                return Task.FromResult(new BackupAutoConfig
                {
                    Enabled = false,
                    CantidadMaximaBackups = 7,
                    CarpetaDestino = _carpetaBackupsPorDefecto
                });
            }
        }

        public Task GuardarConfiguracionAsync(BackupAutoConfig config)
        {
            var rutaConfig = RutaConfiguracion();
            var carpeta = Path.GetDirectoryName(rutaConfig);

            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(rutaConfig, json);

            return Task.CompletedTask;
        }

        public async Task<BackupResult?> BackupAutomaticoSiHabilitadoAsync()
        {
            var config = await ObtenerConfiguracionAsync();

            if (!config.Enabled)
                return null;

            return await RealizarBackupAsync("automatico");
        }

        // ── Helpers privados ───────────────────────────────────────────────────

        private string RutaConfiguracion()
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "config",
                "backup.json");
        }

        private async Task AplicarRetencionAsync(BackupAutoConfig config, string carpeta)
        {
            var backups = await ObtenerBackupsAsync();
            var lista = backups.ToList();

            if (lista.Count <= config.CantidadMaximaBackups)
                return;

            // Eliminar los más antiguos
            var aEliminar = lista
                .Skip(config.CantidadMaximaBackups)
                .ToList();

            foreach (var b in aEliminar)
            {
                try
                {
                    if (File.Exists(b.FullPath))
                        File.Delete(b.FullPath);
                }
                catch
                {
                    // Ignorar errores de eliminación (mejor dejar que siga)
                }
            }
        }
    }
}
