public class ReporteVendedorDto
{
    public string Nombre { get; set; } = string.Empty;
    public int Ventas { get; set; }
    public decimal Total { get; set; }
    public decimal TicketProm { get; set; }
    public int IdUsuario { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string Sucursal { get; set; } = string.Empty;
    public int CantidadVentas { get; set; }
    public decimal TotalVendido { get; set; }
    public decimal PromedioVenta { get; set; }
    public decimal TotalDescuentos { get; set; }
}
