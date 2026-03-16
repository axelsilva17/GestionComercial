
using GestionComercial.Dominio.Entidades.Producto;
using Microsoft.EntityFrameworkCore;

public static class SemillaCategoria
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Categoria>().HasData(
                // Categorías padre
                new Categoria { Id = 1, Nombre = "Herramientas",  CategoriaPadre_id = null, Id_empresa = 1 },
                new Categoria { Id = 2, Nombre = "Materiales",    CategoriaPadre_id = null, Id_empresa = 1 },
                new Categoria { Id = 3, Nombre = "Electricidad",  CategoriaPadre_id = null, Id_empresa = 1 },
                new Categoria { Id = 4, Nombre = "Pintura",       CategoriaPadre_id = null, Id_empresa = 1 },
                new Categoria { Id = 5, Nombre = "Plomería",      CategoriaPadre_id = null, Id_empresa = 1 },
                // Subcategorías
                new Categoria { Id = 6, Nombre = "Manuales",      CategoriaPadre_id = 1,    Id_empresa = 1 },
                new Categoria { Id = 7, Nombre = "Eléctricas",    CategoriaPadre_id = 1,    Id_empresa = 1 },
                new Categoria { Id = 8, Nombre = "Látex",         CategoriaPadre_id = 4,    Id_empresa = 1 },
                new Categoria { Id = 9, Nombre = "Esmalte",       CategoriaPadre_id = 4,    Id_empresa = 1 }
            );
        }
    }