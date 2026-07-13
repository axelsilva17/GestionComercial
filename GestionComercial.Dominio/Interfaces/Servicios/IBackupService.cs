using System.Collections.Generic;
using System.Threading.Tasks;
using GestionComercial.Dominio.DTOs.Infraestructura;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
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