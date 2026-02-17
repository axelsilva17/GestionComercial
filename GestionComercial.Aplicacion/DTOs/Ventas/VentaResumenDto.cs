using System;

namespace GestionComercial.Aplicacion.DTOs.Ventas
{
    public class VentaResumenDto
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public decimal TotalFinal { get; set; }
        public string Estado { get; set; }
        public string NombreSucursal { get; set; }
        public string NombreUsuario { get; set; }
        public int CantidadItems { get; set; }
        public string MetodosPago { get; set; }
    }
}