using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaInicial
    {
        // Hashes generados con BCrypt.Net-Next workFactor 12
        private const string AdminHash    = "$2a$12$1afFAY7Q1dY9UOpV5EboqOM9P1IO41RZz4F01zEqC918SeOU0qaRy"; // admin12345
        private const string GerenteHash  = "$2a$12$NKA/6TaLtSB80UsdZUsZN.uO0IhAMH03WPDNeRQMOHrN/XRTECI9a"; // Gerente2024!
        private const string VendedorHash = "$2a$12$v4qlp9oXiSIn8kCyfNdmU.fQJMAETzMpXvXVF9h5U.TnxOvq1yolu"; // Vendedor2024!

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
                    Nombre       = "Administrador",
                    Apellido     = "Sistema",
                    Email        = "admin@sistema.com",
                    PasswordHash = AdminHash,
                    Activo       = true,
                    Id_sucursal  = 1,
                    Id_rol       = 2, // Administrador
                },
                new Usuario
                {
                    Id           = 2,
                    Nombre       = "Gerente",
                    Apellido     = "General",
                    Email        = "gerente@sistema.com",
                    PasswordHash = GerenteHash,
                    Activo       = true,
                    Id_sucursal  = 1,
                    Id_rol       = 1, // Gerente
                },
                new Usuario
                {
                    Id           = 3,
                    Nombre       = "Vendedor",
                    Apellido     = "Sistema",
                    Email        = "vendedor@sistema.com",
                    PasswordHash = VendedorHash,
                    Activo       = true,
                    Id_sucursal  = 1,
                    Id_rol       = 3, // Vendedor
                }
            );
        }
    }
}
