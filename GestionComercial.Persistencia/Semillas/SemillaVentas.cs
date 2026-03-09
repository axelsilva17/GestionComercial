using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Ventas;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaVentas
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Venta>().HasData(
                // Enero
                new Venta { Id = 1,  Fecha = new DateTime(2025, 1, 5),  TotalBruto = 25700,  TotalDescuento = 0,    TotalFinal = 25700,  Estado = 2, Id_sucursal = 1, Id_cliente = 2, Id_usuario = 1, Id_caja = 1 },
                new Venta { Id = 2,  Fecha = new DateTime(2025, 1, 8),  TotalBruto = 85000,  TotalDescuento = 0,    TotalFinal = 85000,  Estado = 2, Id_sucursal = 1, Id_cliente = 1, Id_usuario = 1, Id_caja = 1 },
                new Venta { Id = 3,  Fecha = new DateTime(2025, 1, 12), TotalBruto = 18500,  TotalDescuento = 925,  TotalFinal = 17575,  Estado = 2, Id_sucursal = 1, Id_cliente = 3, Id_usuario = 1, Id_caja = 1 },
                new Venta { Id = 4,  Fecha = new DateTime(2025, 1, 20), TotalBruto = 54800,  TotalDescuento = 0,    TotalFinal = 54800,  Estado = 2, Id_sucursal = 1, Id_cliente = 6, Id_usuario = 1, Id_caja = 1 },
                new Venta { Id = 5,  Fecha = new DateTime(2025, 1, 25), TotalBruto = 32000,  TotalDescuento = 1600, TotalFinal = 30400,  Estado = 2, Id_sucursal = 1, Id_cliente = 4, Id_usuario = 1, Id_caja = 1 },
                // Febrero
                new Venta { Id = 6,  Fecha = new DateTime(2025, 2, 3),  TotalBruto = 42000,  TotalDescuento = 0,    TotalFinal = 42000,  Estado = 2, Id_sucursal = 1, Id_cliente = 1, Id_usuario = 1, Id_caja = 2 },
                new Venta { Id = 7,  Fecha = new DateTime(2025, 2, 7),  TotalBruto = 12500,  TotalDescuento = 0,    TotalFinal = 12500,  Estado = 2, Id_sucursal = 1, Id_cliente = 2, Id_usuario = 1, Id_caja = 2 },
                new Venta { Id = 8,  Fecha = new DateTime(2025, 2, 14), TotalBruto = 93600,  TotalDescuento = 4680, TotalFinal = 88920,  Estado = 2, Id_sucursal = 1, Id_cliente = 6, Id_usuario = 1, Id_caja = 2 },
                new Venta { Id = 9,  Fecha = new DateTime(2025, 2, 18), TotalBruto = 27300,  TotalDescuento = 0,    TotalFinal = 27300,  Estado = 2, Id_sucursal = 1, Id_cliente = 5, Id_usuario = 1, Id_caja = 2 },
                new Venta { Id = 10, Fecha = new DateTime(2025, 2, 22), TotalBruto = 16800,  TotalDescuento = 0,    TotalFinal = 16800,  Estado = 2, Id_sucursal = 1, Id_cliente = 3, Id_usuario = 1, Id_caja = 2 },
                // Marzo
                new Venta { Id = 11, Fecha = new DateTime(2025, 3, 4),  TotalBruto = 72000,  TotalDescuento = 3600, TotalFinal = 68400,  Estado = 2, Id_sucursal = 1, Id_cliente = 6, Id_usuario = 1, Id_caja = 3 },
                new Venta { Id = 12, Fecha = new DateTime(2025, 3, 9),  TotalBruto = 9800,   TotalDescuento = 0,    TotalFinal = 9800,   Estado = 2, Id_sucursal = 1, Id_cliente = 1, Id_usuario = 1, Id_caja = 3 },
                new Venta { Id = 13, Fecha = new DateTime(2025, 3, 15), TotalBruto = 47500,  TotalDescuento = 0,    TotalFinal = 47500,  Estado = 2, Id_sucursal = 1, Id_cliente = 4, Id_usuario = 1, Id_caja = 3 },
                new Venta { Id = 14, Fecha = new DateTime(2025, 3, 21), TotalBruto = 156000, TotalDescuento = 7800, TotalFinal = 148200, Estado = 2, Id_sucursal = 1, Id_cliente = 6, Id_usuario = 1, Id_caja = 3 },
                new Venta { Id = 15, Fecha = new DateTime(2025, 3, 28), TotalBruto = 22400,  TotalDescuento = 0,    TotalFinal = 22400,  Estado = 2, Id_sucursal = 1, Id_cliente = 2, Id_usuario = 1, Id_caja = 3 }
            );

            builder.Entity<VentaDetalle>().HasData(
                // Venta 1
                new VentaDetalle { Id = 1,  Id_venta = 1,  Id_producto = 1,  Cantidad = 1, PrecioUnitario = 8500,  CostoUnitario = 5500,  Descuento = 0, Subtotal = 8500,  MargenUnitario = 3000  },
                new VentaDetalle { Id = 2,  Id_venta = 1,  Id_producto = 2,  Cantidad = 2, PrecioUnitario = 4200,  CostoUnitario = 2600,  Descuento = 0, Subtotal = 8400,  MargenUnitario = 1600  },
                new VentaDetalle { Id = 3,  Id_venta = 1,  Id_producto = 12, Cantidad = 2, PrecioUnitario = 3200,  CostoUnitario = 2000,  Descuento = 0, Subtotal = 6400,  MargenUnitario = 1200  },
                new VentaDetalle { Id = 4,  Id_venta = 1,  Id_producto = 13, Cantidad = 1, PrecioUnitario = 7800,  CostoUnitario = 5100,  Descuento = 0, Subtotal = 7800,  MargenUnitario = 2700  },
                // Venta 2
                new VentaDetalle { Id = 5,  Id_venta = 2,  Id_producto = 5,  Cantidad = 1, PrecioUnitario = 85000, CostoUnitario = 58000, Descuento = 0, Subtotal = 85000, MargenUnitario = 27000 },
                // Venta 3
                new VentaDetalle { Id = 6,  Id_venta = 3,  Id_producto = 7,  Cantidad = 1, PrecioUnitario = 18500, CostoUnitario = 12000, Descuento = 925, Subtotal = 17575, MargenUnitario = 5575 },
                // Venta 4
                new VentaDetalle { Id = 7,  Id_venta = 4,  Id_producto = 8,  Cantidad = 1, PrecioUnitario = 42000, CostoUnitario = 28000, Descuento = 0, Subtotal = 42000, MargenUnitario = 14000 },
                new VentaDetalle { Id = 8,  Id_venta = 4,  Id_producto = 9,  Cantidad = 1, PrecioUnitario = 9800,  CostoUnitario = 6400,  Descuento = 0, Subtotal = 9800,  MargenUnitario = 3400  },
                new VentaDetalle { Id = 9,  Id_venta = 4,  Id_producto = 3,  Cantidad = 1, PrecioUnitario = 6800,  CostoUnitario = 4200,  Descuento = 0, Subtotal = 6800,  MargenUnitario = 2600  },
                // Venta 5
                new VentaDetalle { Id = 10, Id_venta = 5,  Id_producto = 10, Cantidad = 1, PrecioUnitario = 32000, CostoUnitario = 21000, Descuento = 1600, Subtotal = 30400, MargenUnitario = 9400 },
                // Venta 6
                new VentaDetalle { Id = 11, Id_venta = 6,  Id_producto = 8,  Cantidad = 1, PrecioUnitario = 42000, CostoUnitario = 28000, Descuento = 0, Subtotal = 42000, MargenUnitario = 14000 },
                // Venta 7
                new VentaDetalle { Id = 12, Id_venta = 7,  Id_producto = 4,  Cantidad = 1, PrecioUnitario = 12500, CostoUnitario = 8000,  Descuento = 0, Subtotal = 12500, MargenUnitario = 4500  },
                // Venta 8
                new VentaDetalle { Id = 13, Id_venta = 8,  Id_producto = 5,  Cantidad = 1, PrecioUnitario = 85000, CostoUnitario = 58000, Descuento = 4680, Subtotal = 80320, MargenUnitario = 22320 },
                new VentaDetalle { Id = 14, Id_venta = 8,  Id_producto = 6,  Cantidad = 1, PrecioUnitario = 72000, CostoUnitario = 50000, Descuento = 0, Subtotal = 72000, MargenUnitario = 22000 },
                // Venta 9
                new VentaDetalle { Id = 15, Id_venta = 9,  Id_producto = 7,  Cantidad = 1, PrecioUnitario = 18500, CostoUnitario = 12000, Descuento = 0, Subtotal = 18500, MargenUnitario = 6500  },
                new VentaDetalle { Id = 16, Id_venta = 9,  Id_producto = 11, Cantidad = 1, PrecioUnitario = 5500,  CostoUnitario = 3500,  Descuento = 0, Subtotal = 5500,  MargenUnitario = 2000  },
                new VentaDetalle { Id = 17, Id_venta = 9,  Id_producto = 2,  Cantidad = 1, PrecioUnitario = 4200,  CostoUnitario = 2600,  Descuento = 0, Subtotal = 4200,  MargenUnitario = 1600  },
                // Venta 10
                new VentaDetalle { Id = 18, Id_venta = 10, Id_producto = 9,  Cantidad = 1, PrecioUnitario = 9800,  CostoUnitario = 6400,  Descuento = 0, Subtotal = 9800,  MargenUnitario = 3400  },
                new VentaDetalle { Id = 19, Id_venta = 10, Id_producto = 3,  Cantidad = 1, PrecioUnitario = 6800,  CostoUnitario = 4200,  Descuento = 0, Subtotal = 6800,  MargenUnitario = 2600  },
                // Venta 11
                new VentaDetalle { Id = 20, Id_venta = 11, Id_producto = 6,  Cantidad = 1, PrecioUnitario = 72000, CostoUnitario = 50000, Descuento = 3600, Subtotal = 68400, MargenUnitario = 18400 },
                // Venta 12
                new VentaDetalle { Id = 21, Id_venta = 12, Id_producto = 9,  Cantidad = 1, PrecioUnitario = 9800,  CostoUnitario = 6400,  Descuento = 0, Subtotal = 9800,  MargenUnitario = 3400  },
                // Venta 13
                new VentaDetalle { Id = 22, Id_venta = 13, Id_producto = 8,  Cantidad = 1, PrecioUnitario = 42000, CostoUnitario = 28000, Descuento = 0, Subtotal = 42000, MargenUnitario = 14000 },
                new VentaDetalle { Id = 23, Id_venta = 13, Id_producto = 1,  Cantidad = 1, PrecioUnitario = 8500,  CostoUnitario = 5500,  Descuento = 0, Subtotal = 8500,  MargenUnitario = 3000  },
                // Venta 14
                new VentaDetalle { Id = 24, Id_venta = 14, Id_producto = 5,  Cantidad = 1, PrecioUnitario = 85000, CostoUnitario = 58000, Descuento = 7800, Subtotal = 77200, MargenUnitario = 19200 },
                new VentaDetalle { Id = 25, Id_venta = 14, Id_producto = 6,  Cantidad = 1, PrecioUnitario = 72000, CostoUnitario = 50000, Descuento = 0, Subtotal = 72000, MargenUnitario = 22000 },
                // Venta 15
                new VentaDetalle { Id = 26, Id_venta = 15, Id_producto = 7,  Cantidad = 1, PrecioUnitario = 18500, CostoUnitario = 12000, Descuento = 0, Subtotal = 18500, MargenUnitario = 6500  },
                new VentaDetalle { Id = 27, Id_venta = 15, Id_producto = 14, Cantidad = 1, PrecioUnitario = 4800,  CostoUnitario = 3100,  Descuento = 0, Subtotal = 4800,  MargenUnitario = 1700  }
            );

            builder.Entity<Pago>().HasData(
                new Pago { Id = 1,  Id_venta = 1,  Id_metodoPago = 1, Monto = 25700  },
                new Pago { Id = 2,  Id_venta = 2,  Id_metodoPago = 3, Monto = 85000  },
                new Pago { Id = 3,  Id_venta = 3,  Id_metodoPago = 1, Monto = 17575  },
                new Pago { Id = 4,  Id_venta = 4,  Id_metodoPago = 4, Monto = 54800  },
                new Pago { Id = 5,  Id_venta = 5,  Id_metodoPago = 2, Monto = 30400  },
                new Pago { Id = 6,  Id_venta = 6,  Id_metodoPago = 1, Monto = 42000  },
                new Pago { Id = 7,  Id_venta = 7,  Id_metodoPago = 5, Monto = 12500  },
                new Pago { Id = 8,  Id_venta = 8,  Id_metodoPago = 4, Monto = 88920  },
                new Pago { Id = 9,  Id_venta = 9,  Id_metodoPago = 1, Monto = 27300  },
                new Pago { Id = 10, Id_venta = 10, Id_metodoPago = 2, Monto = 16800  },
                new Pago { Id = 11, Id_venta = 11, Id_metodoPago = 3, Monto = 68400  },
                new Pago { Id = 12, Id_venta = 12, Id_metodoPago = 1, Monto = 9800   },
                new Pago { Id = 13, Id_venta = 13, Id_metodoPago = 4, Monto = 47500  },
                new Pago { Id = 14, Id_venta = 14, Id_metodoPago = 3, Monto = 148200 },
                new Pago { Id = 15, Id_venta = 15, Id_metodoPago = 1, Monto = 22400  }
            );
        }
    }
}
