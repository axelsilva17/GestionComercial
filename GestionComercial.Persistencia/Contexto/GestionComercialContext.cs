using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Configuracion;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Entidades.Vistas;
using GestionComercial.Persistencia.Semillas;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Contexto
{
    public class GestionComercialContext : DbContext
    {
        public GestionComercialContext(DbContextOptions<GestionComercialContext> options)
            : base(options) { }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }
        public DbSet<UnidadMedida> UnidadesMedida { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoDocumento> TiposDocumento { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }
        public DbSet<VentaDetalleDescuento> VentaDetalleDescuentos { get; set; }
        public DbSet<VentaDetalleImpuesto> VentaDetalleImpuestos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraDetalle> CompraDetalles { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<TipoMovimientoStock> TiposMovimientoStock { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<TipoMovimientoCaja> MovimientosCaja { get; set; }
        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }
        public DbSet<TablaAuditada> TablasAuditadas { get; set; }
        public DbSet<GestionComercial.Dominio.Entidades.Proveedores.ProveedorProductoCosto> ProveedorProductoCostos { get; set; }
        public DbSet<DescuentoConfiguracion> DescuentoConfiguraciones { get; set; }
        public DbSet<DescuentoMetodoPago> DescuentoMetodosPago { get; set; }

        // ── Configuración ──────────────────────────────────────────
        public DbSet<BackupConfig> BackupConfigs { get; set; }

        // ── Vistas (entidades de solo lectura) ──────────────────────
        public DbSet<VistaVentasResumida> VistaVentasResumidas { get; set; }
        public DbSet<VistaProductosConStock> VistaProductosConStock { get; set; }
        public DbSet<VistaMovimientosStock> VistaMovimientosStock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestionComercialContext).Assembly);
            base.OnModelCreating(modelBuilder);

            // ── Índices para optimizar búsquedas y filtros ──
            // Producto: búsqueda por código de barra + empresa (importación masiva)
            modelBuilder.Entity<Producto>()
                .HasIndex(p => new { p.CodigoBarra, p.Id_empresa })
                .HasDatabaseName("IX_Producto_CodigoBarra_Empresa")
                .IsUnique();

            // Producto: filtro por empresa (listado principal)
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Id_empresa)
                .HasDatabaseName("IX_Producto_IdEmpresa");

            // Producto: búsqueda por categoría
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Id_categoria)
                .HasDatabaseName("IX_Producto_IdCategoria");

            // Producto: búsqueda por nombre
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Nombre)
                .HasDatabaseName("IX_Producto_Nombre");

            // Producto: búsqueda por código de barra (individual)
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.CodigoBarra)
                .HasDatabaseName("IX_Producto_CodigoBarra");

            // Cliente: búsqueda por empresa
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Id_empresa)
                .HasDatabaseName("IX_Cliente_IdEmpresa");

            // Cliente: búsqueda por nombre
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Nombre)
                .HasDatabaseName("IX_Cliente_Nombre");

            // Proveedor: búsqueda por nombre
            modelBuilder.Entity<Proveedor>()
                .HasIndex(p => p.Nombre)
                .HasDatabaseName("IX_Proveedor_Nombre");

            // ── Venta: índices para filtrado por fecha y combinaciones comunes ──
            modelBuilder.Entity<Venta>()
                .HasIndex(v => v.Fecha)
                .HasDatabaseName("IX_Venta_Fecha");

            modelBuilder.Entity<Venta>()
                .HasIndex(v => new { v.Id_sucursal, v.Fecha })
                .HasDatabaseName("IX_Venta_Sucursal_Fecha");

            modelBuilder.Entity<Venta>()
                .HasIndex(v => new { v.Id_usuario, v.Fecha })
                .HasDatabaseName("IX_Venta_Usuario_Fecha");

            // BackupConfig: singleton configuration table
            modelBuilder.Entity<BackupConfig>(entity =>
            {
                entity.ToTable("BackupConfig");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Frecuencia).HasConversion<int>();
                entity.Property(e => e.DiaSemana).HasConversion<int?>();
                entity.Property(e => e.HoraProgramada).HasConversion<string?>();
            });

            SemillaRoles.Sembrar(modelBuilder);
            SemillaTipoMovimiento.Sembrar(modelBuilder);
            SemillaTipoDocumento.Sembrar(modelBuilder);
            SemillaPermisos.Sembrar(modelBuilder);
            SemillaInicial.Sembrar(modelBuilder);
            SemillaUnidadMedida.Sembrar(modelBuilder);
            SemillaCategoria.Sembrar(modelBuilder);
            SemillaMetodoPago.Sembrar(modelBuilder);
            SemillaProveedor.Sembrar(modelBuilder);
            SemillaCliente.Sembrar(modelBuilder);
            SemillaProducto.Sembrar(modelBuilder);
            SemillaCaja.Sembrar(modelBuilder);
            SemillaVentas.Sembrar(modelBuilder);
            SemillaCompras.Sembrar(modelBuilder);
            SemillaUsuario.Sembrar(modelBuilder);

        }
    }
}
