using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    /// <summary>
    /// Información de un archivo de backup existente.
    /// </summary>
    public class BackupInfo
    {
        public string FileName { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Configuración para backup automático.
    /// </summary>
    public class BackupAutoConfig
    {
        public bool Enabled { get; set; }
        public int CantidadMaximaBackups { get; set; } = 7;
        public string? CarpetaDestino { get; set; }
    }

    /// <summary>
    /// Resultado de una operación de backup.
    /// </summary>
    public class BackupResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? RutaBackup { get; set; }
        public long? TamanoBytes { get; set; }
    }

    /// <summary>
    /// Servicio de backup y restauración de la base de datos.
    /// Implementación en Infraestructura (SQLite).
    /// </summary>
    public interface IBackupService
    {
        /// <summary>
        /// Realiza un backup manual de la base de datos.
        /// </summary>
        /// <param name="nombreOpcional">Nombre opcional para el backup</param>
        /// <returns>Resultado de la operación</returns>
        Task<BackupResult> RealizarBackupAsync(string? nombreOpcional = null);

        /// <summary>
        /// Obtiene la lista de backups existentes.
        /// </summary>
        /// <returns>Lista de backups ordenados por fecha (más reciente primero)</returns>
        Task<IReadOnlyCollection<BackupInfo>> ObtenerBackupsAsync();

        /// <summary>
        /// Elimina un backup existente.
        /// </summary>
        Task<bool> EliminarBackupAsync(string rutaCompleta);

        /// <summary>
        /// Obtiene la configuración actual de backup automático.
        /// </summary>
        Task<BackupAutoConfig> ObtenerConfiguracionAsync();

        /// <summary>
        /// Guarda la configuración de backup automático.
        /// </summary>
        Task GuardarConfiguracionAsync(BackupAutoConfig config);

        /// <summary>
        /// Realiza un backup automático si está habilitado
        /// y aplica la política de retención (elimina los más antiguos).
        /// </summary>
        Task<BackupResult?> BackupAutomaticoSiHabilitadoAsync();
    }
}
