using System;

namespace GestionComercial.Dominio.DTOs.Infraestructura
{
    [Obsolete("Usar BackupConfig entity en GestionComercial.Dominio.Entidades.Configuracion en su lugar.")]
    public class BackupAutoConfig
    {
        public bool Enabled { get; set; }
        public int CantidadMaximaBackups { get; set; } = 7;
        public string? CarpetaDestino { get; set; }
    }
}
