using System;

namespace GestionComercial.Aplicacion.DTOs.Caja
{
    /// <summary>
    /// Representa un movimiento individual de caja (ingreso o egreso manual, o venta).
    /// </summary>
    public class MovimientoCajaDto
    {
        public int IdMovimiento { get; set; }
        public string Tipo { get; set; }  // "Ingreso" | "Egreso" | "Venta"
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public int? ReferenciaId { get; set; }  // IdVenta si aplica

        // Para mostrar en UI
        public bool EsIngreso => Tipo == "Ingreso" || Tipo == "Venta";
        public string TipoIcono => Tipo switch
        {
            "Venta" => "◈",
            "Ingreso" => "↑",
            "Egreso" => "↓",
            _ => "·"
        };
    }
 
}