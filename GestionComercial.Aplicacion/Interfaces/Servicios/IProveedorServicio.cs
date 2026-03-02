using GestionComercial.Dominio.Entidades.Proveedores;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IProveedorServicio
    {
        Task<IEnumerable<Proveedor>> ObtenerTodosAsync(int idEmpresa);
        Task<Proveedor?>             ObtenerPorIdAsync(int id);
        Task<Proveedor>              CrearAsync(Proveedor proveedor);
        Task                         ActualizarAsync(Proveedor proveedor);
        Task                         DesactivarAsync(int id);
    }
}
