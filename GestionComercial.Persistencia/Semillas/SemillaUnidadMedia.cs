using GestionComercial.Dominio.Entidades.Producto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaUnidadMedida
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<UnidadMedida>().HasData(
                new UnidadMedida { Id = 1, Nombre = "Unidad", Abreviatura = "UN" },
                new UnidadMedida { Id = 2, Nombre = "Kilogramo", Abreviatura = "KG" },
                new UnidadMedida { Id = 3, Nombre = "Metro", Abreviatura = "MT" },
                new UnidadMedida { Id = 4, Nombre = "Litro", Abreviatura = "LT" },
                new UnidadMedida { Id = 5, Nombre = "Caja", Abreviatura = "CJA" }
            );
        }
    }
}
