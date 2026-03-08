using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    // ── Proveedor ─────────────────────────────────────────────────────────────
    public class ProveedorRepositorio : RepositorioBase<Proveedor>, IProveedorRepositorio
    {
        public ProveedorRepositorio(GestionComercialContext context) : base(context) { }
        public async Task<bool> EstaActivoAsync(int idProveedor)
    => await _dbSet.AnyAsync(p => p.Id == idProveedor && p.Activo);

        public async Task<IEnumerable<Proveedor>> ObtenerPorEmpresaAsync(int idEmpresa)
            => await _dbSet
                .Where(p => p.Id_empresa == idEmpresa && p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
    }
}