namespace GestionComercial.Aplicacion.Excepciones
{
    public class VentaInvalidaException : Exception
    {
        public VentaInvalidaException(string mensaje) : base(mensaje) { }
    }
}
