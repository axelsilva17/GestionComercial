using GestionComercial.Dominio.Entidades.Vistas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones.Vistas
{
    ///     /// Configuración de EF Core para la vista VistaMovimientosStock.
    public class VistaMovimientosStockConfiguration : IEntityTypeConfiguration<VistaMovimientosStock>
    {
        public void Configure(EntityTypeBuilder<VistaMovimientosStock> builder)
        {
            builder.HasNoKey();
            builder.ToView("VistaMovimientosStock");
        }
    }
}