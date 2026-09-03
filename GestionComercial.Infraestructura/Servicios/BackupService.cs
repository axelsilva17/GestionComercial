using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Dominio.DTOs.Infraestructura;
using GestionComercial.Dominio.Entidades.Configuracion;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Persistencia.Contexto;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.Infraestructura.Servicios
{
    public class BackupService : IBackupService
    {
        private readonly string _connectionString;
        private readonly GestionComercialContext _context;
        private readonly string _carpetaBackupsPorDefecto;
        private string? _rutaDbCacheada;

        public BackupService(string connectionString, GestionComercialContext context)
        {
            _connectionString = connectionString;
            _context = context;
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
                    var builder = new SqliteConnectionStringBuilder(_connectionString);
                    _rutaDbCacheada = builder.DataSource;

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

        public async Task<BackupResult> GenerarBackupAsync(string? nombreOpcional = null)
        {
            try
            {
                var config = await ObtenerConfiguracionAsync();
                var carpetaDestino = string.IsNullOrEmpty(config.CarpetaDestino)
                    ? _carpetaBackupsPorDefecto
                    : config.CarpetaDestino;

                if (!Directory.Exists(carpetaDestino))
                    Directory.CreateDirectory(carpetaDestino);

                var timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm");
                var sufijo = string.IsNullOrEmpty(nombreOpcional) ? "" : $"_{nombreOpcional.Replace(" ", "_")}";
                var nombreArchivo = $"GestionComercial_Backup_{timestamp}{sufijo}";
                var rutaDestino = Path.Combine(carpetaDestino, $"{nombreArchivo}.db");

                if (!HayEspacioDisponible(10 * 1024 * 1024))
                {
                    return new BackupResult
                    {
                        Success = false,
                        ErrorMessage = "No hay suficiente espacio en disco para realizar el backup."
                    };
                }

                var rutaDb = RutaBaseDeDatos;
                if (!File.Exists(rutaDb))
                {
                    return new BackupResult
                    {
                        Success = false,
                        ErrorMessage = $"No se encontró la base de datos en: {rutaDb}"
                    };
                }

                bool backupExitoso = false;
                Exception? ultimaExcepcion = null;

                for (int intento = 1; intento <= 2; intento++)
                {
                    try
                    {
                        using var conexion = new SqliteConnection(
                            new SqliteConnectionStringBuilder(_connectionString)
                            {
                                Pooling = false,
                                Cache = SqliteCacheMode.Shared
                            }.ToString());

                        await conexion.OpenAsync();

                        using (var cmdCheckpoint = conexion.CreateCommand())
                        {
                            cmdCheckpoint.CommandText = "PRAGMA wal_checkpoint(TRUNCATE);";
                            await cmdCheckpoint.ExecuteNonQueryAsync();
                        }

                        using (var cmdVacuum = conexion.CreateCommand())
                        {
                            cmdVacuum.CommandText = $"VACUUM INTO '{rutaDestino.Replace("'", "''")}';";
                            await cmdVacuum.ExecuteNonQueryAsync();
                        }

                        conexion.Close();
                        SqliteConnection.ClearAllPools();

                        Thread.Sleep(100);

                        if (File.Exists(rutaDestino))
                        {
                            backupExitoso = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ultimaExcepcion = ex;
                        try { if (File.Exists(rutaDestino)) File.Delete(rutaDestino); } catch { }

                        if (intento < 2)
                        {
                            SqliteConnection.ClearAllPools();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            Thread.Sleep(500);
                        }
                    }
                }

                if (!backupExitoso)
                {
                    return new BackupResult
                    {
                        Success = false,
                        ErrorMessage = $"No se pudo realizar el backup después de reintentos.\n" +
                                      $"Último error: {ultimaExcepcion?.Message}"
                    };
                }

                var configActual = await ObtenerConfiguracionAsync();
                configActual.UltimoBackup = DateTime.Now;
                await GuardarConfiguracionAsync(configActual);

                RotarBackups();

                var archivoFinal = new FileInfo(rutaDestino);
                return new BackupResult
                {
                    Success = true,
                    RutaBackup = rutaDestino,
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

        public bool HayEspacioDisponible(long tamanoMinimoBytes)
        {
            try
            {
                var config = ObtenerConfiguracionAsync().GetAwaiter().GetResult();
                var carpeta = string.IsNullOrEmpty(config.CarpetaDestino)
                    ? _carpetaBackupsPorDefecto
                    : config.CarpetaDestino;

                var driveInfo = new DriveInfo(Path.GetPathRoot(Path.GetFullPath(carpeta)));
                return driveInfo.AvailableFreeSpace >= tamanoMinimoBytes;
            }
            catch
            {
                return false;
            }
        }

        public void RotarBackups()
        {
            try
            {
                var config = ObtenerConfiguracionAsync().GetAwaiter().GetResult();
                var carpeta = string.IsNullOrEmpty(config.CarpetaDestino)
                    ? _carpetaBackupsPorDefecto
                    : config.CarpetaDestino;

                if (!Directory.Exists(carpeta)) return;

                var archivos = new DirectoryInfo(carpeta)
                    .GetFiles("GestionComercial_Backup_*.db")
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();

                if (archivos.Count <= config.MaxBackups) return;

                var aEliminar = archivos.Skip(config.MaxBackups).ToList();
                foreach (var archivo in aEliminar)
                {
                    try { archivo.Delete(); } catch { }
                }
            }
            catch { }
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
                .GetFiles("GestionComercial_Backup_*.db")
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

        public async Task<BackupConfig> ObtenerConfiguracionAsync()
        {
            var config = await _context.BackupConfigs
                .OrderBy(c => c.Id)
                .FirstOrDefaultAsync();
            if (config == null)
            {
                config = new BackupConfig
                {
                    Frecuencia = FrecuenciaBackupEnum.Desactivado,
                    MaxBackups = 10,
                    CarpetaDestino = _carpetaBackupsPorDefecto
                };
                _context.BackupConfigs.Add(config);
                await _context.SaveChangesAsync();
            }
            return config;
        }

        public async Task GuardarConfiguracionAsync(BackupConfig config)
        {
            var existing = await _context.BackupConfigs
                .OrderBy(c => c.Id)
                .FirstOrDefaultAsync();
            if (existing != null)
            {
                existing.Frecuencia = config.Frecuencia;
                existing.DiaSemana = config.DiaSemana;
                existing.HoraProgramada = config.HoraProgramada;
                existing.MaxBackups = config.MaxBackups;
                existing.CarpetaDestino = config.CarpetaDestino;
                existing.UltimoBackup = config.UltimoBackup;
            }
            else
            {
                _context.BackupConfigs.Add(config);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<BackupResult?> BackupAutomaticoSiHabilitadoAsync()
        {
            var config = await ObtenerConfiguracionAsync();

            if (config.Frecuencia == FrecuenciaBackupEnum.Desactivado)
                return null;

            if (!DebeEjecutarBackup(config))
                return null;

            var result = await GenerarBackupAsync("automatico");
            return result;
        }

        private bool DebeEjecutarBackup(BackupConfig config)
        {
            var hoy = DateTime.Today;

            return config.Frecuencia switch
            {
                FrecuenciaBackupEnum.AlAbrirApp =>
                    config.UltimoBackup == null || config.UltimoBackup.Value.Date != hoy,

                FrecuenciaBackupEnum.Diario =>
                    config.UltimoBackup == null || config.UltimoBackup.Value.Date < hoy,

                FrecuenciaBackupEnum.Semanal =>
                    config.DiaSemana.HasValue &&
                    hoy.DayOfWeek == config.DiaSemana.Value &&
                    (config.UltimoBackup == null || config.UltimoBackup.Value.Date.AddDays(7) <= hoy),

                _ => false
            };
        }
    }
}
