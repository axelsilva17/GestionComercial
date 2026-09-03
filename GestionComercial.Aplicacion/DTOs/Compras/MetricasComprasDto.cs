namespace GestionComercial.Aplicacion.DTOs.Compras
{
    /// <summary>
    /// Métricas agregadas de compras para un período y sucursal.
    /// </summary>
    public class MetricasComprasDto
    {
        public decimal Total { get; set; }
        public int Count { get; set; }
        public decimal Promedio { get; set; }
        public string ProveedorTop { get; set; } = string.Empty;
        public int ProductosRepuestos { get; set; }
    }
}