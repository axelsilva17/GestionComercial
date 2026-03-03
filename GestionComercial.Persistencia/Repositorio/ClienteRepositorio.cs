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

        public async Task<Cliente?> ObtenerPorDocumentoAsync(string documento, int idEmpresa)
            => await _dbSet
                .FirstOrDefaultAsync(c => c.Documento == documento && c.Id_empresa == idEmpresa);

        public async Task<IEnumerable<Cliente>> BuscarPorNombreAsync(string nombre, int idEmpresa)
            => await _dbSet
                .Where(c => c.Nombre.Contains(nombre) && c.Id_empresa == idEmpresa)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
    }
}