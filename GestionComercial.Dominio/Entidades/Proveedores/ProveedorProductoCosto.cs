using System;
using GestionComercial.Dominio.Entidades;
using GestionComercial.Dominio.Entidades.Producto;

namespace GestionComercial.Dominio.Entidades.Proveedores
{
    public class ProveedorProductoCosto : EntidadBase
    {
        public int IdProveedor { get; set; }
        public int IdProducto { get; set; }
        public decimal Costo { get; set; }

        public Proveedor Proveedor { get; set; } = null!;
        public ProveedorProductoCosto Producto { get; set; } = null!;

        // Factory para crear con validaciones simples
        public static ProveedorProductoCosto Crear(int idProveedor, int idProducto, decimal costo)
        {
            if (idProveedor <= 0) throw new ArgumentException("ID de proveedor inválido.", nameof(idProveedor));
            if (idProducto <= 0) throw new ArgumentException("ID de producto inválido.", nameof(idProducto));
            var costoVal = costo < 0 ? 0 : costo;
            return new ProveedorProductoCosto
            {
                IdProveedor = idProveedor,
                IdProducto = idProducto,
                Costo = costoVal,
                FechaAlta = DateTime.Now,
                Activo = true
            };
        }
    }
}
