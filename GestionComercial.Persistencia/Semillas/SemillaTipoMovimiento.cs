using GestionComercial.Dominio.Entidades.Movimientos;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaTipoMovimiento
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<TipoMovimientoStock>().HasData(
                new TipoMovimientoStock { Id = 1, Nombre = "Entrada",         Descripcion = "Ingreso de mercadería"    },
                new TipoMovimientoStock { Id = 2, Nombre = "Salida",          Descripcion = "Egreso de mercadería"     },
                new TipoMovimientoStock { Id = 3, Nombre = "Ajuste Positivo", Descripcion = "Ajuste positivo de stock" },
                new TipoMovimientoStock { Id = 4, Nombre = "Ajuste Negativo", Descripcion = "Ajuste negativo de stock" }
            );
        }
    }
}
