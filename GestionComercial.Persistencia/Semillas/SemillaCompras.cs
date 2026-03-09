using GestionComercial.Dominio.Entidades.Compras;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaCompras
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Compra>().HasData(
                new Compra { Id = 1, Fecha = new DateTime(2025, 1, 2),  Total = 145000, Estado = 2, Id_proveedor = 1, Id_sucursal = 1, Id_usuario = 1 },
                new Compra { Id = 2, Fecha = new DateTime(2025, 1, 15), Total = 84000,  Estado = 2, Id_proveedor = 2, Id_sucursal = 1, Id_usuario = 1 },
                new Compra { Id = 3, Fecha = new DateTime(2025, 2, 1),  Total = 108000, Estado = 2, Id_proveedor = 3, Id_sucursal = 1, Id_usuario = 1 },
                new Compra { Id = 4, Fecha = new DateTime(2025, 2, 20), Total = 62000,  Estado = 2, Id_proveedor = 4, Id_sucursal = 1, Id_usuario = 1 },
                new Compra { Id = 5, Fecha = new DateTime(2025, 3, 5),  Total = 174000, Estado = 2, Id_proveedor = 1, Id_sucursal = 1, Id_usuario = 1 },
                new Compra { Id = 6, Fecha = new DateTime(2025, 3, 18), Total = 96000,  Estado = 2, Id_proveedor = 2, Id_sucursal = 1, Id_usuario = 1 }
            );

            builder.Entity<CompraDetalle>().HasData(
                // Compra 1 - Distribuidora Norte - Herramientas
                new CompraDetalle { Id = 1,  Id_compra = 1, Id_producto = 1,  Cantidad = 10, PrecioCosto = 5500,  Subtotal = 55000  },
                new CompraDetalle { Id = 2,  Id_compra = 1, Id_producto = 2,  Cantidad = 15, PrecioCosto = 2600,  Subtotal = 39000  },
                new CompraDetalle { Id = 3,  Id_compra = 1, Id_producto = 3,  Cantidad = 8,  PrecioCosto = 4200,  Subtotal = 33600  },
                new CompraDetalle { Id = 4,  Id_compra = 1, Id_producto = 4,  Cantidad = 4,  PrecioCosto = 8000,  Subtotal = 32000  },
                // Compra 2 - Pinturerías del Sur
                new CompraDetalle { Id = 5,  Id_compra = 2, Id_producto = 7,  Cantidad = 10, PrecioCosto = 12000, Subtotal = 120000 },
                new CompraDetalle { Id = 6,  Id_compra = 2, Id_producto = 9,  Cantidad = 8,  PrecioCosto = 6400,  Subtotal = 51200  },
                // Compra 3 - Electro Mayorista
                new CompraDetalle { Id = 7,  Id_compra = 3, Id_producto = 5,  Cantidad = 1,  PrecioCosto = 58000, Subtotal = 58000  },
                new CompraDetalle { Id = 8,  Id_compra = 3, Id_producto = 11, Cantidad = 10, PrecioCosto = 3500,  Subtotal = 35000  },
                new CompraDetalle { Id = 9,  Id_compra = 3, Id_producto = 12, Cantidad = 10, PrecioCosto = 2000,  Subtotal = 20000  },
                // Compra 4 - El Constructor - Plomería
                new CompraDetalle { Id = 10, Id_compra = 4, Id_producto = 14, Cantidad = 10, PrecioCosto = 3100,  Subtotal = 31000  },
                new CompraDetalle { Id = 11, Id_compra = 4, Id_producto = 15, Cantidad = 20, PrecioCosto = 520,   Subtotal = 10400  },
                new CompraDetalle { Id = 12, Id_compra = 4, Id_producto = 13, Cantidad = 8,  PrecioCosto = 5100,  Subtotal = 40800  },
                // Compra 5 - Distribuidora Norte
                new CompraDetalle { Id = 13, Id_compra = 5, Id_producto = 5,  Cantidad = 1,  PrecioCosto = 58000, Subtotal = 58000  },
                new CompraDetalle { Id = 14, Id_compra = 5, Id_producto = 6,  Cantidad = 1,  PrecioCosto = 50000, Subtotal = 50000  },
                new CompraDetalle { Id = 15, Id_compra = 5, Id_producto = 1,  Cantidad = 12, PrecioCosto = 5500,  Subtotal = 66000  },
                // Compra 6 - Pinturerías del Sur
                new CompraDetalle { Id = 16, Id_compra = 6, Id_producto = 8,  Cantidad = 2,  PrecioCosto = 28000, Subtotal = 56000  },
                new CompraDetalle { Id = 17, Id_compra = 6, Id_producto = 10, Cantidad = 2,  PrecioCosto = 21000, Subtotal = 42000  }
            );
        }
    }
}
