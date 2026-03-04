namespace GestionComercial.Dominio.Entidades.Cliente
{
    public class TipoDocumento : EntidadBase
    {
        public string  Nombre      { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
