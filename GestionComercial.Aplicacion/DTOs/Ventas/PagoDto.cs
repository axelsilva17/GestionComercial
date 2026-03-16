namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class PagoDto
    {
        public int     IdPago        { get; set; }
        public decimal Monto         { get; set; }
        public int     IdMetodoPago  { get; set; }
        public string  MetodoNombre  { get; set; }
        public bool    EsEfectivo    { get; set; }
        public string  Icono         { get; set; }
    }
    public class MetodoPagoItemDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public bool EsEfectivo { get; set; }
        public bool IsSeleccionado { get; set; }
    }

    public class PagoIngresadoDto
    {
        public int MetodoId { get; set; }
        public string MetodoNombre { get; set; }
        public string Icono { get; set; }
        public decimal Monto { get; set; }
    }
}
