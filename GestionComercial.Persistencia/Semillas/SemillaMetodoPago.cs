using GestionComercial.Dominio.Entidades.Pagos;
using Microsoft.EntityFrameworkCore;

public static class SemillaMetodoPago
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<MetodoPago>().HasData(
                new MetodoPago { Id = 1, Nombre = "Efectivo",         EsEfectivo = true,  Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 2, Nombre = "Débito",           EsEfectivo = false, Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 3, Nombre = "Crédito",          EsEfectivo = false, Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 4, Nombre = "Transferencia",    EsEfectivo = false, Activo = true, Id_empresa = 1 },
                new MetodoPago { Id = 5, Nombre = "Mercado Pago",     EsEfectivo = false, Activo = true, Id_empresa = 1 }
            );
        }
    }