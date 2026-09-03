using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Persistencia.Contexto;
using GestionComercial.Persistencia.Repositorio;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GestionComercialContext _context;

        public IProductoRepositorio        Productos        { get; }
        public IClienteRepositorio         Clientes         { get; }
        public IProveedorRepositorio       Proveedores      { get; }
        public IVentaRepostorio            Ventas           { get; }
        public ICompraRepositorio          Compras          { get; }
        public ICajaRepositorio            Cajas            { get; }
        public IMovimientoStockRepositorio MovimientosStock { get; }
        public IMovimientoCajaRepositorio  MovimientosCaja  { get; }
        public IUsuarioRepositorio         Usuarios         { get; }
        public ICategoriaRepositorio      Categorias       { get; }
        public IEmpresaRepositorio         Empresas         { get; }
        public ISucursalRepositorio        Sucursales       { get; }
        public IPagoRepositorio            Pagos            { get; }
        public IMetodoPagoRepositorio      MetodosPago      { get; }
        public IAuditoriaRepositorio       Auditoria        { get; }
        public IProveedorProductoCostoRepositorio ProveedoresCostos { get; }
        public IRolRepositorio     Roles    { get; }
        public IPermisoRepositorio Permisos { get; }
        public IDescuentoConfiguracionRepositorio DescuentoConfiguraciones { get; }
        public IMantenimientoLogRepositorio MantenimientoLogs { get; }

        public UnitOfWork(GestionComercialContext context)
        {
            _context         = context;
            Productos        = new ProductoRepositorio(context);
            Clientes         = new ClienteRepositorio(context);
            Proveedores      = new ProveedorRepositorio(context);
            Ventas           = new VentaRepositorio(context);
            Compras          = new CompraRepositorio(context);
            Cajas            = new CajaRepositorio(context);
            MovimientosStock = new MovimientoStockRepositorio(context);
            MovimientosCaja  = new MovimientoCajaRepositorio(context);
            Usuarios         = new UsuarioRepositorio(context);
            Categorias       = new CategoriaRepositorio(context);
            Empresas         = new EmpresaRepositorio(context);
            Sucursales       = new SucursalRepositorio(context);
            Pagos            = new PagoRepositorio(context);
            MetodosPago      = new MetodoPagoRepositorio(context);
            Auditoria        = new AuditoriaRepositorio(context);
            ProveedoresCostos = new ProveedorProductoCostoRepositorio(context);
            Roles            = new RolRepositorio(context);
            Permisos         = new PermisoRepositorio(context);
            DescuentoConfiguraciones = new DescuentoConfiguracionRepositorio(context);
            MantenimientoLogs = new MantenimientoLogRepositorio(context);
        }

        public async Task<int> GuardarCambiosAsync(CancellationToken ct = default)
        {
            var result = await _context.SaveChangesAsync(ct);
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
            return result;
        }

        public async Task EjecutarEnTransaccionAsync(Func<Task> operacion, CancellationToken ct = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                await operacion();
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
