using System.Collections.Generic;
using System.Threading.Tasks;
using GestionComercial.Dominio.DTOs.Infraestructura;
using GestionComercial.Dominio.Entidades.Configuracion;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IBackupService
    {
        Task<BackupResult> GenerarBackupAsync(string? nombreOpcional = null);

        bool HayEspacioDisponible(long tamanoMinimoBytes);

        void RotarBackups();

        Task<IReadOnlyCollection<BackupInfo>> ObtenerBackupsAsync();

        Task<bool> EliminarBackupAsync(string rutaCompleta);

        Task<BackupConfig> ObtenerConfiguracionAsync();

        Task GuardarConfiguracionAsync(BackupConfig config);

        Task<BackupResult?> BackupAutomaticoSiHabilitadoAsync();
    }
}
