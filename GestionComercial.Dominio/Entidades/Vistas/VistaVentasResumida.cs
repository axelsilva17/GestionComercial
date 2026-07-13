namespace GestionComercial.Dominio.Entidades.Vistas
{
    ///     /// Entidad de solo lectura para la vista VistaVentasResumidas.
    /// Mapea a: CREATE VIEW VistaVentasResumidas AS ...
    public class VistaVentasResumida
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int Estado { get; set; }
        public string? ClienteNombre { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? SucursalNombre { get; set; }
        public string? MetodoPagoNombre { get; set; }
    }
}