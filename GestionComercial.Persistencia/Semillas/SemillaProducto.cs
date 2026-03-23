

using GestionComercial.Dominio.Entidades.Producto;
using Microsoft.EntityFrameworkCore;

public static class SemillaProducto
{
    public static void Sembrar(ModelBuilder builder)
    {
        builder.Entity<Producto>().HasData(
            // Herramientas manuales
            new Producto { Id = 1,  Nombre = "Martillo 500g",          CodigoBarra = "7790001000001", PrecioVentaActual = 8500,   PrecioCostoActual = 5500,   StockActual = 25,  StockMinimo = 5,  Id_empresa = 1, Id_categoria = 6, Id_unidadMedida = 1 },
            new Producto { Id = 2,  Nombre = "Destornillador Philips",  CodigoBarra = "7790001000002", PrecioVentaActual = 4200,   PrecioCostoActual = 2600,   StockActual = 40,  StockMinimo = 8,  Id_empresa = 1, Id_categoria = 6, Id_unidadMedida = 1 },
            new Producto { Id = 3,  Nombre = "Alicate Universal",       CodigoBarra = "7790001000003", PrecioVentaActual = 6800,   PrecioCostoActual = 4200,   StockActual = 18,  StockMinimo = 5,  Id_empresa = 1, Id_categoria = 6, Id_unidadMedida = 1 },
            new Producto { Id = 4,  Nombre = "Sierra Arco",             CodigoBarra = "7790001000004", PrecioVentaActual = 12500,  PrecioCostoActual = 8000,   StockActual = 12,  StockMinimo = 3,  Id_empresa = 1, Id_categoria = 6, Id_unidadMedida = 1 },
            // Herramientas eléctricas
            new Producto { Id = 5,  Nombre = "Taladro 650W",            CodigoBarra = "7790001000005", PrecioVentaActual = 85000,  PrecioCostoActual = 58000,  StockActual = 8,   StockMinimo = 2,  Id_empresa = 1, Id_categoria = 7, Id_unidadMedida = 1 },
            new Producto { Id = 6,  Nombre = "Amoladora 115mm",         CodigoBarra = "7790001000006", PrecioVentaActual = 72000,  PrecioCostoActual = 50000,  StockActual = 6,   StockMinimo = 2,  Id_empresa = 1, Id_categoria = 7, Id_unidadMedida = 1 },
            // Pintura látex
            new Producto { Id = 7,  Nombre = "Látex Interior Blanco 4L",CodigoBarra = "7790001000007", PrecioVentaActual = 18500,  PrecioCostoActual = 12000,  StockActual = 30,  StockMinimo = 6,  Id_empresa = 1, Id_categoria = 8, Id_unidadMedida = 4 },
            new Producto { Id = 8,  Nombre = "Látex Exterior 10L",      CodigoBarra = "7790001000008", PrecioVentaActual = 42000,  PrecioCostoActual = 28000,  StockActual = 15,  StockMinimo = 4,  Id_empresa = 1, Id_categoria = 8, Id_unidadMedida = 4 },
            // Pintura esmalte
            new Producto { Id = 9,  Nombre = "Esmalte Sintético 1L",    CodigoBarra = "7790001000009", PrecioVentaActual = 9800,   PrecioCostoActual = 6400,   StockActual = 20,  StockMinimo = 5,  Id_empresa = 1, Id_categoria = 9, Id_unidadMedida = 4 },
            new Producto { Id = 10, Nombre = "Esmalte Negro 4L",        CodigoBarra = "7790001000010", PrecioVentaActual = 32000,  PrecioCostoActual = 21000,  StockActual = 10,  StockMinimo = 3,  Id_empresa = 1, Id_categoria = 9, Id_unidadMedida = 4 },
            // Electricidad
            new Producto { Id = 11, Nombre = "Cable Unipolar 1.5mm x5m",CodigoBarra = "7790001000011", PrecioVentaActual = 5500,   PrecioCostoActual = 3500,   StockActual = 50,  StockMinimo = 10, Id_empresa = 1, Id_categoria = 3, Id_unidadMedida = 3 },
            new Producto { Id = 12, Nombre = "Tomacorriente Doble",      CodigoBarra = "7790001000012", PrecioVentaActual = 3200,   PrecioCostoActual = 2000,   StockActual = 35,  StockMinimo = 8,  Id_empresa = 1, Id_categoria = 3, Id_unidadMedida = 1 },
            new Producto { Id = 13, Nombre = "Disyuntor 16A",            CodigoBarra = "7790001000013", PrecioVentaActual = 7800,   PrecioCostoActual = 5100,   StockActual = 22,  StockMinimo = 5,  Id_empresa = 1, Id_categoria = 3, Id_unidadMedida = 1 },
            // Plomería
            new Producto { Id = 14, Nombre = "Caño PVC 1\" x3m",        CodigoBarra = "7790001000014", PrecioVentaActual = 4800,   PrecioCostoActual = 3100,   StockActual = 40,  StockMinimo = 8,  Id_empresa = 1, Id_categoria = 5, Id_unidadMedida = 3 },
            new Producto { Id = 15, Nombre = "Codo PVC 90° 1\"",        CodigoBarra = "7790001000015", PrecioVentaActual = 850,    PrecioCostoActual = 520,    StockActual = 3,   StockMinimo = 10, Id_empresa = 1, Id_categoria = 5, Id_unidadMedida = 1 }
        );
    }
}