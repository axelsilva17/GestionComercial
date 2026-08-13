using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class CompraServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<ICompraRepositorio> _mockCompraRepo = new();
        private readonly Mock<IProductoRepositorio> _mockProductoRepo = new();
        private readonly Mock<IInventarioServicio> _mockInventario = new();
        private readonly CompraServicio _servicio;

        public CompraServicioTests()
        {
            _mockUow.Setup(u => u.Compras).Returns(_mockCompraRepo.Object);
            _mockUow.Setup(u => u.Productos).Returns(_mockProductoRepo.Object);

            _servicio = new CompraServicio(_mockUow.Object, _mockInventario.Object);
        }

        [Fact]
        public async Task CrearAsync_ConItemsValidos_CreaCompra()
        {
            var producto = new Producto
            {
                Id = 1,
                Nombre = "Producto Test",
                StockActual = 10,
                PrecioVentaActual = 100m,
                PrecioCostoActual = 50m
            };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);

            _mockCompraRepo
                .Setup(r => r.AgregarAsync(It.IsAny<GestionComercial.Dominio.Entidades.Compras.Compra>()))
                .Returns<GestionComercial.Dominio.Entidades.Compras.Compra>(c => Task.FromResult(c));

            _mockCompraRepo
                .Setup(r => r.ObtenerConDetallesAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var c = GestionComercial.Dominio.Entidades.Compras.Compra.Crear(1, 1, 1);
                    c.GetType().GetProperty("Id")!.SetValue(c, id);
                    return c;
                });

            var dto = new CompraCrearDto
            {
                IdProveedor = 1,
                IdSucursal = 1,
                IdUsuario = 1,
                Items = new List<CompraDetalleCrearDto>
                {
                    new() { IdProducto = 1, Cantidad = 5, PrecioCosto = 40m }
                }
            };

            var resultado = await _servicio.CrearAsync(dto);

            resultado.Should().NotBeNull();
        }

        [Fact]
        public async Task CrearAsync_PasaUnidadTrabajoCompartida_AInventarioServicio()
        {
            var producto = new Producto
            {
                Id = 1,
                Nombre = "Producto Test",
                StockActual = 10,
                PrecioVentaActual = 100m,
                PrecioCostoActual = 50m
            };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);

            _mockCompraRepo
                .Setup(r => r.AgregarAsync(It.IsAny<GestionComercial.Dominio.Entidades.Compras.Compra>()))
                .Returns<GestionComercial.Dominio.Entidades.Compras.Compra>(c => Task.FromResult(c));

            _mockCompraRepo
                .Setup(r => r.ObtenerConDetallesAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var c = GestionComercial.Dominio.Entidades.Compras.Compra.Crear(1, 1, 1);
                    c.GetType().GetProperty("Id")!.SetValue(c, id);
                    return c;
                });

            var dto = new CompraCrearDto
            {
                IdProveedor = 1,
                IdSucursal = 1,
                IdUsuario = 1,
                Items = new List<CompraDetalleCrearDto>
                {
                    new() { IdProducto = 1, Cantidad = 5, PrecioCosto = 40m }
                }
            };

            await _servicio.CrearAsync(dto);

            // Verificar que se pasó una unidad de trabajo (no null) para que el movimiento
            // y la compra se persistan en el mismo contexto
            _mockInventario.Verify(i => i.RegistrarMovimientoAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                false,
                It.Is<IUnitOfWork?>(u => u != null)), Times.AtLeastOnce);
        }
    }
}
