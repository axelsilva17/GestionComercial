using GestionComercial.Dominio.Entidades.Pagos;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IMetodoPagoRepositorio : IRepositorioBase<MetodoPago>
    {
        Task<IEnumerable<MetodoPago>> ObtenerTodosPorEmpresaAsync(int idEmpresa, CancellationToken ct = default);
    }
}
