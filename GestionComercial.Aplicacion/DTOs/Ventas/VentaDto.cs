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
        public int      IdCaja         { get; set; } // Para impresión de tickets
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
        /// <summary>Descuento aplicado directamente al ítem (no al total de la venta).</summary>
        public decimal DescuentoPorItem { get; set; }
        /// <summary>Lista de descuentos aplicados al ítem (nuevo en spec modulo-ventas-full).</summary>
        public List<DescuentoItemDto> Descuentos { get; set; } = new();
    }

    /// <summary>
    /// DTO para representar un descuento aplicado a un ítem de venta.
    /// Agregado según spec modulo-ventas-full.
    /// </summary>
    public class DescuentoItemDto
    {
        public decimal Porcentaje { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
    }

}
