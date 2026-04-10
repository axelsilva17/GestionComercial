using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Compras;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using System;

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

        public async Task<CompraDto?> ObtenerPorIdAsync(int id)
        {
            var c = await _uow.Compras.ObtenerConDetallesAsync(id);
            return c == null ? null : MapearDto(c);
        }

        public async Task<CompraDto> CrearAsync(CompraCrearDto dto)
        {
            var compra = new Compra
            {
                Fecha        = DateTime.Now,
                Id_proveedor = dto.IdProveedor,
                Id_sucursal  = dto.IdSucursal,
                Id_usuario   = dto.IdUsuario,
                Total        = 0,
                Estado       = 1, // Pendiente
            };

            decimal total = 0;
            foreach (var item in dto.Items)
            {
                var producto = await _uow.Productos.ObtenerPorIdAsync(item.IdProducto)
                    ?? throw new KeyNotFoundException($"Producto {item.IdProducto} no encontrado");

                var subtotal = item.PrecioCosto * item.Cantidad;
                total += subtotal;

                compra.Detalles.Add(new CompraDetalle
                {
                    Id_producto  = item.IdProducto,
                    Cantidad     = item.Cantidad,
                    PrecioCosto  = item.PrecioCosto,
                    Subtotal     = subtotal,
                });

                // Actualizar stock y precio costo
                producto.StockActual      += item.Cantidad;
                producto.PrecioCostoActual = item.PrecioCosto;
                _uow.Productos.Actualizar(producto);
            }

            compra.Total = total;
            await _uow.Compras.AgregarAsync(compra);
            await _uow.GuardarCambiosAsync();

            return await ObtenerPorIdAsync(compra.Id) ?? throw new Exception("Error al crear compra");
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
