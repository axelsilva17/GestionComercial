namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class PagoDto
    {
        public int IdPago { get; set; }
        public decimal Monto { get; set; }
        public int IdMetodoPago { get; set; }
        public string NombreMetodoPago { get; set; }
        public bool EsEfectivo { get; set; }
    }
}