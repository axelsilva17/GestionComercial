namespace GestionComercial.Aplicacion.Excepciones
{
    public class StockInsuficienteException : Exception
    {
        public StockInsuficienteException(string nombreProducto, int stockActual, int cantidadSolicitada)
            : base($"Stock insuficiente para '{nombreProducto}'. Stock actual: {stockActual}, solicitado: {cantidadSolicitada}.") { }
    }
}
