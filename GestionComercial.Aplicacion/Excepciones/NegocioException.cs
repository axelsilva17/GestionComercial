namespace GestionComercial.Aplicacion.Excepciones
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensaje) : base(mensaje) { }
    }
}
