using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Dominio.Entidades.Configuracion
{
    public class BackupConfig : EntidadBase
    {
        public FrecuenciaBackupEnum Frecuencia { get; set; } = FrecuenciaBackupEnum.Desactivado;
        public DayOfWeek? DiaSemana { get; set; }
        public TimeOnly? HoraProgramada { get; set; }
        public int MaxBackups { get; set; } = 10;
        public string CarpetaDestino { get; set; } = string.Empty;
        public DateTime? UltimoBackup { get; set; }
    }
}
