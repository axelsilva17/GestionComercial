namespace GestionComercial.Aplicacion.Interfaces.Autenticacion
{
    // Nueva interfaz de compatibilidad: alias a IAuthService para evitar roturas
    public interface IAutenticacionServicio
    {
        bool IsCurrentUserAdmin();
    }
}
