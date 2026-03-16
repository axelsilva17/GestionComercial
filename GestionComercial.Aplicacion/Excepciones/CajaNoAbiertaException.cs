namespace GestionComercial.Aplicacion.Excepciones
{
    public class CajaNoAbiertaException : Exception
    {
        public CajaNoAbiertaException() 
            : base("No hay una caja abierta para esta sucursal.") { }

        public CajaNoAbiertaException(string mensaje) : base(mensaje) { }
    }
}
