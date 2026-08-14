namespace GestionComercial.Dominio.Entidades.Descuento
{
    public enum TipoDescuentoEnum
    {
        Producto = 1,
        Categoria = 2,
        [Obsolete("Implementar en futura iteración")]
        MetodoPago = 99
    }
}
