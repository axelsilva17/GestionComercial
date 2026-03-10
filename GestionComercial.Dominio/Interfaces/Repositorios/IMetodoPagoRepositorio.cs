using GestionComercial.Dominio.Entidades.Pagos;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IMetodoPagoRepositorio : IRepositorioBase<MetodoPago>
    {
        Task<IEnumerable<MetodoPago>> ObtenerTodosPorEmpresaAsync(int idEmpresa);
    }
}
