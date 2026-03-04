using GestionComercial.Dominio.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaRoles
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Rol>().HasData(
                new Rol { Id_rol = 1, Nombre = "Gerente",       Descripcion = "Acceso total al sistema" },
                new Rol { Id_rol = 2, Nombre = "Administrador", Descripcion = "Administración general" },
                new Rol { Id_rol = 3, Nombre = "Vendedor",      Descripcion = "Operaciones de venta" }
            );
        }
    }
}
