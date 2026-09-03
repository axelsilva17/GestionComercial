using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Entidades.Ventas;
using System.Threading;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
   
    public interface ICategoriaRepositorio : IRepositorioBase<Categoria>
    {
        Task<IEnumerable<Categoria>> ObtenerPorEmpresaAsync(int idEmpresa, CancellationToken ct = default);
        Task<IEnumerable<Categoria>> ObtenerRaicesAsync(int idEmpresa, CancellationToken ct = default);

        // Nuevos métodos para eliminar dependencias EF Core de la capa Aplicacion
        Task<List<Categoria>> ObtenerSubCategoriasAsync(int idCategoria, CancellationToken ct = default);
        Task<Categoria?> ObtenerPorNombreAsync(string nombre, int idEmpresa, CancellationToken ct = default);
    }


}