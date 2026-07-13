using GestionComercial.Dominio.Entidades.Organizacion;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaInicial
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Empresa>().HasData(
                new Empresa
                {
                    Id       = 1,
                    Nombre   = "Mi Empresa",
                    CUIT     = "20-12345678-9",
                    Direccion = "Dirección Principal 123",
                    Email    = "admin@miempresa.com",
                    Telefono = "3794000000",
                    Activo   = true,
                }
            );

            builder.Entity<Sucursal>().HasData(
                new Sucursal
                {
                    Id         = 1,
                    Nombre     = "Casa Central",
                    Direccion  = "Dirección Principal 123",
                    Telefono   = "3794000000",
                    Activo    = true,
                    Id_empresa = 1,
                }
            );
        }
    }
}
