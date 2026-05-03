
// ── MovimientoStock ───────────────────────────────────────────────────────
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
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
}