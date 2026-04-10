using System;
using System.Collections.Generic;

namespace GestionComercial.Aplicacion.DTOs.Compras
{
    public class CompraDto
    {
        public int              IdCompra          { get; set; }
        public DateTime         Fecha             { get; set; }
        public decimal          Total             { get; set; }
        public int              Estado            { get; set; } // 1=Pendiente, 2=Recibida, 3=Pagada
        public string           EstadoTexto       => Estado switch { 1 => "Pendiente", 2 => "Recibida", 3 => "Pagada", _ => "—" };
        public int              Id_proveedor      { get; set; }
        public string           ProveedorNombre   { get; set; }
        public string           ProveedorTelefono { get; set; }
        public int              IdSucursal        { get; set; }
        public string           SucursalNombre    { get; set; }
        public int              IdUsuario         { get; set; }
        public string           UsuarioNombre     { get; set; }
        public string           Observacion       { get; set; }
        public List<CompraDetalleDto> Items       { get; set; } = new();
        public string ProveedorInicial => string.IsNullOrEmpty(ProveedorNombre) ? "?" : ProveedorNombre[0].ToString().ToUpper();
        public int    CantidadProductos => Items?.Count ?? 0;
    }
}
