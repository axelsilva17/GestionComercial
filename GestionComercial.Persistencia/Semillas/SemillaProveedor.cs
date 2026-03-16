

using GestionComercial.Dominio.Entidades.Proveedores;
using Microsoft.EntityFrameworkCore;

public static class SemillaProveedor
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Proveedor>().HasData(
                new Proveedor { Id = 1, Nombre = "Distribuidora Norte",   Telefono = "3794111111", Email = "norte@proveedor.com",    CUIT = "30-11111111-1", Id_empresa = 1 },
                new Proveedor { Id = 2, Nombre = "Pinturerias del Sur",   Telefono = "3794222222", Email = "sur@pinturerias.com",    CUIT = "30-22222222-2", Id_empresa = 1 },
                new Proveedor { Id = 3, Nombre = "Electro Mayorista",     Telefono = "3794333333", Email = "ventas@electro.com",     CUIT = "30-33333333-3", Id_empresa = 1 },
                new Proveedor { Id = 4, Nombre = "Materiales El Constructor", Telefono = "3794444444", Email = "info@constructor.com", CUIT = "30-44444444-4", Id_empresa = 1 }
            );
        }
    }