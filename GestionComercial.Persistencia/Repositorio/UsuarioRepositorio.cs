using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
            => 
            await _dbSet
                .Include(u => u.Rol)
                .Include(u => u.Sucursal)
                    .ThenInclude(s => s.Empresa)
                .FirstOrDefaultAsync(u => u.Email == email && u.Activo);

        public async Task<IEnumerable<Usuario>> ObtenerPorSucursalAsync(int idSucursal)
            => await _dbSet
                .Where(u => u.Id_sucursal == idSucursal && u.Activo)
                .Include(u => u.Rol)
                .OrderBy(u => u.Apellido)
                .ToListAsync();
    }
}
