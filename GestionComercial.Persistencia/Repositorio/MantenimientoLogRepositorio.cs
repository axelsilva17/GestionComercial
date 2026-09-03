using GestionComercial.Dominio.Entidades.Mantenimiento;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class MantenimientoLogRepositorio : RepositorioBase<MantenimientoLog>, IMantenimientoLogRepositorio
    {
        public MantenimientoLogRepositorio(GestionComercialContext context) : base(context)
        {
        }

        public async Task<List<MantenimientoLog>> ObtenerRecientesAsync(int cantidad = 50)
        {
            return await _dbSet
                .AsNoTracking()
                .OrderByDescending(l => l.FechaAlta)
                .Take(cantidad)
                .ToListAsync();
        }
    }
}
