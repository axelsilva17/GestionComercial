namespace GestionComercial.Dominio.DTOs.Infraestructura
{
    /// <summary>
    /// Configuración para backup automático.
    /// </summary>
    public class BackupAutoConfig
    {
        public bool Enabled { get; set; }
        public int CantidadMaximaBackups { get; set; } = 7;
        public string? CarpetaDestino { get; set; }
    }
}