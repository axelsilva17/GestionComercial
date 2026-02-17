using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaDto
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TotalBruto { get; set; }
        public decimal TotalDescuento { get; set; }
        public decimal TotalFinal { get; set; }
        public string Estado { get; set; }
        public int IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }

        public List<VentaDetalleDto> Detalles { get; set; }
        public List<PagoDto> Pagos { get; set; }

        public VentaDto()
        {
            Detalles = new List<VentaDetalleDto>();
            Pagos = new List<PagoDto>();
        }
    }
}