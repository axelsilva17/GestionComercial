using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class VentaServicio : IVentaServicio
    {
        private readonly IUnitOfWork _uow;
        public VentaServicio(IUnitOfWork uow) => _uow = uow;

        // Implementa la interfaz
        public async Task<IEnumerable<VentaResumenDto>> ObtenerPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerPorFechaAsync(desde, hasta, idSucursal);
            return ventas.Select(MapearResumen);
        }

        public async Task<VentaDto?> ObtenerPorIdAsync(int id)
        {
            var v = await _uow.Ventas.ObtenerConDetallesAsync(id);
            return v == null ? null : MapearDto(v);
        }

        public async Task<VentaDto> CrearAsync(VentaCrearDto dto)
        {
            foreach (var item in dto.Items)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(item.IdProducto)
                    ?? throw new ProductoNoEncontradoException(item.IdProducto);
                if (producto.StockActual < item.Cantidad)
                    throw new StockInsuficienteException(producto.Nombre, (int)producto.StockActual, item.Cantidad);
            }

            var venta = new Venta
            {
                Fecha = DateTime.Now,
                Estado = 1,
                Id_sucursal = dto.IdSucursal,
                Id_cliente = dto.IdCliente,
                Id_usuario = dto.IdUsuario,
                Id_caja = dto.IdCaja,
                TotalBruto = 0,
                TotalDescuento = 0,
                TotalFinal = 0,
            };

            decimal totalBruto = 0;

            foreach (var item in dto.Items)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(item.IdProducto)!;
                var subtotal = producto!.PrecioVentaActual * item.Cantidad;
                totalBruto += subtotal;

                venta.Detalles.Add(new VentaDetalle
                {
                    Id_producto = item.IdProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.PrecioVentaActual,
                    CostoUnitario = producto.PrecioCostoActual,
                    Subtotal = subtotal,
                    MargenUnitario = producto.PrecioVentaActual - producto.PrecioCostoActual,
                });

                producto.StockActual -= item.Cantidad;
                _uow.Productos.Actualizar(producto);
            }

            venta.TotalBruto = totalBruto;
            venta.TotalDescuento = dto.TotalDescuento;
            venta.TotalFinal = totalBruto - dto.TotalDescuento;
            venta.Estado = 2;

            await _uow.Ventas.AgregarAsync(venta);
            await _uow.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(venta.Id) ?? throw new NegocioException("Error al crear venta.");
        }

        public async Task CancelarAsync(int id)
        {
            var venta = await _uow.Ventas.ObtenerConDetallesAsync(id)
                ?? throw new VentaInvalidaException($"Venta {id} no encontrada.");

            if (venta.Estado == 3)
                throw new VentaInvalidaException("La venta ya está cancelada.");

            foreach (var detalle in venta.Detalles)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(detalle.Id_producto);
                if (producto != null)
                {
                    producto.StockActual += detalle.Cantidad;
                    _uow.Productos.Actualizar(producto);
                }
            }

            venta.Estado = 3;
            _uow.Ventas.Actualizar(venta);
            await _uow.GuardarCambiosAsync();
        }

        public async Task<decimal> ObtenerTotalDelDiaAsync(int idSucursal)
            => await _uow.Ventas.ObtenerTotalDelDiaAsync(idSucursal);

        // ── Mapeos ────────────────────────────────────────────────────────────
        private static VentaResumenDto MapearResumen(Venta v) => new()
        {
            IdVenta = v.Id,
            Fecha = v.Fecha,
            TotalFinal = v.TotalFinal,
            Estado = v.Estado == 1 ? "Pendiente" : v.Estado == 2 ? "Pagada" : "Anulada",
            ClienteNombre = v.Cliente?.Nombre ?? string.Empty,
            UsuarioNombre = v.Usuario != null ? $"{v.Usuario.Nombre} {v.Usuario.Apellido}" : string.Empty,
        };

        private static VentaDto MapearDto(Venta v) => new()
        {
            IdVenta = v.Id,
            Fecha = v.Fecha,
            TotalBruto = v.TotalBruto,
            TotalDescuento = v.TotalDescuento,
            TotalFinal = v.TotalFinal,
            Estado = v.Estado == 1 ? "Pendiente" : v.Estado == 2 ? "Pagada" : "Anulada",
            ClienteNombre = v.Cliente?.Nombre ?? string.Empty,
            UsuarioNombre = v.Usuario != null ? $"{v.Usuario.Nombre} {v.Usuario.Apellido}" : string.Empty,
            Items = v.Detalles.Select(d => new VentaDetalleDto
            {
                IdProducto = d.Id_producto,
                ProductoNombre = d.Producto?.Nombre ?? string.Empty,
                Cantidad = (int)d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal,
            }).ToList(),
        };
    }
}