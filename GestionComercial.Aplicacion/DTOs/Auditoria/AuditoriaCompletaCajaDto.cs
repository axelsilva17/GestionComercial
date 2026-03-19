namespace GestionComercial.Aplicacion.DTOs.Auditoria
{
    /// <summary>
    /// DTO para un registro individual de auditoría de caja.
    /// </summary>
    public class AuditoriaLogDto
    {
        public int         Id            { get; set; }
        public DateTime    Fecha         { get; set; }
        public string      Usuario       { get; set; } = string.Empty;
        public string      TipoOperacion { get; set; } = string.Empty;
        public string      Tabla         { get; set; } = string.Empty;
        public int         RegistroId    { get; set; }
        public string?     Detalles      { get; set; }
        public string      FechaFormateada => Fecha.ToString("dd/MM HH:mm");
    }

    /// <summary>
    /// DTO que contiene tanto la auditoría de cajas como los movimientos de caja.
    /// </summary>
    public class AuditoriaCompletaCajaDto
    {
        public List<AuditoriaLogDto> AuditoriaCajas    { get; set; } = new();
        public List<AuditoriaLogDto> MovimientosCaja { get; set; } = new();
    }
}
