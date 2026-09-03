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
        private readonly IInventarioServicio _inventarioServicio;
        private readonly SesionServicio _sesion;

        public CompraServicio(IUnitOfWork uow, IInventarioServicio inventarioServicio, SesionServicio sesion)
        {
            _uow = uow;
            _inventarioServicio = inventarioServicio;
            _sesion = sesion;
        }

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
            if (!_sesion.HasPermission("Compras.Crear"))
                throw new KeyNotFoundException("No tenés permiso para crear compras.");

            // Batch fetch: traer todos los productos de una sola vez (fuera de transacción — solo lectura)
            var idsProductos = dto.Items.Select(i => i.IdProducto).Distinct().ToList();
            var productos = await _uow.Productos.BuscarAsync(p => idsProductos.Contains(p.Id));
            var productosDict = productos.ToDictionary(p => p.Id);

            Compra compra = null!;

            await _uow.EjecutarEnTransaccionAsync(async () =>
            {
                // ── Crear la compra con factory method (DDD) ──
                compra = Compra.Crear(
                    idProveedor: dto.IdProveedor,
                    idSucursal: dto.IdSucursal,
                    idUsuario: dto.IdUsuario,
                    observacion: dto.Observacion
                );

                // ── Agregar detalles con factory methods (DDD) ──
                foreach (var item in dto.Items)
                {
                    if (!productosDict.TryGetValue(item.IdProducto, out var producto))
                        throw new KeyNotFoundException($"Producto {item.IdProducto} no encontrado");

                    // Factory method: CompraDetalle.Crear() calcula el subtotal SOLO
                    var detalle = CompraDetalle.Crear(producto, item.Cantidad, item.PrecioCosto);

                    // Agregar a la compra — Compra recalcula el total automáticamente
                    compra.AgregarDetalle(detalle);

                    // Actualizar precio de costo del producto
                    producto.PrecioCostoActual = item.PrecioCosto;
                    _uow.Productos.Actualizar(producto);

                    // Registrar movimiento de stock (Entrada por compra)
                    // Esto también actualiza el stock del producto
                    await _inventarioServicio.RegistrarMovimientoAsync(
                        item.IdProducto,
                        "Entrada",
                        item.Cantidad,
                        $"Compra #{compra.Id} - {producto.Nombre}",
                        dto.IdSucursal,
                        dto.IdUsuario,
                        guardarCambios: false,
                        unidadTrabajo: _uow
                    );
                }

                await _uow.Compras.AgregarAsync(compra);
            });

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