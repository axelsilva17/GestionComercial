namespace GestionComercial.Aplicacion.Excepciones
{
    public class AutenticacionException : Exception
    {
        public AutenticacionException(string mensaje = "Credenciales inválidas.") 
            : base(mensaje) { }
    }
}
