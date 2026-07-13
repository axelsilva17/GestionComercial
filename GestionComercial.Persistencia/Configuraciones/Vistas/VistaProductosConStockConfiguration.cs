using GestionComercial.Dominio.Entidades.Vistas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones.Vistas
{
    ///     /// Configuración de EF Core para la vista VistaProductosConStock.
    public class VistaProductosConStockConfiguration : IEntityTypeConfiguration<VistaProductosConStock>
    {
        public void Configure(EntityTypeBuilder<VistaProductosConStock> builder)
        {
            builder.HasNoKey();
            builder.ToView("VistaProductosConStock");
        }
    }
}