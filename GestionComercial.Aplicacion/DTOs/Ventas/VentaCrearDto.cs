using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaCrearDto
    {
        public int                    IdSucursal     { get; set; }
        public int                    IdCliente      { get; set; }
        public int                    IdUsuario      { get; set; }
        public int                    IdCaja         { get; set; }
        public decimal                TotalBruto     { get; set; }
        public decimal                TotalDescuento { get; set; }
        public decimal                TotalFinal     { get; set; }
        public DateTime               Fecha          { get; set; } = DateTime.Now;
        public List<VentaDetalleCrearDto> Items       { get; set; } = new();
        public List<PagoCrearDto>         Pagos       { get; set; } = new();
    }

    public class VentaDetalleCrearDto
    {
        public int     IdProducto     { get; set; }
        public int     Cantidad       { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitario  { get; set; }
        /// <summary>
        /// Descuentos aplicados directamente a este ítem (spec modulo-ventas-full).
        /// </summary>
        public List<DescuentoItemDto> Descuentos { get; set; } = new();
    }

    public class PagoCrearDto
    {
        public int     IdMetodoPago { get; set; }
        public decimal Monto        { get; set; }
    }

    /// <summary>
    /// Representa un descuento aplicado a un ítem de venta específico.
    /// </summary>
    public class DescuentoItemDto
    {
        public decimal  Porcentaje   { get; set; }
        public decimal  Monto       { get; set; }
        public string?  Descripcion { get; set; }
    }
}
