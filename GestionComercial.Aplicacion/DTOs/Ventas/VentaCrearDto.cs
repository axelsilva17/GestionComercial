using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaCrearDto
    {
        public DateTime Fecha { get; set; }
        public int IdSucursal { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public decimal TotalDescuento { get; set; }

        public List<VentaDetalleCrearDto> Detalles { get; set; }
        public List<PagoCrearDto> Pagos { get; set; }

        public VentaCrearDto()
        {
            Fecha = DateTime.Now;
            Detalles = new List<VentaDetalleCrearDto>();
            Pagos = new List<PagoCrearDto>();
        }
    }

    public class VentaDetalleCrearDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal Descuento { get; set; }
    }

    public class PagoCrearDto
    {
        public int IdMetodoPago { get; set; }
        public decimal Monto { get; set; }
    }
}