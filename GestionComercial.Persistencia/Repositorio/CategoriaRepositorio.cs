// ── Categoria ─────────────────────────────────────────────────────────────
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Persistencia.Repositorio;

public class CategoriaRepositorio : RepositorioBase<Categoria>, ICateogoriaRepositorio
{
    public CategoriaRepositorio(GestionComercialContext context) : base(context) { }

    public async Task<IEnumerable<Categoria>> ObtenerPorEmpresaAsync(int idEmpresa)
        => await _dbSet
            .Where(c => c.Id_empresa == idEmpresa && c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

    public async Task<IEnumerable<Categoria>> ObtenerRaicesAsync(int idEmpresa)
        => await _dbSet
            .Where(c => c.Id_empresa == idEmpresa && c.CategoriaPadre_id == null && c.Activo)
            .Include(c => c.SubCategorias)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
}