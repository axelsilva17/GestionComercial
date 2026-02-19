using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Compras
{
    public class CompraCrearDto
    {
        public int                    IdProveedor { get; set; }
        public int                    IdSucursal  { get; set; }
        public int                    IdUsuario   { get; set; }
        public DateTime               Fecha       { get; set; } = DateTime.Now;
        public string                 Observacion { get; set; }
        public List<CompraDetalleCrearDto> Items  { get; set; } = new();
    }

    public class CompraDetalleCrearDto
    {
        public int     IdProducto  { get; set; }
        public int     Cantidad    { get; set; }
        public decimal PrecioCosto { get; set; }
    }
}
