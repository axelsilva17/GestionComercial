using GestionComercial.Dominio.Entidades.Mantenimiento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.ConfiguracionEntidades
{
    public class MantenimientoLogConfiguration : IEntityTypeConfiguration<MantenimientoLog>
    {
        public void Configure(EntityTypeBuilder<MantenimientoLog> builder)
        {
            builder.ToTable("MantenimientoLog");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Resultado).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Detalles).HasMaxLength(2000);
        }
    }
}
