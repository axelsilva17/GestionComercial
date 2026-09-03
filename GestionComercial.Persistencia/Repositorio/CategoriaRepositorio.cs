namespace GestionComercial.Persistencia.Repositorio
{
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

public class CategoriaRepositorio : RepositorioBase<Categoria>, ICategoriaRepositorio
{
    public CategoriaRepositorio(GestionComercialContext context) : base(context) { }

    public async Task<IEnumerable<Categoria>> ObtenerPorEmpresaAsync(int idEmpresa, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(c => c.Id_empresa == idEmpresa && c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync(ct);

    public async Task<IEnumerable<Categoria>> ObtenerRaicesAsync(int idEmpresa, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(c => c.Id_empresa == idEmpresa && c.CategoriaPadre_id == null && c.Activo)
            .Include(c => c.SubCategorias)
            .OrderBy(c => c.Nombre)
            .ToListAsync(ct);

    // ── Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion ──

    public async Task<List<Categoria>> ObtenerSubCategoriasAsync(int idCategoria, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(c => c.CategoriaPadre_id == idCategoria)
            .ToListAsync(ct);

    public async Task<Categoria?> ObtenerPorNombreAsync(string nombre, int idEmpresa, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Nombre == nombre
                                   && c.Id_empresa == idEmpresa
                                   && c.Activo, ct);
}
}