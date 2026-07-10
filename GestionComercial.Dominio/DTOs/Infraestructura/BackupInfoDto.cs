namespace GestionComercial.Dominio.DTOs.Infraestructura
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
}