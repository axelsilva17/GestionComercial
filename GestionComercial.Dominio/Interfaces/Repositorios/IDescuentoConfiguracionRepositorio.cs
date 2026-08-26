using GestionComercial.Dominio.Entidades.Descuento;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IDescuentoConfiguracionRepositorio : IRepositorioBase<DescuentoConfiguracion>
    {
        Task<List<DescuentoConfiguracion>> ObtenerVigentesPorEmpresaAsync(int idEmpresa);
        Task<List<DescuentoConfiguracion>> ObtenerConMetodosPagoAsync(int idEmpresa);
        Task<List<DescuentoConfiguracion>> ObtenerConMetodosPagoPorIdAsync(int id);
        Task<List<DescuentoConfiguracion>> BuscarAsync(int idEmpresa, string? texto, bool? activo);
        Task ActualizarMetodosPagoAsync(int idDescuento, List<int> idsMetodosPago);
    }
}