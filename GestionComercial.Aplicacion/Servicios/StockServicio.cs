using GestionComercial.Aplicacion.DTOs.Productos;
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
                IdProducto = producto.Id,
                ProductoNombre = producto.Nombre,
                IdSucursal = 0,
                SucursalNombre = string.Empty,
                StockActual = (int)producto.StockActual,
                StockMinimo = (int)producto.StockMinimo,
            };
        }

        public async Task AjustarStockAsync(int idProducto, int cantidad, string motivo, int idSucursal, int idUsuario)
        {
            var producto = await _uow.Productos.ObtenerPorIdAsync(idProducto)
                ?? throw new KeyNotFoundException($"Producto {idProducto} no encontrado");

            var stockAnterior = producto.StockActual;
            producto.StockActual += cantidad;
            _uow.Productos.Actualizar(producto);

            // ── Usar factory method DDD según el tipo de movimiento ────────
            var movimiento = cantidad >= 0
                ? MovimientoStock.Entrada(Math.Abs(cantidad), stockAnterior, idProducto, idSucursal, idUsuario, motivo)
                : MovimientoStock.Ajuste(producto.StockActual, stockAnterior, idProducto, idSucursal, idUsuario, motivo);

            await _uow.MovimientosStock.AgregarAsync(movimiento);

            await _uow.GuardarCambiosAsync();
        }

        public async Task<IEnumerable<MovimientoStock>> ObtenerMovimientosAsync(int idProducto)
            => await _uow.MovimientosStock.ObtenerPorProductoAsync(idProducto);
    }
}