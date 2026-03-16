using System;

namespace GestionComercial.Aplicacion.DTOs.Reportes
{
    public class ReporteRotacionDto
    {
        public int      IdProducto      { get; set; }
        public string   ProductoNombre  { get; set; }
        public string   Categoria       { get; set; }
        public int      StockActual     { get; set; }
        public int      CantidadVendida { get; set; }
        public int      CantidadComprada { get; set; }
        public decimal  IndiceRotacion  { get; set; }
        public DateTime UltimaVenta     { get; set; }
        public DateTime UltimaCompra    { get; set; }
    }
}
