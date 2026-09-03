using GestionComercial.Dominio.Entidades.Mantenimiento;
using GestionComercial.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.Dominio.Interfaces.Servicios
{
    public interface IMantenimientoLogRepositorio : IRepositorioBase<MantenimientoLog>
    {
        Task<List<MantenimientoLog>> ObtenerRecientesAsync(int cantidad = 50);
    }
}
