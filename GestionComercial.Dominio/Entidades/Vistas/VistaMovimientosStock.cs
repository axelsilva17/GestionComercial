namespace GestionComercial.Dominio.Entidades.Vistas
{
    /// <summary>
    /// Entidad de solo lectura para la vista VistaMovimientosStock.
    /// Mapea a: CREATE VIEW VistaMovimientosStock AS ...
    /// </summary>
    public class VistaMovimientosStock
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Cantidad { get; set; }
        public string? TipoMovimientoNombre { get; set; }
        public string? ProductoNombre { get; set; }
        public string? Observacion { get; set; }
    }
}