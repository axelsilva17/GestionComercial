using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Repositorio
{
    public class MetodoPagoRepositorio : RepositorioBase<MetodoPago>, IMetodoPagoRepositorio
    {
        public MetodoPagoRepositorio(GestionComercialContext context) : base(context) { }

        public async Task<IEnumerable<MetodoPago>> ObtenerTodosPorEmpresaAsync(int idEmpresa)
            => await _context.MetodosPago
                .Where(m => m.Id_empresa == idEmpresa)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
    }
}
