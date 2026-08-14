using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class DescuentoConfiguracionRepositorio : RepositorioBase<DescuentoConfiguracion>, IDescuentoConfiguracionRepositorio
    {
        public DescuentoConfiguracionRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<List<DescuentoConfiguracion>> ObtenerVigentesPorEmpresaAsync(int idEmpresa)
        {
            return await _dbSet
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.EstaVigente)
                .ToListAsync();
        }

        public async Task<List<DescuentoConfiguracion>> ObtenerPorTipoAsync(int idEmpresa, TipoDescuentoEnum tipo)
        {
            return await _dbSet
                .Where(d => d.Id_empresa == idEmpresa && d.Activo && d.Tipo == tipo)
                .ToListAsync();
        }

        public async Task<List<DescuentoConfiguracion>> BuscarAsync(int idEmpresa, string? texto, TipoDescuentoEnum? tipo, bool? activo)
        {
            var query = _dbSet
                .Where(d => d.Id_empresa == idEmpresa);

            if (!string.IsNullOrWhiteSpace(texto))
                query = query.Where(d => d.Nombre.Contains(texto));

            if (tipo.HasValue)
                query = query.Where(d => d.Tipo == tipo.Value);

            if (activo.HasValue)
                query = query.Where(d => d.Activo == activo.Value);

            return await query.OrderBy(d => d.Nombre).ToListAsync();
        }
    }
}
