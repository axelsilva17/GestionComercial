namespace GestionComercial.Aplicacion.DTOs.Clientes
{
    /// <summary>DTO liviano para listado y sidebar. NO duplica ClienteDto.</summary>
    public class ClienteItemDto
    {
        public int    IdCliente    { get; set; }
        public string Nombre       { get; set; } = string.Empty;
        public int    Documento    { get; set; }
        public string Telefono     { get; set; } = string.Empty;
        public string Email        { get; set; } = string.Empty;
        public bool   Activo       { get; set; }
        public int    TotalVentas  { get; set; }
        public string Inicial      => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
