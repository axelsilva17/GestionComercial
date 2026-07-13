using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Persistencia.Repositorio;

public class MovimientoStockRepositorio : RepositorioBase<MovimientoStock>, IMovimientoStockRepositorio
{
    public MovimientoStockRepositorio(GestionComercialContext context) : base(context) { }

    public async Task<IEnumerable<MovimientoStock>> ObtenerPorProductoAsync(int idProducto)
        => await _dbSet
            .Where(m => m.Id_producto == idProducto)
            .Include(m => m.Usuario)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();

    public async Task<IEnumerable<MovimientoStock>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int? idSucursal = null)
        {
            var query = _dbSet
                .Include(m => m.Producto)
                    .ThenInclude(p => p!.Categoria)
                .Include(m => m.Usuario)
                .Include(m => m.Sucursal)
                .Where(m => m.Fecha >= desde && m.Fecha <= hasta);

            if (idSucursal.HasValue)
                query = query.Where(m => m.Id_sucursal == idSucursal.Value);

            return await query
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }

    // ── Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion ──

    public async Task<(List<MovimientoStock> Items, int Total)> ObtenerPaginadoAsync(
        string? textoBusqueda,
        string? filtroTipo,
        string? filtroUsuario,
        string? filtroSucursal,
        DateTime fechaDesde,
        DateTime fechaHasta,
        int pagina,
        int itemsPorPagina)
    {
        var query = _dbSet
            .Include(m => m.Producto)
                .ThenInclude(p => p!.Categoria)
            .Include(m => m.Usuario)
            .Include(m => m.Sucursal)
            .Where(m => m.Fecha >= fechaDesde && m.Fecha <= fechaHasta.AddDays(1));

        // Filtrar por tipo
        if (!string.IsNullOrWhiteSpace(filtroTipo) && filtroTipo != "Todos")
        {
            var tipoEnum = Enum.Parse<TipoMovimientoStockEnum>(filtroTipo, ignoreCase: true);
            query = query.Where(m => m.TipoMovimiento == (int)tipoEnum);
        }

        // Filtrar por usuario
        if (!string.IsNullOrWhiteSpace(filtroUsuario) && filtroUsuario != "Todos")
        {
            var termino = filtroUsuario.ToLower();
            query = query.Where(m =>
                m.Usuario != null &&
                (m.Usuario.Nombre.ToLower().Contains(termino) ||
                 m.Usuario.Apellido.ToLower().Contains(termino)));
        }

        // Filtrar por búsqueda (producto o código)
        if (!string.IsNullOrWhiteSpace(textoBusqueda))
        {
            var termino = textoBusqueda.ToLower();
            query = query.Where(m =>
                (m.Producto != null && m.Producto.Nombre.ToLower().Contains(termino)) ||
                (m.Producto != null && m.Producto.CodigoBarra != null && m.Producto.CodigoBarra.ToLower().Contains(termino)));
        }

        // Filtrar por sucursal
        if (!string.IsNullOrWhiteSpace(filtroSucursal) && filtroSucursal != "Todas")
        {
            query = query.Where(m => m.Sucursal != null && m.Sucursal.Nombre == filtroSucursal);
        }

        // Ordenar
        query = query.OrderByDescending(m => m.Fecha);

        // Total (COUNT en SQL)
        var total = await query.CountAsync();

        // Página (Skip/Take en SQL)
        var items = await query
            .Skip((pagina - 1) * itemsPorPagina)
            .Take(itemsPorPagina)
            .ToListAsync();

        return (items, total);
    }
}