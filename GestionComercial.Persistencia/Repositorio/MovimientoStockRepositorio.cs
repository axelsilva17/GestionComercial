
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

    public async Task<IEnumerable<MovimientoStock>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta, int idSucursal)
        => await _dbSet
            .Where(m => m.Fecha >= desde && m.Fecha <= hasta && m.Id_sucursal == idSucursal)
            .Include(m => m.Producto)
            .Include(m => m.Usuario)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
}