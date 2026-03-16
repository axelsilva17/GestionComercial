using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaDto
    {
        public int      IdVenta        { get; set; }
        public DateTime Fecha          { get; set; }
        public decimal  TotalBruto     { get; set; }
        public decimal  TotalDescuento { get; set; }
        public decimal  TotalFinal     { get; set; }
        public string   Estado         { get; set; }
        public int      IdSucursal     { get; set; }
        public string   SucursalNombre { get; set; }
        public int      IdCliente      { get; set; }
        public string   ClienteNombre  { get; set; }
        public int      IdUsuario      { get; set; }
        public string   UsuarioNombre  { get; set; }
        public List<VentaDetalleDto> Items { get; set; } = new();
        public List<PagoDto>         Pagos { get; set; } = new();
    }
    public class VentaItemDto
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public string CodigoBarra { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

}
