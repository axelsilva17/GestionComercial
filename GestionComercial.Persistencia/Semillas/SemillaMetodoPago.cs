using GestionComercial.Dominio.Entidades.Pagos;
using Microsoft.EntityFrameworkCore;

public static class SemillaMetodoPago
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<MetodoPago>().HasData(
                new MetodoPago { Id = 1, Nombre = "Efectivo",         Categoria = "Efectivo",     Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 2, Nombre = "Débito",           Categoria = "Tarjeta",      Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 3, Nombre = "Crédito",          Categoria = "Tarjeta",      Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 4, Nombre = "Transferencia",    Categoria = "Transferencia", Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 5, Nombre = "Mercado Pago",     Categoria = "Otro",         Activo = true, Id_empresa = 1 }
            );
        }
    }