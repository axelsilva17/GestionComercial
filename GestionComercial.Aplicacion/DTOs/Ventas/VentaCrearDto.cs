using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaCrearDto
    {
        public int                    IdSucursal     { get; set; }
        public int                    IdCliente      { get; set; }
        public int                    IdUsuario      { get; set; }
        public int                      IdCaja        { get; set; }
        public decimal                TotalBruto     { get; set; }
        public decimal                TotalDescuento { get; set; }
        public decimal                TotalFinal     { get; set; }
        public DateTime               Fecha          { get; set; } = DateTime.Now;
        public List<VentaDetalleCrearDto> Items      { get; set; } = new();
        public List<PagoCrearDto>         Pagos      { get; set; } = new();
    }

    public class VentaDetalleCrearDto
    {
        public int     IdProducto     { get; set; }
        public int     Cantidad       { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitario  { get; set; }
    }

    public class PagoCrearDto
    {
        public int     IdMetodoPago { get; set; }
        public decimal Monto        { get; set; }
    }
}
