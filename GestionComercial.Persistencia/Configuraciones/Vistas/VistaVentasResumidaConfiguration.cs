using GestionComercial.Dominio.Entidades.Vistas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones.Vistas
{
    ///     /// Configuración de EF Core para la vista VistaVentasResumidas.
    public class VistaVentasResumidaConfiguration : IEntityTypeConfiguration<VistaVentasResumida>
    {
        public void Configure(EntityTypeBuilder<VistaVentasResumida> builder)
        {
            builder.HasNoKey();
            builder.ToView("VistaVentasResumidas");
        }
    }
}