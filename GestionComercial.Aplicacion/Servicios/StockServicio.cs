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

            await _uow.MovimientosStock.AgregarAsync(new MovimientoStock
            {
                Id_producto = idProducto,
                Id_sucursal = idSucursal,
                Id_usuario = idUsuario,
                Cantidad = Math.Abs(cantidad),
                TipoMovimiento = cantidad >= 0 ? 1 : 2, // 1=Entrada 3=Ajuste
                Observacion = motivo,
                Fecha = DateTime.Now,
                StockAnterior = stockAnterior,
                StockNuevo = producto.StockActual,
            });

            await _uow.GuardarCambiosAsync();
        }

        public async Task<IEnumerable<MovimientoStock>> ObtenerMovimientosAsync(int idProducto)
            => await _uow.MovimientosStock.ObtenerPorProductoAsync(idProducto);
    }
}