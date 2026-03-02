using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Persistencia.Repositorio;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Dominio.Interfaces.Repositorios;

namespace GestionComercial.Persistencia.Repositorio
{
    public class ClienteRepositorio : RepositorioBase<Cliente>
    {
        public ClienteRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<Cliente?> ObtenerPorDocumentoAsync(string documento, int idEmpresa)
            => await _dbSet.FirstOrDefaultAsync(c => c.Documento == documento && c.Id_empresa == idEmpresa);

        public async Task<IEnumerable<Cliente>> BuscarPorNombreAsync(string nombre, int idEmpresa)
            => await _dbSet
                .Where(c => c.Nombre.Contains(nombre) && c.Id_empresa == idEmpresa && c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
    }
}
