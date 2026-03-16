namespace GestionComercial.Dominio.Entidades.Movimientos
{
    public class TipoMovimientoStock : EntidadBase
    {
        public string  Nombre      { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
