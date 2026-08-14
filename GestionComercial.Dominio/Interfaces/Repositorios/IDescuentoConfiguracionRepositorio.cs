using GestionComercial.Dominio.Entidades.Descuento;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    public interface IDescuentoConfiguracionRepositorio : IRepositorioBase<DescuentoConfiguracion>
    {
        Task<List<DescuentoConfiguracion>> ObtenerVigentesPorEmpresaAsync(int idEmpresa);
        Task<List<DescuentoConfiguracion>> ObtenerPorTipoAsync(int idEmpresa, TipoDescuentoEnum tipo);
        Task<List<DescuentoConfiguracion>> BuscarAsync(int idEmpresa, string? texto, TipoDescuentoEnum? tipo, bool? activo);
    }
}
