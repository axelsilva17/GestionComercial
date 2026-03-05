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
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraDetalle> CompraDetalles { get; set; }
        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<TipoMovimientoStock> TiposMovimientoStock { get; set; }
        public DbSet<Caja> Cajas { get; set; }
        public DbSet<TipoMovimientoCaja> MovimientosCaja { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestionComercialContext).Assembly);
            base.OnModelCreating(modelBuilder);

            SemillaRoles.Sembrar(modelBuilder);
            SemillaTipoMovimiento.Sembrar(modelBuilder);
            SemillaTipoDocumento.Sembrar(modelBuilder);
            SemillaPermisos.Sembrar(modelBuilder);
            SemillaInicial.Sembrar(modelBuilder);

        }
    }
}