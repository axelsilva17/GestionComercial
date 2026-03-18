using GestionComercial.Dominio.Entidades.Cliente;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IClienteRepositorio : IRepositorioBase<Cliente>
    {
        Task<bool>                  ExisteDocumentoAsync(int documento, int idEmpresa);
        Task<bool>                  ExisteEmailAsync(string email, int idEmpresa);  // string, no int
        Task<Cliente?>              ObtenerPorDocumentoAsync(int documento, int idEmpresa);
        Task<IEnumerable<Cliente>>  BuscarPorNombreAsync(string nombre, int idEmpresa);
        Task<IEnumerable<Cliente>>  ObtenerPorEmpresaAsync(int idEmpresa);
        Task<IEnumerable<Cliente>>  ObtenerPorEmpresaYFechaAsync(int idEmpresa, DateTime desde, DateTime hasta);
    }
}
