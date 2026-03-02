using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Interfaces;


namespace GestionComercial.Aplicacion.Servicios
{
    public class StockServicio : IStockServicio
    {
        private readonly IUnitOfWork _uow;
        public StockServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<StockDto?> ObtenerStockAsync(int idProducto)
        {
            var producto = await _uow.Productos.ObtenerPorIdAsync(idProducto);
            if (producto == null) return null;
            return new StockDto
            {
                IdProducto  = producto.Id,
                ProductoNombre      = producto.Nombre,
                StockActual = (int)producto.StockActual,
                StockMinimo = (int)producto.StockMinimo,
            };
        }

        public async Task AjustarStockAsync(int idProducto, int cantidad, string motivo, int idSucursal, int idUsuario)
        {
            var producto = await _uow.Productos.ObtenerPorIdAsync(idProducto)
                ?? throw new KeyNotFoundException($"Producto {idProducto} no encontrado");

            var tipo = cantidad >= 0 ? TipoMovimiento.AjustePositivo : TipoMovimiento.AjusteNegativo;

            producto.StockActual += cantidad;
            _uow.Productos.Actualizar(producto);

            await _uow.MovimientosStock.AgregarAsync(new MovimientoStock
            {
                Id_producto  = idProducto,
                Id_sucursal  = idSucursal,
                Id_usuario   = idUsuario,
                Cantidad     = Math.Abs(cantidad),
                TipoMovimiento         = tipo,
                Observacion  = motivo,
                Fecha        = DateTime.Now,
            });

            await _uow.GuardarCambiosAsync();
        }

        public async Task<IEnumerable<MovimientoStock>> ObtenerMovimientosAsync(int idProducto)
            => await _uow.MovimientosStock.ObtenerPorProductoAsync(idProducto);
    }
}
