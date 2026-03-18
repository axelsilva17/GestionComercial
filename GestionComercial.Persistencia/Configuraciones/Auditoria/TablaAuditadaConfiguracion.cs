using GestionComercial.Dominio.Entidades.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones.Auditoria
{
    /// <summary>
    /// Configuración de Entity Framework para TablaAuditada.
    /// </summary>
    public class TablaAuditadaConfiguracion : IEntityTypeConfiguration<TablaAuditada>
    {
        public void Configure(EntityTypeBuilder<TablaAuditada> builder)
        {
            builder.ToTable("TablasAuditadas");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.NombreTabla)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Descripcion)
                .HasMaxLength(255);

            builder.Property(t => t.CamposExcluidos)
                .HasMaxLength(500);

            builder.Property(t => t.Habilitada)
                .HasDefaultValue(true);

            builder.Property(t => t.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            // Índice único para evitar duplicados
            builder.HasIndex(t => t.NombreTabla)
                .IsUnique();
        }
    }
}
