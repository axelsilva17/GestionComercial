using GestionComercial.Dominio.Entidades.Caja;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaCaja
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Caja>().HasData(
                new Caja
                {
                    Id                 = 1,
                    FechaApertura      = new DateTime(2025, 10, 1, 8, 0, 0),
                    FechaCierre        = new DateTime(2025, 10, 1, 20, 0, 0),
                    MontoInicial       = 50000,
                    MontoFinal         = 285000,
                    Estado             = 2,
                    UsuarioApertura_id = 1,
                    UsuarioCierre_id   = 1,
                    Id_sucursal        = 1,
                },
                new Caja
                {
                    Id                 = 2,
                    FechaApertura      = new DateTime(2025, 11, 1, 8, 0, 0),
                    FechaCierre        = new DateTime(2025, 11, 1, 20, 0, 0),
                    MontoInicial       = 50000,
                    MontoFinal         = 320000,
                    Estado             = 2,
                    UsuarioApertura_id = 1,
                    UsuarioCierre_id   = 1,
                    Id_sucursal        = 1,
                },
                new Caja
                {
                    Id                 = 3,
                    FechaApertura      = new DateTime(2025, 12, 1, 8, 0, 0),
                    FechaCierre        = new DateTime(2025, 12, 1, 20, 0, 0),
                    MontoInicial       = 50000,
                    MontoFinal         = 410000,
                    Estado             = 2,
                    UsuarioApertura_id = 1,
                    UsuarioCierre_id   = 1,
                    Id_sucursal        = 1,
                },
                new Caja
                {
                    Id                 = 4,
                    FechaApertura      = new DateTime(2026, 1, 1, 8, 0, 0),
                    FechaCierre        = new DateTime(2026, 1, 1, 20, 0, 0),
                    MontoInicial       = 50000,
                    MontoFinal         = 380000,
                    Estado             = 2,
                    UsuarioApertura_id = 1,
                    UsuarioCierre_id   = 1,
                    Id_sucursal        = 1,
                },
                new Caja
                {
                    Id                 = 5,
                    FechaApertura      = new DateTime(2026, 2, 1, 8, 0, 0),
                    FechaCierre        = new DateTime(2026, 2, 1, 20, 0, 0),
                    MontoInicial       = 50000,
                    MontoFinal         = 450000,
                    Estado             = 2,
                    UsuarioApertura_id = 1,
                    UsuarioCierre_id   = 1,
                    Id_sucursal        = 1,
                },
                new Caja
                {
                    Id                 = 6,
                    FechaApertura      = new DateTime(2026, 3, 1, 8, 0, 0),
                    FechaCierre        = new DateTime(2026, 3, 1, 20, 0, 0),
                    MontoInicial       = 50000,
                    MontoFinal         = 520000,
                    Estado             = 2,
                    UsuarioApertura_id = 1,
                    UsuarioCierre_id   = 1,
                    Id_sucursal        = 1,
                }
            );
        }
    }
}
