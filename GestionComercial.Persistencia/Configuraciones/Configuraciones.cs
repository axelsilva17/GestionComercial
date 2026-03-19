using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionComercial.Persistencia.Configuraciones
{
    public class EmpresaConfiguracion : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> b)
        {
            b.ToTable("Empresa");
            b.HasKey(e => e.Id);
            b.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            b.Property(e => e.CUIT).HasMaxLength(13).IsRequired();
            b.Property(e => e.Direccion).HasMaxLength(200).IsRequired();
            b.Property(e => e.Email).HasMaxLength(100);
            b.Property(e => e.Telefono).HasMaxLength(20);
            b.HasIndex(e => e.CUIT).IsUnique();
        }
    }

    public class SucursalConfiguracion : IEntityTypeConfiguration<Sucursal>
    {
        public void Configure(EntityTypeBuilder<Sucursal> b)
        {
            b.ToTable("Sucursal");
            b.HasKey(s => s.Id);
            b.Property(s => s.Nombre).HasMaxLength(100).IsRequired();
            b.Property(s => s.Direccion).HasMaxLength(200).IsRequired();
            b.Property(s => s.Telefono).HasMaxLength(20);
            b.HasOne(s => s.Empresa).WithMany(e => e.Sucursales)
             .HasForeignKey(s => s.Id_empresa).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class RolConfiguracion : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> b)
        {
            b.ToTable("Rol");
            b.HasKey(r => r.Id);
            b.Property(r => r.Nombre).HasMaxLength(50).IsRequired();
            b.Property(r => r.Descripcion).HasMaxLength(200);
            b.HasIndex(r => r.Nombre).IsUnique();
        }
    }
    public class PermisoConfiguracion : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> b)
        {
            b.ToTable("Permiso");
            b.HasKey(p => p.Id);
            b.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            b.Property(p => p.Descripcion).HasMaxLength(200);
            b.HasIndex(p => p.Nombre).IsUnique();
        }
    }

    public class RolPermisoConfiguracion : IEntityTypeConfiguration<RolPermiso>
    {
        public void Configure(EntityTypeBuilder<RolPermiso> b)
        {
            b.ToTable("RolPermiso");
            b.HasKey(rp => rp.Id);
            b.HasOne(rp => rp.Rol).WithMany(r => r.RolPermisos)
             .HasForeignKey(rp => rp.Id_rol).OnDelete(DeleteBehavior.Restrict);
            b.HasOne(rp => rp.Permiso).WithMany(p => p.RolPermisos)
             .HasForeignKey(rp => rp.Id_permiso).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class TipoDocumentoConfiguracion : IEntityTypeConfiguration<TipoDocumento>
    {
        public void Configure(EntityTypeBuilder<TipoDocumento> b)
        {
            b.ToTable("TipoDocumento");
            b.HasKey(t => t.Id);
            b.Property(t => t.Nombre).HasMaxLength(50).IsRequired();
            b.Property(t => t.Descripcion).HasMaxLength(200);
        }
    }

    public class TipoMovimientoStockConfiguracion : IEntityTypeConfiguration<TipoMovimientoStock>
    {
        public void Configure(EntityTypeBuilder<TipoMovimientoStock> b)
        {
            b.ToTable("TipoMovimientoStock");
            b.HasKey(t => t.Id);
            b.Property(t => t.Nombre).HasMaxLength(50).IsRequired();
            b.Property(t => t.Descripcion).HasMaxLength(200);
        }

        public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
        {
            public void Configure(EntityTypeBuilder<Usuario> b)
            {
                b.ToTable("Usuario");
                b.HasKey(u => u.Id);
                b.Property(u => u.Nombre).HasMaxLength(50).IsRequired();
                b.Property(u => u.Apellido).HasMaxLength(50).IsRequired();
                b.Property(u => u.Email).HasMaxLength(100).IsRequired();
                b.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
                b.HasIndex(u => u.Email).IsUnique();
                b.Ignore(u => u.NombreCompleto);
                b.Ignore(u => u.Inicial);
                b.HasOne(u => u.Sucursal).WithMany(s => s.Usuarios)
                 .HasForeignKey(u => u.Id_sucursal).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(u => u.Rol).WithMany(r => r.Usuarios)
                 .HasForeignKey(u => u.Id_rol).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class UnidadMedidaConfiguracion : IEntityTypeConfiguration<UnidadMedida>
        {
            public void Configure(EntityTypeBuilder<UnidadMedida> b)
            {
                b.ToTable("UnidadMedida");
                b.HasKey(u => u.Id);
                b.Property(u => u.Nombre).HasMaxLength(50).IsRequired();
                b.Property(u => u.Abreviatura).HasMaxLength(10).IsRequired();
            }
        }

        public class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria>
        {
            public void Configure(EntityTypeBuilder<Categoria> b)
            {
                b.ToTable("Categoria");
                b.HasKey(c => c.Id);
                b.Property(c => c.Nombre).HasMaxLength(50).IsRequired();
                b.HasOne(c => c.CategoriaPadre).WithMany(c => c.SubCategorias)
                 .HasForeignKey(c => c.CategoriaPadre_id).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(c => c.Empresa).WithMany(e => e.Categorias)
                 .HasForeignKey(c => c.Id_empresa).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
        {
            public void Configure(EntityTypeBuilder<Producto> b)
            {
                b.ToTable("Producto");
                b.HasKey(p => p.Id);
                b.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
                b.Property(p => p.CodigoBarra).HasMaxLength(50);
                b.Property(p => p.Descripcion).HasMaxLength(500);
                b.Property(p => p.PrecioVentaActual).HasColumnType("decimal(18,2)");
                b.Property(p => p.PrecioCostoActual).HasColumnType("decimal(18,2)");
                b.Property(p => p.StockActual).HasColumnType("decimal(18,3)");
                b.Property(p => p.StockMinimo).HasColumnType("decimal(18,3)");
                b.Ignore(p => p.StockBajo);
                b.Ignore(p => p.Margen);
                b.HasIndex(p => p.CodigoBarra).IsUnique().HasFilter("[CodigoBarra] IS NOT NULL");
                b.HasIndex(p => p.Id_empresa);
                b.HasOne(p => p.Empresa).WithMany(e => e.Productos)
                 .HasForeignKey(p => p.Id_empresa).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(p => p.Categoria).WithMany(c => c.Productos)
                 .HasForeignKey(p => p.Id_categoria).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(p => p.UnidadMedida).WithMany(u => u.Productos)
                 .HasForeignKey(p => p.Id_unidadMedida).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
        {
            public void Configure(EntityTypeBuilder<Cliente> b)
            {
                b.ToTable("Cliente");
                b.HasKey(c => c.Id);
                b.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
                b.Property(c => c.Documento).HasMaxLength(20).IsRequired();
                b.Property(c => c.Telefono).HasMaxLength(20);
                b.Property(c => c.Email).HasMaxLength(100);
                b.Ignore(c => c.Inicial);
                b.HasOne(c => c.Empresa).WithMany(e => e.Clientes)
                 .HasForeignKey(c => c.Id_empresa).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class ProveedorConfiguracion : IEntityTypeConfiguration<Proveedor>
        {
            public void Configure(EntityTypeBuilder<Proveedor> b)
            {
                b.ToTable("Proveedor");
                b.HasKey(p => p.Id);
                b.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
                b.Property(p => p.Telefono).HasMaxLength(20);
                b.Property(p => p.Email).HasMaxLength(100);
                b.Property(p => p.CUIT).HasMaxLength(13);
                b.HasOne(p => p.Empresa).WithMany(e => e.Proveedores)
                 .HasForeignKey(p => p.Id_empresa).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class MetodoPagoConfiguracion : IEntityTypeConfiguration<MetodoPago>
        {
            public void Configure(EntityTypeBuilder<MetodoPago> b)
            {
                b.ToTable("MetodoPago");
                b.HasKey(m => m.Id);
                b.Property(m => m.Nombre).HasMaxLength(50).IsRequired();
                b.HasOne(m => m.Empresa).WithMany(e => e.MetodosPago)
                 .HasForeignKey(m => m.Id_empresa).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class PagoConfiguracion : IEntityTypeConfiguration<Pago>
        {
            public void Configure(EntityTypeBuilder<Pago> b)
            {
                b.ToTable("Pago");
                b.HasKey(p => p.Id);
                b.Property(p => p.Monto).HasColumnType("decimal(18,2)");
                b.HasOne(p => p.Venta).WithMany(v => v.Pagos)
                 .HasForeignKey(p => p.Id_venta).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(p => p.MetodoPago).WithMany(m => m.Pagos)
                 .HasForeignKey(p => p.Id_metodoPago).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(p => p.MovimientoCaja).WithMany()
                 .HasForeignKey(p => p.Id_movimientoCaja).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class VentaConfiguracion : IEntityTypeConfiguration<Venta>
        {
            public void Configure(EntityTypeBuilder<Venta> b)
            {
                b.ToTable("Venta");
                b.HasKey(v => v.Id);
                b.Property(v => v.TotalBruto).HasColumnType("decimal(18,2)");
                b.Property(v => v.TotalDescuento).HasColumnType("decimal(18,2)");
                b.Property(v => v.TotalFinal).HasColumnType("decimal(18,2)");
                b.Property(v => v.Observacion).HasMaxLength(500);
                // ── Campos de anulación ─────────────────────────────────────────
                b.Property(v => v.MotivoAnulacion).HasMaxLength(500);
                b.HasIndex(v => v.Fecha);
                b.HasIndex(v => new { v.Id_sucursal, v.Fecha });
                b.HasOne(v => v.Sucursal).WithMany(s => s.Ventas)
                 .HasForeignKey(v => v.Id_sucursal).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(v => v.Cliente).WithMany(c => c.Ventas)
                 .HasForeignKey(v => v.Id_cliente).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(v => v.Usuario).WithMany()
                 .HasForeignKey(v => v.Id_usuario).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(v => v.Caja).WithMany(c => c.Ventas)
                 .HasForeignKey(v => v.Id_caja).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class VentaDetalleConfiguracion : IEntityTypeConfiguration<VentaDetalle>
        {
            public void Configure(EntityTypeBuilder<VentaDetalle> b)
            {
                b.ToTable("VentaDetalle");
                b.HasKey(v => v.Id);
                b.Property(v => v.Cantidad).HasColumnType("decimal(18,3)");
                b.Property(v => v.PrecioUnitario).HasColumnType("decimal(18,2)");
                b.Property(v => v.CostoUnitario).HasColumnType("decimal(18,2)");
                b.Property(v => v.Descuento).HasColumnType("decimal(18,2)"); // Legacy
                b.Property(v => v.Subtotal).HasColumnType("decimal(18,2)");
                b.Property(v => v.MargenUnitario).HasColumnType("decimal(18,2)");
                b.HasOne(v => v.Venta).WithMany(v => v.Detalles)
                 .HasForeignKey(v => v.Id_venta).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(v => v.Producto).WithMany(p => p.VentaDetalles)
                 .HasForeignKey(v => v.Id_producto).OnDelete(DeleteBehavior.Restrict);
                // ── Descuentos e Impuestos por ítem ─────────────────────────────
                b.HasMany(v => v.Descuentos).WithOne(d => d.Detalle)
                 .HasForeignKey(d => d.Id_detalle).OnDelete(DeleteBehavior.Cascade);
                b.HasMany(v => v.Impuestos).WithOne(i => i.Detalle)
                 .HasForeignKey(i => i.Id_detalle).OnDelete(DeleteBehavior.Cascade);
            }
        }

        public class VentaDetalleDescuentoConfiguracion : IEntityTypeConfiguration<VentaDetalleDescuento>
        {
            public void Configure(EntityTypeBuilder<VentaDetalleDescuento> b)
            {
                b.ToTable("VentaDetalleDescuento");
                b.HasKey(d => d.Id);
                b.Property(d => d.Porcentaje).HasColumnType("decimal(18,2)");
                b.Property(d => d.Monto).HasColumnType("decimal(18,2)");
                b.Property(d => d.Descripcion).HasMaxLength(200);
            }
        }

        public class VentaDetalleImpuestoConfiguracion : IEntityTypeConfiguration<VentaDetalleImpuesto>
        {
            public void Configure(EntityTypeBuilder<VentaDetalleImpuesto> b)
            {
                b.ToTable("VentaDetalleImpuesto");
                b.HasKey(i => i.Id);
                b.Property(i => i.Porcentaje).HasColumnType("decimal(18,2)");
                b.Property(i => i.Monto).HasColumnType("decimal(18,2)");
            }
        }

        public class CompraConfiguracion : IEntityTypeConfiguration<Compra>
        {
            public void Configure(EntityTypeBuilder<Compra> b)
            {
                b.ToTable("Compra");
                b.HasKey(c => c.Id);
                b.Property(c => c.Total).HasColumnType("decimal(18,2)");
                b.Property(c => c.Observacion).HasMaxLength(500);
                b.HasOne(c => c.Proveedor).WithMany(p => p.Compras)
                 .HasForeignKey(c => c.Id_proveedor).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(c => c.Sucursal).WithMany(s => s.Compras)
                 .HasForeignKey(c => c.Id_sucursal).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(c => c.Usuario).WithMany()
                 .HasForeignKey(c => c.Id_usuario).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class CompraDetalleConfiguracion : IEntityTypeConfiguration<CompraDetalle>
        {
            public void Configure(EntityTypeBuilder<CompraDetalle> b)
            {
                b.ToTable("CompraDetalle");
                b.HasKey(c => c.Id);
                b.Property(c => c.Cantidad).HasColumnType("decimal(18,3)");
                b.Property(c => c.PrecioCosto).HasColumnType("decimal(18,2)");
                b.Property(c => c.Subtotal).HasColumnType("decimal(18,2)");
                b.HasOne(c => c.Compra).WithMany(c => c.Detalles)
                 .HasForeignKey(c => c.Id_compra).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(c => c.Producto).WithMany(p => p.CompraDetalles)
                 .HasForeignKey(c => c.Id_producto).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class MovimientoStockConfiguracion : IEntityTypeConfiguration<MovimientoStock>
        {
            public void Configure(EntityTypeBuilder<MovimientoStock> b)
            {
                b.ToTable("MovimientoStock");
                b.HasKey(m => m.Id);
                b.Property(m => m.Cantidad).HasColumnType("decimal(18,3)");
                b.Property(m => m.StockAnterior).HasColumnType("decimal(18,3)");
                b.Property(m => m.StockNuevo).HasColumnType("decimal(18,3)");
                b.Property(m => m.Observacion).HasMaxLength(200);
                b.HasIndex(m => new { m.Id_producto, m.Fecha });
                b.HasOne(m => m.Producto).WithMany(p => p.Movimientos)
                 .HasForeignKey(m => m.Id_producto).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(m => m.Sucursal).WithMany()
                 .HasForeignKey(m => m.Id_sucursal).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(m => m.Usuario).WithMany()
                 .HasForeignKey(m => m.Id_usuario).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class CajaConfiguracion : IEntityTypeConfiguration<Caja>
        {
            public void Configure(EntityTypeBuilder<Caja> b)
            {
                b.ToTable("Caja");
                b.HasKey(c => c.Id);
                b.Property(c => c.MontoInicial).HasColumnType("decimal(18,2)");
                b.Property(c => c.MontoFinal).HasColumnType("decimal(18,2)");
                b.Property(c => c.Observacion).HasMaxLength(500);
                b.Ignore(c => c.EstaAbierta);
                b.HasIndex(c => new { c.Id_sucursal, c.Estado });
                b.HasOne(c => c.Sucursal).WithMany(s => s.Cajas)
                 .HasForeignKey(c => c.Id_sucursal).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(c => c.UsuarioApertura).WithMany()
                 .HasForeignKey(c => c.UsuarioApertura_id).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(c => c.UsuarioCierre).WithMany()
                 .HasForeignKey(c => c.UsuarioCierre_id).OnDelete(DeleteBehavior.Restrict);
            }
        }

        public class MovimientoCajaConfiguracion : IEntityTypeConfiguration<TipoMovimientoCaja>
        {
            public void Configure(EntityTypeBuilder<TipoMovimientoCaja> b)
            {
                b.ToTable("MovimientoCaja");
                b.HasKey(m => m.Id);
                b.Property(m => m.Monto).HasColumnType("decimal(18,2)");
                b.Property(m => m.Concepto).HasMaxLength(200);
                b.HasOne(m => m.Caja).WithMany(c => c.Movimientos)
                 .HasForeignKey(m => m.Id_caja).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(m => m.Usuario).WithMany()
                 .HasForeignKey(m => m.Id_usuario).OnDelete(DeleteBehavior.Restrict);
                // ── Link a Venta (para trazabilidad) ────────────────────────────
                b.HasOne(m => m.Venta).WithMany()
                 .HasForeignKey(m => m.Id_venta).OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}