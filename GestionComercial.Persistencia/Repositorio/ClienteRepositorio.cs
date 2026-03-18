using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class ClienteRepositorio : RepositorioBase<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<Cliente>> ObtenerPorEmpresaAsync(int idEmpresa)
            => await _dbSet
                .Where(c => c.Id_empresa == idEmpresa)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

        public async Task<Cliente?> ObtenerPorDocumentoAsync(int documento, int idEmpresa)
            => await _dbSet
                .FirstOrDefaultAsync(c => c.Documento == documento && c.Id_empresa == idEmpresa);

        public async Task<bool> ExisteDocumentoAsync(int documento, int idEmpresa)
            => await _dbSet.AnyAsync(c => c.Documento == documento && c.Id_empresa == idEmpresa);

        public async Task<bool> ExisteEmailAsync(string email, int idEmpresa)
            => await _dbSet.AnyAsync(c => c.Email == email && c.Id_empresa == idEmpresa);

        public async Task<IEnumerable<Cliente>> BuscarPorNombreAsync(string nombre, int idEmpresa)
            => await _dbSet
                .Where(c => c.Nombre.Contains(nombre) && c.Id_empresa == idEmpresa)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

        public async Task<IEnumerable<Cliente>> ObtenerPorEmpresaYFechaAsync(int idEmpresa, DateTime desde, DateTime hasta)
            => await _dbSet
                .Where(c => c.Id_empresa == idEmpresa && c.FechaAlta >= desde && c.FechaAlta <= hasta)
                .OrderByDescending(c => c.FechaAlta)
                .ToListAsync();
    }
}
