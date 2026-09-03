using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionComercial.Aplicacion.Servicios
{
    public interface IActualizacionServicio
    {
        string ObtenerVersionActual();
        string? BuscarActualizacionEnRuta(string ruta);
        Task<bool> AplicarActualizacionAsync(string rutaArchivo);
        List<string> ObtenerCambios(string rutaArchivo);
    }
}
