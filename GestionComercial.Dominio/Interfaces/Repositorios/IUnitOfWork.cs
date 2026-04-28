using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Dominio.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductoRepositorio        Productos        { get; }
        IClienteRepositorio         Clientes         { get; }
        IProveedorRepositorio       Proveedores      { get; }
        IVentaRepostorio            Ventas           { get; }
        ICompraRepositorio          Compras          { get; }
        ICajaRepositorio            Cajas            { get; }
        IMovimientoStockRepositorio MovimientosStock { get; }
        IMovimientoCajaRepositorio  MovimientosCaja  { get; }
        IUsuarioRepositorio         Usuarios         { get; }
        ICategoriaRepositorio      Categorias       { get; }
        IEmpresaRepositorio         Empresas         { get; }
        ISucursalRepositorio        Sucursales       { get; }
        IPagoRepositorio            Pagos            { get; }
        IMetodoPagoRepositorio      MetodosPago      { get; }
        IAuditoriaRepositorio       Auditoria        { get; }
        IProveedorProductoCostoRepositorio ProveedoresCostos { get; }
        Task<int> GuardarCambiosAsync();
        Task      EjecutarEnTransaccionAsync(Func<Task> operacion);
    }
}
