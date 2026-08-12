using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class RolRepositorio : RepositorioBase<Rol>, IRolRepositorio
    {
        public RolRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<Rol>> ObtenerTodosConPermisosAsync()
            => await _dbSet
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                .OrderBy(r => r.Nombre)
                .ToListAsync();

        public async Task ActualizarPermisosRolAsync(int rolId, List<int> permisoIds)
        {
            // Eliminar en lote sin cargar entidades (ExecuteDelete EF Core 7+)
            await _context.Set<RolPermiso>()
                .Where(rp => rp.Id_rol == rolId)
                .ExecuteDeleteAsync();

            // Insertar los nuevos
            var nuevos = permisoIds.Select(pid => new RolPermiso
            {
                Id_rol = rolId,
                Id_permiso = pid,
            }).ToList();

            await _context.Set<RolPermiso>().AddRangeAsync(nuevos);
            await _context.SaveChangesAsync();
        }
    }
}
