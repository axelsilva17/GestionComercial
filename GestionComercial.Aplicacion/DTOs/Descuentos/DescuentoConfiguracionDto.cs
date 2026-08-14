namespace GestionComercial.Aplicacion.DTOs.Descuentos
{
    public class DescuentoConfiguracionDto
    {
        public string Nombre { get; set; } = string.Empty;
        public Dominio.Entidades.Descuento.TipoDescuentoEnum Tipo { get; set; }
        public decimal Valor { get; set; }
        public int? Id_producto { get; set; }
        public int? Id_categoria { get; set; }
        public int Id_empresa { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int Prioridad { get; set; }
    }

    public class DescuentoListadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public Dominio.Entidades.Descuento.TipoDescuentoEnum Tipo { get; set; }
        public decimal Valor { get; set; }
        public int? Id_producto { get; set; }
        public string? ProductoNombre { get; set; }
        public int? Id_categoria { get; set; }
        public string? CategoriaNombre { get; set; }
        public int? Id_metodoPago { get; set; }
        public string? MetodoPagoNombre { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int Prioridad { get; set; }
        public bool Activo { get; set; }
    }
}
