using System;

namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    /// <summary>
    /// Fila mensual de métodos de pago para exportación Excel.
    /// Mes = primer día del mes (se formatea con "MMM yy" en el ViewModel).
    /// </summary>
    public class MetodosPagoMesExportDto
    {
        public DateTime Mes { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
    }
}