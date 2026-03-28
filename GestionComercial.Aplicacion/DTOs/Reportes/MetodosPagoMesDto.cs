using System;

namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    public class MetodosPagoMesDto
    {
        public string Mes { get; set; } = "";
        public string Metodo { get; set; } = "";
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
    }
}
