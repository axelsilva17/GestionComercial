using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Inventario;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.DTOs.Inventario;
using GestionComercial.Dominio.Entidades.Movimientos;
using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Dominio.Enumeraciones;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class InventarioServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IMovimientoStockRepositorio> _mockMovimientosRepo = new();
        private readonly Mock<IProductoRepositorio> _mockProductoRepo = new();
        private readonly Mock<ISucursalRepositorio> _mockSucursalRepo = new();
        private readonly Mock<IUsuarioRepositorio> _mockUsuarioRepo = new();
        private readonly InventarioServicio _servicio;

        public InventarioServicioTests()
        {
            _mockUow.Setup(u => u.MovimientosStock).Returns(_mockMovimientosRepo.Object);
            _mockUow.Setup(u => u.Productos).Returns(_mockProductoRepo.Object);
            _mockUow.Setup(u => u.Sucursales).Returns(_mockSucursalRepo.Object);
            _mockUow.Setup(u => u.Usuarios).Returns(_mockUsuarioRepo.Object);

            _servicio = new InventarioServicio(_mockUow.Object);
        }

        // ═══════════════════════════════════════════════════════════
        // RegistrarMovimientoAsync — Salida
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarMovimientoAsync_Salida_CreaMovimientoConTipoCorrecto()
        {
            var producto = new Producto { Id = 1, Nombre = "Prod Test", StockActual = 10 };
            var sucursal = new Sucursal { Id = 1, Nombre = "Sucursal A" };
            var usuario = new Usuario { Id = 1, Nombre = "Juan", Apellido = "Pérez" };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);
            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(sucursal);
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(usuario);
            _mockMovimientosRepo
                .Setup(r => r.AgregarAsync(It.IsAny<MovimientoStock>()))
                .Returns<MovimientoStock>(m => Task.FromResult(m));

            await _servicio.RegistrarMovimientoAsync(
                idProducto: 1,
                tipoMovimiento: "Salida",
                cantidad: 3,
                observacion: "Venta #1 - Prod Test",
                idSucursal: 1,
                idUsuario: 1,
                guardarCambios: false);

            _mockMovimientosRepo.Verify(r => r.AgregarAsync(
                It.Is<MovimientoStock>(m =>
                    m.TipoMovimiento == (int)TipoMovimientoStockEnum.Salida &&
                    m.Cantidad == 3 &&
                    m.StockAnterior == 10 &&
                    m.StockNuevo == 7)), Times.Once);

            producto.StockActual.Should().Be(7);
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_Salida_ConStockInsuficiente_LanzaExcepcion()
        {
            var producto = new Producto { Id = 1, Nombre = "Prod Test", StockActual = 2 };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);

            var act = () => _servicio.RegistrarMovimientoAsync(
                idProducto: 1,
                tipoMovimiento: "Salida",
                cantidad: 5,
                observacion: null,
                idSucursal: 1,
                idUsuario: 1);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*Stock insuficiente*");
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_Salida_GuardarCambiosFalse_NoLlamaGuardarCambios()
        {
            var producto = new Producto { Id = 1, Nombre = "Prod Test", StockActual = 10 };
            var sucursal = new Sucursal { Id = 1, Nombre = "Sucursal A" };
            var usuario = new Usuario { Id = 1, Nombre = "Juan", Apellido = "Pérez" };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);
            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(sucursal);
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(usuario);
            _mockMovimientosRepo
                .Setup(r => r.AgregarAsync(It.IsAny<MovimientoStock>()))
                .Returns<MovimientoStock>(m => Task.FromResult(m));

            await _servicio.RegistrarMovimientoAsync(
                idProducto: 1,
                tipoMovimiento: "Salida",
                cantidad: 2,
                observacion: null,
                idSucursal: 1,
                idUsuario: 1,
                guardarCambios: false);

            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Never);
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_Salida_GuardarCambiosTrue_LlamaGuardarCambios()
        {
            var producto = new Producto { Id = 1, Nombre = "Prod Test", StockActual = 10 };
            var sucursal = new Sucursal { Id = 1, Nombre = "Sucursal A" };
            var usuario = new Usuario { Id = 1, Nombre = "Juan", Apellido = "Pérez" };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);
            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(sucursal);
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(usuario);
            _mockMovimientosRepo
                .Setup(r => r.AgregarAsync(It.IsAny<MovimientoStock>()))
                .Returns<MovimientoStock>(m => Task.FromResult(m));

            await _servicio.RegistrarMovimientoAsync(
                idProducto: 1,
                tipoMovimiento: "Salida",
                cantidad: 2,
                observacion: null,
                idSucursal: 1,
                idUsuario: 1,
                guardarCambios: true);

            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        // ═══════════════════════════════════════════════════════════
        // RegistrarMovimientoAsync — Entrada
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarMovimientoAsync_Entrada_CreaMovimientoConTipoCorrecto()
        {
            var producto = new Producto { Id = 1, Nombre = "Prod Test", StockActual = 5 };
            var sucursal = new Sucursal { Id = 1, Nombre = "Sucursal A" };
            var usuario = new Usuario { Id = 1, Nombre = "Juan", Apellido = "Pérez" };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);
            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(sucursal);
            _mockUsuarioRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(usuario);
            _mockMovimientosRepo
                .Setup(r => r.AgregarAsync(It.IsAny<MovimientoStock>()))
                .Returns<MovimientoStock>(m => Task.FromResult(m));

            await _servicio.RegistrarMovimientoAsync(
                idProducto: 1,
                tipoMovimiento: "Entrada",
                cantidad: 4,
                observacion: "Compra #1",
                idSucursal: 1,
                idUsuario: 1,
                guardarCambios: true);

            _mockMovimientosRepo.Verify(r => r.AgregarAsync(
                It.Is<MovimientoStock>(m =>
                    m.TipoMovimiento == (int)TipoMovimientoStockEnum.Entrada &&
                    m.Cantidad == 4 &&
                    m.StockAnterior == 5 &&
                    m.StockNuevo == 9)), Times.Once);

            producto.StockActual.Should().Be(9);
        }

        // ═══════════════════════════════════════════════════════════
        // RegistrarMovimientoAsync — Validaciones
        // ═══════════════════════════════════════════════════════════

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task RegistrarMovimientoAsync_CantidadInvalida_LanzaExcepcion(decimal cantidad)
        {
            var act = () => _servicio.RegistrarMovimientoAsync(
                idProducto: 1,
                tipoMovimiento: "Salida",
                cantidad: cantidad,
                observacion: null,
                idSucursal: 1,
                idUsuario: 1);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*cantidad*");
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_ProductoNoExiste_LanzaExcepcion()
        {
            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(999))
                .ReturnsAsync((Producto?)null);

            var act = () => _servicio.RegistrarMovimientoAsync(
                idProducto: 999,
                tipoMovimiento: "Salida",
                cantidad: 1,
                observacion: null,
                idSucursal: 1,
                idUsuario: 1);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("*999*");
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerMovimientosPorProductoAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerMovimientosPorProductoAsync_ConSalidas_DevuelveTipoCorrecto()
        {
            var movimientos = new List<MovimientoStock>
            {
                CrearMovimiento(TipoMovimientoStockEnum.Entrada, 5, 0, 5, 1),
                CrearMovimiento(TipoMovimientoStockEnum.Salida, 3, 5, 2, 1),
                CrearMovimiento(TipoMovimientoStockEnum.Salida, 1, 2, 1, 1),
                CrearMovimiento(TipoMovimientoStockEnum.Ajuste, 10, 1, 10, 1),
            };

            _mockMovimientosRepo
                .Setup(r => r.ObtenerPorProductoAsync(1))
                .ReturnsAsync(movimientos);

            var resultado = await _servicio.ObtenerMovimientosPorProductoAsync(1);

            var lista = resultado.ToList();
            lista.Should().HaveCount(4);

            lista.Count(m => m.TipoMovimiento == "Salida").Should().Be(2);
            lista.Count(m => m.TipoMovimiento == "Entrada").Should().Be(1);
            lista.Count(m => m.TipoMovimiento == "Ajuste").Should().Be(1);
        }

        [Fact]
        public async Task ObtenerMovimientosPorProductoAsync_MapeoTipoMovimiento_SalidaEsStringCorrecto()
        {
            var movimientos = new List<MovimientoStock>
            {
                CrearMovimiento(TipoMovimientoStockEnum.Salida, 2, 10, 8, 1),
            };

            _mockMovimientosRepo
                .Setup(r => r.ObtenerPorProductoAsync(1))
                .ReturnsAsync(movimientos);

            var resultado = await _servicio.ObtenerMovimientosPorProductoAsync(1);

            var dto = resultado.Single();
            dto.TipoMovimiento.Should().Be("Salida");
            dto.EsSalida.Should().BeTrue();
            dto.EsEntrada.Should().BeFalse();
            dto.EsAjuste.Should().BeFalse();
            dto.TipoIcono.Should().Be("↓");
            dto.Cantidad.Should().Be(2);
        }

        [Fact]
        public async Task ObtenerMovimientosPorProductoAsync_SinMovimientos_DevuelveVacio()
        {
            _mockMovimientosRepo
                .Setup(r => r.ObtenerPorProductoAsync(999))
                .ReturnsAsync(new List<MovimientoStock>());

            var resultado = await _servicio.ObtenerMovimientosPorProductoAsync(999);

            resultado.Should().BeEmpty();
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerMovimientosAsync (paginado)
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerMovimientosAsync_ConSalidasEnPaginado_DevuelveTipoString()
        {
            var movimientos = new List<MovimientoStock>
            {
                CrearMovimiento(TipoMovimientoStockEnum.Salida, 3, 10, 7, 1),
                CrearMovimiento(TipoMovimientoStockEnum.Entrada, 5, 0, 5, 1),
            };

            _mockMovimientosRepo
                .Setup(r => r.ObtenerPaginadoAsync(
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    1, 15))
                .ReturnsAsync((movimientos, movimientos.Count));

            var resultado = await _servicio.ObtenerMovimientosAsync(
                null, null, null, null,
                DateTime.Today.AddDays(-30), DateTime.Today,
                1, 15, 1);

            var lista = resultado.Items.ToList();
            lista.Should().HaveCount(2);

            var salida = lista.Single(m => m.TipoMovimiento == "Salida");
            salida.TipoMovimiento.Should().Be("Salida");
            salida.EsSalida.Should().BeTrue();
            salida.TipoIcono.Should().Be("↓");
            salida.Cantidad.Should().Be(3);

            var entrada = lista.Single(m => m.TipoMovimiento == "Entrada");
            entrada.TipoMovimiento.Should().Be("Entrada");
            entrada.EsEntrada.Should().BeTrue();
            entrada.TipoIcono.Should().Be("↑");
        }

        [Fact]
        public async Task ObtenerMovimientosAsync_FiltroTipoSalida_FiltraCorrectamente()
        {
            var movimientos = new List<MovimientoStock>
            {
                CrearMovimiento(TipoMovimientoStockEnum.Salida, 2, 10, 8, 1),
            };

            _mockMovimientosRepo
                .Setup(r => r.ObtenerPaginadoAsync(
                    null, "Salida", null, null,
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    1, 15))
                .ReturnsAsync((movimientos, 1));

            var resultado = await _servicio.ObtenerMovimientosAsync(
                null, "Salida", null, null,
                DateTime.Today.AddDays(-30), DateTime.Today,
                1, 15, 1);

            resultado.Items.Should().HaveCount(1);
            resultado.Items.First().TipoMovimiento.Should().Be("Salida");
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerResumenPeriodoAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerResumenPeriodoAsync_ConMovimientos_DevuelveResumenCorrecto()
        {
            var resumenDto = new ResumenMovimientoStockDto
            {
                TotalEntradas = 3,
                TotalSalidas = 2,
                TotalAjustes = 1,
                UnidadesIngresadas = 6,
                UnidadesEgresadas = 5,
            };

            _mockMovimientosRepo
                .Setup(r => r.ObtenerResumenPeriodoAsync(
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(resumenDto);

            var resultado = await _servicio.ObtenerResumenPeriodoAsync(
                DateTime.Today.AddDays(-30), DateTime.Today, 1);

            resultado.TotalEntradas.Should().Be(3);
            resultado.TotalSalidas.Should().Be(2);
            resultado.TotalAjustes.Should().Be(1);
            resultado.UnidadesIngresadas.Should().Be(6);
            resultado.UnidadesEgresadas.Should().Be(5);
            resultado.BalanceNeto.Should().Be(1);
        }

        [Fact]
        public async Task ObtenerResumenPeriodoAsync_SinMovimientos_DevuelveResumenVacio()
        {
            _mockMovimientosRepo
                .Setup(r => r.ObtenerResumenPeriodoAsync(
                    It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                    It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync((ResumenMovimientoStockDto?)null);

            var resultado = await _servicio.ObtenerResumenPeriodoAsync(
                DateTime.Today.AddDays(-30), DateTime.Today, 1);

            resultado.Should().NotBeNull();
            resultado.TotalEntradas.Should().Be(0);
            resultado.TotalSalidas.Should().Be(0);
            resultado.TotalAjustes.Should().Be(0);
            resultado.UnidadesIngresadas.Should().Be(0);
            resultado.UnidadesEgresadas.Should().Be(0);
            resultado.BalanceNeto.Should().Be(0);
        }

        [Fact]
        public async Task ObtenerResumenPeriodoAsync_DelegaAlRepositorioConParametrosCorrectos()
        {
            var desde = new DateTime(2025, 1, 1);
            var hasta = new DateTime(2025, 1, 31);

            _mockMovimientosRepo
                .Setup(r => r.ObtenerResumenPeriodoAsync(desde, hasta, 5, 3))
                .ReturnsAsync(new ResumenMovimientoStockDto());

            await _servicio.ObtenerResumenPeriodoAsync(desde, hasta, 5, 3);

            _mockMovimientosRepo.Verify(r => r.ObtenerResumenPeriodoAsync(
                desde, hasta, 5, 3), Times.Once);
        }

        // ═══════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════

        private static MovimientoStock CrearMovimiento(
            TipoMovimientoStockEnum tipo,
            decimal cantidad,
            decimal stockAnterior,
            decimal stockNuevo,
            int idProducto)
        {
            var mov = tipo switch
            {
                TipoMovimientoStockEnum.Entrada => MovimientoStock.Entrada(
                    cantidad, stockAnterior, idProducto, idSucursal: 1, idUsuario: 1, "Test"),
                TipoMovimientoStockEnum.Salida => MovimientoStock.Salida(
                    cantidad, stockAnterior, idProducto, idSucursal: 1, idUsuario: 1, "Test"),
                TipoMovimientoStockEnum.Ajuste => MovimientoStock.Ajuste(
                    stockNuevo, stockAnterior, idProducto, idSucursal: 1, idUsuario: 1, "Test"),
                _ => throw new ArgumentException($"Tipo inválido: {tipo}")
            };
            return mov;
        }
    }
}
