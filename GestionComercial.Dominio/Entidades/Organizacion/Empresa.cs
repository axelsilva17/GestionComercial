using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Cliente;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Organizacion
{
    public class Empresa : EntidadBase
    {
        public string  Nombre    { get; set; } = string.Empty;
        public string  CUIT      { get; set; } = string.Empty;
        public string  Direccion { get; set; } = string.Empty;
        public string? Email     { get; set; }
        public string? Telefono  { get; set; }

        public ICollection<Sucursal>   Sucursales  { get; set; } = new List<Sucursal>();
        public ICollection<Categoria>  Categorias  { get; set; } = new List<Categoria>();
        public ICollection<Cliente.Cliente>   Clientes    { get; set; } = new List<Cliente.Cliente>();
        public ICollection<Proveedor>  Proveedores { get; set; } = new List<Proveedor>();
        public ICollection<MetodoPago> MetodosPago { get; set; } = new List<MetodoPago>();
        public ICollection<Producto.Producto>  Productos   { get; set; } = new List<Producto.Producto>();
    }
}
