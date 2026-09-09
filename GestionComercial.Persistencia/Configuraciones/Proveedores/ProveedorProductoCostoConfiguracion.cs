using GestionComercial.Dominio.Entidades.Proveedores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones.Proveedores
{
    /// <summary>
    /// Entity Framework configuration for ProveedorProductoCosto.
    /// Maps the FK navigations to the real Proveedor/Producto entities and to the
    /// physical columns IdProveedor/IdProducto (previously inferred as shadow
    /// ProveedorId/ProductoId, which broke queries with "no such column").
    /// </summary>
    public class ProveedorProductoCostoConfiguracion : IEntityTypeConfiguration<ProveedorProductoCosto>
    {
        public void Configure(EntityTypeBuilder<ProveedorProductoCosto> b)
        {
            b.ToTable("ProveedorProductoCostos");
            b.HasKey(e => e.Id);

            b.HasOne(e => e.Proveedor)
                .WithMany()
                .HasForeignKey(e => e.IdProveedor)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.IdProducto)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(e => e.IdProveedor)
                .HasDatabaseName("IX_ProveedorProductoCostos_IdProveedor");

            b.HasIndex(e => e.IdProducto)
                .HasDatabaseName("IX_ProveedorProductoCostos_IdProducto");

            b.HasIndex(e => new { e.IdProveedor, e.IdProducto })
                .IsUnique()
                .HasDatabaseName("IX_ProveedorProductoCostos_ProveedorProducto");
        }
    }
}
