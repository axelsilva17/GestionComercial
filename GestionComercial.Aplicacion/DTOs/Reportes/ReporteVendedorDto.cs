namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    public class ReporteVendedorDto
    {
        public int     IdUsuario      { get; set; }
        public string  UsuarioNombre  { get; set; }
        public string  Sucursal       { get; set; }
        public int     CantidadVentas { get; set; }
        public decimal TotalVendido   { get; set; }
        public decimal PromedioVenta  { get; set; }
        public decimal TotalDescuentos { get; set; }
        public string  Inicial => string.IsNullOrEmpty(UsuarioNombre) ? "?" : UsuarioNombre[0].ToString().ToUpper();
    }
}
