using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestionComercial.Persistencia.Repositorio
{
    public class MetodoPagoRepositorio : RepositorioBase<MetodoPago>, IMetodoPagoRepositorio
    {
        public MetodoPagoRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<MetodoPago>> ObtenerTodosPorEmpresaAsync(int idEmpresa, CancellationToken ct = default)
            => await _context.MetodosPago.AsNoTracking()
                .Where(m => m.Id_empresa == idEmpresa)
                .OrderBy(m => m.Nombre)
                .ToListAsync(ct);
    }
}
