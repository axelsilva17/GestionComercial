using GestionComercial.Dominio.Entidades.Cliente;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaTipoDocumento
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<TipoDocumento>().HasData(
                new TipoDocumento { Id = 1, Nombre = "DNI",      Descripcion = "Documento Nacional de Identidad"          },
                new TipoDocumento { Id = 2, Nombre = "CUIT",     Descripcion = "Clave Única de Identificación Tributaria" },
                new TipoDocumento { Id = 3, Nombre = "Pasaporte",Descripcion = "Pasaporte"                                }
            );
        }
    }
}
