using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class VentaServicio : IVentaServicio
    {
        private readonly IUnitOfWork _uow;
        public VentaServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<VentaResumenDto>> ObtenerPorSucursalAsync(int idSucursal, DateTime desde, DateTime hasta)
        {
            var ventas = await _uow.Ventas.ObtenerPorSucursalYFechaAsync(idSucursal, desde, hasta);
            return ventas.Select(MapearResumen);
        }

        public async Task<VentaDto?> ObtenerPorIdAsync(int id)
        {
            var v = await _uow.Ventas.ObtenerConDetallesAsync(id);
            if (v == null) return null;
            return MapearDto(v);
        }

        public async Task<VentaDto> CrearAsync(VentaCrearDto dto)
        {
            // Validar stock
            foreach (var item in dto.Items)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(item.IdProducto)
                    ?? throw new ProductoNoEncontradoException(item.IdProducto);
                if (producto.StockActual < item.Cantidad)
                    throw new StockInsuficienteException(producto.Nombre, producto.StockActual, item.Cantidad);
            }

            // Crear venta
            var venta = new Venta
            {
                Fecha           = DateTime.Now,
                Estado          = EstadoVenta.Pendiente,
                Id_sucursal     = dto.IdSucursal,
                Id_cliente      = dto.IdCliente,
                Id_usuario      = dto.IdUsuario,
                TotalBruto      = 0,
                TotalDescuento  = 0,
                TotalFinal      = 0,
            };

            decimal totalBruto = 0;
            decimal totalDescuento = 0;

            foreach (var item in dto.Items)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(item.IdProducto)!;
                var subtotal = producto!.PrecioVentaActual * item.Cantidad;
                totalBruto  += subtotal;

                venta.Detalles.Add(new VentaDetalle
                {
                    Id_producto     = item.IdProducto,
                    Cantidad        = item.Cantidad,
                    PrecioUnitario  = producto.PrecioVentaActual,
                    CostoUnitario   = producto.PrecioCostoActual,
                    Subtotal        = subtotal,
                    MargenUnitario  = producto.PrecioVentaActual - producto.PrecioCostoActual,
                });

                // Descontar stock
                producto.StockActual -= item.Cantidad;
                _uow.Productos.Actualizar(producto);
            }

            venta.TotalBruto     = totalBruto;
            venta.TotalDescuento = totalDescuento;
            venta.TotalFinal     = totalBruto - totalDescuento;

            await _uow.Ventas.AgregarAsync(venta);
            await _uow.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(venta.Id) ?? throw new Exception("Error al crear venta");
        }

        public async Task CancelarAsync(int id)
        {
            var venta = await _uow.Ventas.ObtenerConDetallesAsync(id)
                ?? throw new VentaInvalidaException($"Venta {id} no encontrada");

            if (venta.Estado == EstadoVenta.Cancelada)
                throw new VentaInvalidaException("La venta ya está cancelada");

            // Devolver stock
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(detalle.Id_producto);
                if (producto != null)
                {
                    producto.StockActual += detalle.Cantidad;
                    _uow.Productos.Actualizar(producto);
                }
            }

            venta.Estado = EstadoVenta.Cancelada;
            _uow.Ventas.Actualizar(venta);
            await _uow.GuardarCambiosAsync();
        }

        private static VentaResumenDto MapearResumen(Venta v) => new()
        {
            IdVenta        = v.Id,
            Fecha          = v.Fecha,
            TotalFinal     = v.TotalFinal,
            Estado         = v.Estado.ToString(),
            ClienteNombre  = $"{v.Cliente?.Nombre} {v.Cliente?.Apellido}".Trim(),
            ClienteInicial = v.Cliente?.Nombre?.FirstOrDefault().ToString() ?? "?",
            UsuarioNombre  = $"{v.Usuario?.Nombre} {v.Usuario?.Apellido}".Trim(),
        };

        private static VentaDto MapearDto(Venta v) => new()
        {
            IdVenta        = v.Id,
            Fecha          = v.Fecha,
            TotalBruto     = v.TotalBruto,
            TotalDescuento = v.TotalDescuento,
            TotalFinal     = v.TotalFinal,
            Estado         = v.Estado.ToString(),
            ClienteNombre  = $"{v.Cliente?.Nombre} {v.Cliente?.Apellido}".Trim(),
            UsuarioNombre  = $"{v.Usuario?.Nombre} {v.Usuario?.Apellido}".Trim(),
            Items          = v.Detalles.Select(d => new VentaDetalleDto
            {
                IdProducto     = d.Id_producto,
                NombreProducto = d.Producto?.Nombre ?? string.Empty,
                Cantidad       = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal       = d.Subtotal,
            }).ToList(),
        };
    }
}
