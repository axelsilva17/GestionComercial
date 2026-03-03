using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using GestionComercial.Persistencia.Repositorio;

namespace GestionComercial.Dominio.Repositorio
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GestionComercialContext _context;

        public IProductoRepositorio          Productos        { get; }
        public IClienteRepositorio           Clientes         { get; }
        public IProveedorRepositorio         Proveedores      { get; }
        public IVentaRepostorio              Ventas           { get; }
        public ICompraRepositorio            Compras          { get; }
        public ICajaRepositorio              Cajas            { get; }
        public IMovimientoStockRepositorio   MovimientosStock { get; }
        public IMovimientoCajaRepositorio    MovimientosCaja  { get; }
        public IUsuarioRepositorio           Usuarios         { get; }
        public ICateogoriaRepositorio        Categorias       { get; }
        public IEmpresaRepositorio           Empresas         { get; }
        public ISucursalRepositorio          Sucursales       { get; }

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
        }

        public async Task<int> GuardarCambiosAsync()
            => await _context.SaveChangesAsync();

        public async Task EjecutarEnTransaccionAsync(Func<Task> operacion)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await operacion();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
