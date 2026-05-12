using System.Threading.Tasks;

namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IConfirmService
    {
        Task<bool> ConfirmAsync(string title, string message);
    }
}