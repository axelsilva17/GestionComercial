using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaInicial
    {
        // Hash generado con BCrypt.Net-Next workFactor 12 para "Admin1234"
        // Regenerar si cambia la librería: BC.HashPassword("Admin1234", 12)
        private const string AdminPasswordHash = "$2a$12$xTXms.c66F43LdABY4LZjeXhAJmZSwEeYircoRBHDnBy4cqkklYFu";

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

            builder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id           = 1,
                    Nombre       = "Admin",
                    Apellido     = "Sistema",
                    Email        = "admin@sistema.com",
                    PasswordHash = AdminPasswordHash,
                    Activo       = true,
                    Id_sucursal  = 1,
                    Id_rol       = 1,
                }
            );
        }
    }
}
