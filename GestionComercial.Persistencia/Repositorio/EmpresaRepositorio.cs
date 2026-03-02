// ── Empresa ───────────────────────────────────────────────────────────────
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Persistencia.Repositorio;

public class EmpresaRepositorio : RepositorioBase<Empresa>, IEmpresaRepositorio
{
    public EmpresaRepositorio(GestionComercialContext context) : base(context) { }

    public async Task<Empresa?> ObtenerPorCUITAsync(string cuit)
        => await _dbSet.FirstOrDefaultAsync(e => e.CUIT == cuit);
}
