namespace GestionComercial.Aplicacion.Excepciones
{
    public class ProductoNoEncontradoException : Exception
    {
        public ProductoNoEncontradoException(int idProducto)
            : base($"Producto con ID {idProducto} no encontrado.") { }

        public ProductoNoEncontradoException(string nombre)
            : base($"Producto '{nombre}' no encontrado.") { }
    }
}
