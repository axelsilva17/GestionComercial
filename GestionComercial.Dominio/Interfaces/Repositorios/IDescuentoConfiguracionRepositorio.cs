using GestionComercial.Dominio.Entidades.Descuento;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IDescuentoConfiguracionRepositorio : IRepositorioBase<DescuentoConfiguracion>
    {
        Task<List<DescuentoConfiguracion>> ObtenerVigentesPorEmpresaAsync(int idEmpresa, CancellationToken ct = default);
        Task<List<DescuentoConfiguracion>> ObtenerConMetodosPagoAsync(int idEmpresa, CancellationToken ct = default);
        Task<List<DescuentoConfiguracion>> ObtenerConMetodosPagoPorIdAsync(int id, CancellationToken ct = default);
        Task<List<DescuentoConfiguracion>> BuscarAsync(int idEmpresa, string? texto, bool? activo, CancellationToken ct = default);
        Task ActualizarMetodosPagoAsync(int idDescuento, List<int> idsMetodosPago, CancellationToken ct = default);
    }
}