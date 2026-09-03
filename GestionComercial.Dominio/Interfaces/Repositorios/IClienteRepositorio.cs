using GestionComercial.Dominio.Entidades.Cliente;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IClienteRepositorio : IRepositorioBase<Cliente>
    {
        Task<bool> ExisteDocumentoAsync(int documento, int idEmpresa, CancellationToken ct = default);
        Task<bool> ExisteEmailAsync(string email, int idEmpresa, CancellationToken ct = default);
        Task<Cliente?> ObtenerPorDocumentoAsync(int documento, int idEmpresa, CancellationToken ct = default);
        
        // Búsqueda con StartsWith (prefijo) para uso de índices
        Task<List<Cliente>> BuscarPorNombreAsync(string nombre, int idEmpresa, int take = 10, CancellationToken ct = default);
        
        Task<IEnumerable<Cliente>> ObtenerPorEmpresaAsync(int idEmpresa, CancellationToken ct = default);
        Task<(IEnumerable<Cliente> Items, int TotalCount)> ObtenerPorEmpresaPaginadoAsync(
            int idEmpresa, int page, int pageSize, string? searchTerm = null, bool? soloActivos = null, CancellationToken ct = default);
        Task<IEnumerable<Cliente>> ObtenerPorEmpresaYFechaAsync(int idEmpresa, DateTime desde, DateTime hasta, CancellationToken ct = default);
        Task<int> ContarClientesConVentasAsync(int idEmpresa, CancellationToken ct = default);
    }
}
