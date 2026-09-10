using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class ProveedorRepositorio : RepositorioBase<Proveedor>, IProveedorRepositorio
    {
        public ProveedorRepositorio(GestionComercialContext context) : base(context) { }
        public async Task<bool> EstaActivoAsync(int idProveedor)
    => await _dbSet.AnyAsync(p => p.Id == idProveedor && p.Activo);

        public async Task<IEnumerable<Proveedor>> ObtenerPorEmpresaAsync(int idEmpresa)
        {
            // Include(Compras) materializa la colección para poder contar las compras
            // reales de cada proveedor de una sola consulta (evita N+1). La lista de
            // proveedores de un POS es acotada, por lo que el costo es aceptable.
            return await _dbSet.AsNoTracking()
                .Where(p => p.Id_empresa == idEmpresa)
                .Include(p => p.Compras)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }
    }
}