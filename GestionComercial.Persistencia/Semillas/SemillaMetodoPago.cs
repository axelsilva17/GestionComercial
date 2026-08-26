using GestionComercial.Dominio.Entidades.Pagos;
using Microsoft.EntityFrameworkCore;

public static class SemillaMetodoPago
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<MetodoPago>().HasData(
                new MetodoPago { Id = 1, Nombre = "Efectivo",         Categoria = "Efectivo",     Subcategoria = null,            Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 2, Nombre = "Débito",           Categoria = "Tarjeta",      Subcategoria = "Debito",         Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 3, Nombre = "Crédito",          Categoria = "Tarjeta",      Subcategoria = "Credito",        Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 4, Nombre = "Transferencia",    Categoria = "Transferencia", Subcategoria = null,            Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 5, Nombre = "Mercado Pago",     Categoria = "Otro",         Subcategoria = null,            Activo = true, Id_empresa = 1 }
            );
        }
    }