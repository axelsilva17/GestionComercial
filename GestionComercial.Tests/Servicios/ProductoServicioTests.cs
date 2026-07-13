using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class ProductoServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IProductoRepositorio> _mockProductoRepo = new();
        private readonly ProductoServicio _servicio;

        public ProductoServicioTests()
        {
            _mockUow.Setup(u => u.Productos).Returns(_mockProductoRepo.Object);
            _servicio = new ProductoServicio(_mockUow.Object);
        }

        [Fact]
        public async Task ObtenerTodosAsync_ConProductos_DevuelveListado()
        {
            var productos = new List<Producto>
            {
                new() { Id = 1, Nombre = "Prod A", StockActual = 10, StockMinimo = 5, PrecioVentaActual = 100, Id_empresa = 1 },
                new() { Id = 2, Nombre = "Prod B", StockActual = 2, StockMinimo = 5, PrecioVentaActual = 200, Id_empresa = 1 },
            };
            _mockProductoRepo
                .Setup(r => r.ObtenerPorEmpresaAsync(1))
                .ReturnsAsync(productos);

            var resultado = await _servicio.ObtenerTodosAsync(1);

            resultado.Should().HaveCount(2);
            resultado.Should().Contain(p => p.Nombre == "Prod A");
        }

        [Fact]
        public async Task ObtenerStockCriticoAsync_FiltraCorrectamente()
        {
            var productosCriticos = new List<Producto>
            {
                new() { Id = 2, Nombre = "Prod Bajo", StockActual = 2, StockMinimo = 10, PrecioVentaActual = 50, Id_empresa = 1 },
            };
            _mockProductoRepo
                .Setup(r => r.ObtenerStockCriticoAsync(1))
                .ReturnsAsync(productosCriticos);

            var resultado = await _servicio.ObtenerStockCriticoAsync(1);

            resultado.Should().HaveCount(1);
            resultado.First().Nombre.Should().Be("Prod Bajo");
        }

        [Fact]
        public async Task ObtenerPorIdAsync_ProductoExistente_DevuelveDto()
        {
            var producto = new Producto
            {
                Id = 1,
                Nombre = "Test",
                CodigoBarra = "ABC123",
                PrecioVentaActual = 150m,
                PrecioCostoActual = 100m,
                StockActual = 20,
                StockMinimo = 5,
                Id_empresa = 1
            };
            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdConDetallesAsync(1))
                .ReturnsAsync(producto);

            var resultado = await _servicio.ObtenerPorIdAsync(1);

            resultado.Should().NotBeNull();
            resultado!.Nombre.Should().Be("Test");
            resultado.PrecioVentaActual.Should().Be(150m);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_ProductoNoExiste_DevuelveNull()
        {
            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdConDetallesAsync(999))
                .ReturnsAsync((Producto?)null);

            var resultado = await _servicio.ObtenerPorIdAsync(999);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task CrearAsync_ProductoValido_AgregaYDevuelveDto()
        {
            _mockProductoRepo
                .Setup(r => r.AgregarAsync(It.IsAny<Producto>()))
                .Returns<Producto>(p =>
                {
                    typeof(Producto).GetProperty("Id")!.SetValue(p, 1);
                    return Task.FromResult(p);
                });

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdConDetallesAsync(1))
                .ReturnsAsync(new Producto { Id = 1, Nombre = "Nuevo", PrecioVentaActual = 100, Id_empresa = 1 });

            var dto = new Aplicacion.DTOs.Productos.ProductoCrearDto
            {
                Nombre = "Nuevo",
                PrecioVentaActual = 100m,
                PrecioCostoActual = 50m,
                StockActual = 10,
                StockMinimo = 2,
                IdEmpresa = 1,
                IdCategoria = 1,
                IdUnidadMedida = 1
            };

            var resultado = await _servicio.CrearAsync(dto);

            resultado.Should().NotBeNull();
            _mockProductoRepo.Verify(r => r.AgregarAsync(It.IsAny<Producto>()), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task ActualizarAsync_ProductoExistente_ModificaPropiedades()
        {
            var producto = new Producto { Id = 1, Nombre = "Viejo", PrecioVentaActual = 100, Id_empresa = 1 };
            _mockProductoRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

            // El servicio ProductoServicio.ActualizarAsync solo actualiza Nombre y PrecioVentaActual
            // de acuerdo al ProductoActualizarDto (no expone StockActual)
            var dto = new Aplicacion.DTOs.Productos.ProductoActualizarDto
            {
                IdProducto = 1,
                Nombre = "Nuevo Nombre",
                PrecioVentaActual = 200m,
                PrecioCostoActual = 120m
            };

            await _servicio.ActualizarAsync(dto);

            producto.Nombre.Should().Be("Nuevo Nombre");
            producto.PrecioVentaActual.Should().Be(200m);
            _mockProductoRepo.Verify(r => r.Actualizar(producto), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }
    }
}
