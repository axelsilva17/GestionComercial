namespace GestionComercial.Dominio.DTOs.Infraestructura
{
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
}