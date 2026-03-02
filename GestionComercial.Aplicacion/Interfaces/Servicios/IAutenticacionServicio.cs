using GestionComercial.Aplicacion.DTOs;
using GestionComercial.Aplicacion.DTOs.Usuarios;


namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    public interface IAutenticacionServicio
    {
        Task<UsuarioSesionDto?> LoginAsync(string email, string password);
        string HashPassword(string password);
    }
}
