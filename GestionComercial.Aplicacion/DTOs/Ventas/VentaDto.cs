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
}
