using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class CompraServicio : ICompraServicio
    {
        private readonly IUnitOfWork _uow;
        public CompraServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<CompraDto>> ObtenerPorSucursalAsync(int idSucursal)
        {
            var compras = await _uow.Compras.ObtenerPorSucursalAsync(idSucursal);
            return compras.Select(MapearDto);
        }

        public async Task<IEnumerable<CompraDto>> ObtenerPorPeriodoAsync(int idSucursal, DateTime desde, DateTime hasta)
        {
            var compras = await _uow.Compras.ObtenerPorPeriodoAsync(idSucursal, desde, hasta);
            return compras.Select(MapearDto);
        }

        public async Task<IEnumerable<CompraDto>> ObtenerPorProveedorAsync(int idProveedor)
        {
            var compras = await _uow.Compras.ObtenerPorProveedorAsync(idProveedor);
            return compras.Select(MapearDto);
        }

        public async Task<CompraDto?> ObtenerPorIdAsync(int id)
        {
            var c = await _uow.Compras.ObtenerConDetallesAsync(id);
            return c == null ? null : MapearDto(c);
        }

        public async Task<CompraDto> CrearAsync(CompraCrearDto dto)
        {
            // ── Crear la compra con factory method (DDD) ──
            var compra = Compra.Crear(
                idProveedor: dto.IdProveedor,
                idSucursal: dto.IdSucursal,
                idUsuario: dto.IdUsuario,
                observacion: dto.Observacion
            );

            // ── Agregar detalles con factory methods (DDD) ──
            foreach (var item in dto.Items)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(item.IdProducto)
                    ?? throw new KeyNotFoundException($"Producto {item.IdProducto} no encontrado");

                // Factory method: CompraDetalle.Crear() calcula el subtotal SOLO
                var detalle = CompraDetalle.Crear(producto, item.Cantidad, item.PrecioCosto);
                
                // Agregar a la compra — Compra recalcula el total automáticamente
                compra.AgregarDetalle(detalle);

                // Actualizar producto: precio de costo y stock
                producto.PrecioCostoActual = item.PrecioCosto;
                producto.AgregarStock(item.Cantidad);
                _uow.Productos.Actualizar(producto);
            }

            // ── Persistir ──
            await _uow.Compras.AgregarAsync(compra);
            await _uow.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(compra.Id) 
                ?? throw new InvalidOperationException("Error al crear la compra");
        }

        private static CompraDto MapearDto(Compra c) => new()
        {
            IdCompra        = c.Id,
            Fecha           = c.Fecha,
            Total           = c.Total,
            Estado          = c.Estado,
            Id_proveedor    = c.Id_proveedor,
            ProveedorNombre = c.Proveedor?.Nombre ?? string.Empty,
            Items           = c.Detalles.Select(d => new CompraDetalleDto
            {
                IdProducto = d.Id_producto,
                Id_proveedor = c.Id_proveedor,
                Cantidad    = (int)d.Cantidad,
                PrecioCosto = d.PrecioCosto,
                SubTotal    = d.Subtotal,
            }).ToList(),
        };
    }
}