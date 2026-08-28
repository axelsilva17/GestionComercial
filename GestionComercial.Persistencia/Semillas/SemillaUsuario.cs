using GestionComercial.Dominio.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaUsuario
    {
        public static void Sembrar(ModelBuilder builder)
        {
            // Usuarios demo — contraseñas hasheadas con BCrypt (work factor 10)
            // Admin123! / Vendedor123! / Gerente123!
            builder.Entity<Usuario>().HasData(
                new
                {
                    Id = 1,
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    Email = "admin@miempresa.com",
                    PasswordHash = "$2a$10$vZSSeTQhuOMqZQnUDpuO2.cMfnBcqwuzKGUR4jeq5v96n6rD0e13C",
                    Id_sucursal = 1,
                    Id_rol = 2, // Administrador
                    IntentosFallidos = 0,
                    Activo = true,
                    FechaAlta = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = 2,
                    Nombre = "Vendedor",
                    Apellido = "Demo",
                    Email = "vendedor@miempresa.com",
                    PasswordHash = "$2a$10$LUIblGp1Yji4FbTt64k3e.e8I5nNfDu4WznoJ1P3DwB6WIU9g246a",
                    Id_sucursal = 1,
                    Id_rol = 3, // Vendedor
                    IntentosFallidos = 0,
                    Activo = true,
                    FechaAlta = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                },
                new
                {
                    Id = 3,
                    Nombre = "Gerente",
                    Apellido = "Demo",
                    Email = "gerente@miempresa.com",
                    PasswordHash = "$2a$10$zd3k2FDRQhwROhoa3URlg.kHvSFnim0Hhi/zukq3hAOth4P/EL83y",
                    Id_sucursal = 1,
                    Id_rol = 1, // Gerente
                    IntentosFallidos = 0,
                    Activo = true,
                    FechaAlta = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                }
            );
        }
    }
}
