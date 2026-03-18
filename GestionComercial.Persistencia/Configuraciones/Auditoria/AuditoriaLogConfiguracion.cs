using GestionComercial.Dominio.Entidades.Auditoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones.Auditoria
{
    /// <summary>
    /// Configuración de Entity Framework para AuditoriaLog.
    /// </summary>
    public class AuditoriaLogConfiguracion : IEntityTypeConfiguration<AuditoriaLog>
    {
        public void Configure(EntityTypeBuilder<AuditoriaLog> builder)
        {
            builder.ToTable("AuditoriaLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.NombreTabla)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.TipoOperacion)
                .IsRequired();

            builder.Property(a => a.NombreUsuario)
                .HasMaxLength(100);

            builder.Property(a => a.ValoresAnteriores)
                .HasColumnType("nvarchar(max)");

            builder.Property(a => a.ValoresNuevos)
                .HasColumnType("nvarchar(max)");

            builder.Property(a => a.Workstation)
                .HasMaxLength(50);

            // Índices para优化的查询性能
            builder.HasIndex(a => new { a.NombreTabla, a.RegistroId });
            builder.HasIndex(a => a.FechaOperacion);
            builder.HasIndex(a => a.IdUsuario);
            builder.HasIndex(a => a.IdEmpresa);
            builder.HasIndex(a => a.IdSucursal);

            // Relaciones
            builder.HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Empresa)
                .WithMany()
                .HasForeignKey(a => a.IdEmpresa)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Sucursal)
                .WithMany()
                .HasForeignKey(a => a.IdSucursal)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
