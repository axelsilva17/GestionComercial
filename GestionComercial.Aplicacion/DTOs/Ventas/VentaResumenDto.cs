using System;

namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaResumenDto
    {
        public int      IdVenta        { get; set; }
        public DateTime Fecha          { get; set; }
        public decimal  TotalBruto     { get; set; }
        public decimal  TotalDescuento { get; set; }
        public decimal  TotalFinal     { get; set; }
        public string   Estado         { get; set; }
        public string   ClienteNombre  { get; set; }
        public string   UsuarioNombre  { get; set; }
        public string   ClienteInicial => string.IsNullOrEmpty(ClienteNombre) ? "?" : ClienteNombre[0].ToString().ToUpper();
    }
}
