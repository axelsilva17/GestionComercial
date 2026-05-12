using GestionComercial.Dominio.Entidades.Vistas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones.Vistas
{
    /// <summary>
    /// Configuración de EF Core para la vista VistaProductosConStock.
    /// </summary>
    public class VistaProductosConStockConfiguration : IEntityTypeConfiguration<VistaProductosConStock>
    {
        public void Configure(EntityTypeBuilder<VistaProductosConStock> builder)
        {
            builder.HasNoKey();
            builder.ToView("VistaProductosConStock");
        }
    }
}