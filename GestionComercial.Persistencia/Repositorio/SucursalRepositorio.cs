// ── Sucursal ──────────────────────────────────────────────────────────────
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Persistencia.Repositorio;

public class SucursalRepositorio : RepositorioBase<Sucursal>, ISucursalRepositorio
{
    public SucursalRepositorio(GestionComercialContext context) : base(context) { }

    public async Task<IEnumerable<Sucursal>> ObtenerPorEmpresaAsync(int idEmpresa)
        => await _dbSet
            .Where(s => s.Id_empresa == idEmpresa && s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
}
