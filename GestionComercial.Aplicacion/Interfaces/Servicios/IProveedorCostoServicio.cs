using System.Threading.Tasks;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IProveedorCostoServicio
    {
        Task<(int Nuevos, int Actualizados)> AjusteCostoProveedorAsync(int idProveedor, decimal porcentaje);
    }
}
