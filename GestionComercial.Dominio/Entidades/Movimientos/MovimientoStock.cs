using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Seguridad;

namespace GestionComercial.Dominio.Entidades.Movimientos
{
    public class MovimientoStock
    {
        public int      Id             { get; set; }
        public int      TipoMovimiento { get; set; } // 1=Entrada 2=Salida 3=Ajuste
        public string?  Observacion    { get; set; }
        public decimal  Cantidad       { get; set; }
        public decimal  StockAnterior  { get; set; }
        public decimal  StockNuevo     { get; set; }
        public DateTime Fecha          { get; set; } = DateTime.Now;
        public int?     ReferenciaId   { get; set; }
        public int      Id_sucursal    { get; set; }
        public int      Id_producto    { get; set; }
        public int      Id_usuario     { get; set; }

        public Sucursal Sucursal { get; set; } = null!;
        public Producto.Producto Producto { get; set; } = null!;
        public Usuario  Usuario  { get; set; } = null!;
    }
}
