using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Ventas;
using ProdEntity = GestionComercial.Dominio.Entidades.Producto.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Microsoft.Extensions.Logging;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class VentaServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IVentaRepostorio> _mockVentaRepo = new();
        private readonly Mock<IProductoRepositorio> _mockProductoRepo = new();
        private readonly Mock<IPagoRepositorio> _mockPagoRepo = new();
        private readonly Mock<IMetodoPagoRepositorio> _mockMetodoPagoRepo = new();
        private readonly Mock<IServicioImpresion> _mockImpresion = new();
        private readonly Mock<IInventarioServicio> _mockInventario = new();
        private readonly Mock<ILogger<VentaServicio>> _mockLogger = new();
        private readonly SesionServicio _sesionServicio = new();
        private readonly VentaServicio _servicio;

        public VentaServicioTests()
        {
            _mockUow.Setup(u => u.Ventas).Returns(_mockVentaRepo.Object);
            _mockUow.Setup(u => u.Productos).Returns(_mockProductoRepo.Object);
            _mockUow.Setup(u => u.Pagos).Returns(_mockPagoRepo.Object);
            _mockUow.Setup(u => u.MetodosPago).Returns(_mockMetodoPagoRepo.Object);

            // Mock para EjecutarEnTransaccionAsync: ejecutar el callback inmediatamente
            _mockUow
                .Setup(u => u.EjecutarEnTransaccionAsync(It.IsAny<Func<Task>>()))
                .Returns<Func<Task>>(async callback => await callback());

            _servicio = new VentaServicio(
                _mockUow.Object,
                _mockImpresion.Object,
                _sesionServicio,
                _mockInventario.Object,
                _mockLogger.Object);
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerPorSucursalAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerPorSucursalAsync_ConVentas_DevuelveResumenes()
        {
            var ventas = new List<Venta>
            {
                Venta.Crear(idSucursal: 1, idCliente: 1, idUsuario: 1),
                Venta.Crear(idSucursal: 1, idCliente: 2, idUsuario: 1),
            };
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(ventas);

            var resultado = await _servicio.ObtenerPorSucursalAsync(1, DateTime.Now.AddDays(-1), DateTime.Now);

            resultado.Should().HaveCount(2);
        }

        [Fact]
        public async Task ObtenerPorSucursalAsync_SinVentas_DevuelveVacio()
        {
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new List<Venta>());

            var resultado = await _servicio.ObtenerPorSucursalAsync(1, DateTime.Now.AddDays(-1), DateTime.Now);

            resultado.Should().BeEmpty();
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerVentasAsync (con filtros)
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerVentasAsync_SinFiltros_UsaRangoDefault()
        {
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new List<Venta>());

            await _servicio.ObtenerVentasAsync(1);

            // Verificar que usó el rango default (30 días atrás)
            _mockVentaRepo.Verify(r => r.ObtenerPorFechaAsync(
                It.Is<DateTime>(d => d.Date == DateTime.Today.AddDays(-30)),
                It.IsAny<DateTime>(),
                1), Times.Once);
        }

        [Fact]
        public async Task ObtenerVentasAsync_ConFiltroEstado_FiltraCorrectamente()
        {
            var ventas = new List<Venta>
            {
                CrearVentaPendiente(),
                CrearVentaPendiente(),
            };
            ventas[0].GetType().GetProperty("Estado")!.SetValue(ventas[0], 2); // Pagada
            ventas[1].GetType().GetProperty("Estado")!.SetValue(ventas[1], 1); // Pendiente

            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(ventas);

            var resultado = await _servicio.ObtenerVentasAsync(1, estado: 2);

            resultado.Should().HaveCount(1);
            resultado.First().Estado.Should().Be("Pagada");
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerPorIdAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerPorIdAsync_VentaExistente_DevuelveDto()
        {
            var venta = CrearVentaPendiente();
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 2));

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1))
                .ReturnsAsync(venta);

            var resultado = await _servicio.ObtenerPorIdAsync(1);

            resultado.Should().NotBeNull();
            resultado!.IdVenta.Should().Be(1);
            resultado.Items.Should().HaveCount(1);
        }

        [Fact]
        public async Task ObtenerPorIdAsync_VentaNoExistente_DevuelveNull()
        {
            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(999))
                .ReturnsAsync((Venta?)null);

            var resultado = await _servicio.ObtenerPorIdAsync(999);

            resultado.Should().BeNull();
        }

        // ═══════════════════════════════════════════════════════════
        // CrearAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task CrearAsync_ConStockSuficiente_CreaVenta()
        {
            var producto = new ProdEntity
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

            _mockVentaRepo
                .Setup(r => r.AgregarAsync(It.IsAny<Venta>()))
                .Returns<Venta>(v => Task.FromResult(v));

            // Mock para fallback de busqueda post-creación
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new List<Venta>());

            // Mock para venta recien creada (el fallback busca por CloseTo now)
            var ventaCreada = CrearVentaPendiente();
            ventaCreada.GetType().GetProperty("Id")!.SetValue(ventaCreada, 1);
            ventaCreada.AgregarDetalle(CrearDetalle(100m, 50m, 2));

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(It.IsAny<int>()))
                .ReturnsAsync(ventaCreada);

            // Mock para la búsqueda post-creación (ObtenerPorFechaAsync)
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1))
                .ReturnsAsync(new List<Venta> { ventaCreada });

            var dto = new VentaCrearDto
            {
                IdSucursal = 1,
                IdCliente = 1,
                IdUsuario = 1,
                IdCaja = 5,
                Items = new List<VentaDetalleCrearDto>
                {
                    new() { IdProducto = 1, Cantidad = 2 }
                }
            };

            var resultado = await _servicio.CrearAsync(dto);

            // Verificar que se registró movimiento de stock
            _mockInventario.Verify(i => i.RegistrarMovimientoAsync(
                1, "Salida", 2,
                It.IsAny<string>(), 1, 1,
                false), Times.Once);

            // Verificar que se guardó en repositorio
            _mockVentaRepo.Verify(r => r.AgregarAsync(It.IsAny<Venta>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CrearAsync_StockInsuficiente_LanzaExcepcion()
        {
            var producto = new ProdEntity
            {
                Id = 1,
                Nombre = "Producto Test",
                StockActual = 1,  // Stock insuficiente para cantidad 5
                PrecioVentaActual = 100m,
                PrecioCostoActual = 50m
            };

            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);

            var dto = new VentaCrearDto
            {
                IdSucursal = 1,
                IdCliente = 1,
                IdUsuario = 1,
                Items = new List<VentaDetalleCrearDto>
                {
                    new() { IdProducto = 1, Cantidad = 5 } // Stock es solo 1
                }
            };

            var act = () => _servicio.CrearAsync(dto);

            await act.Should().ThrowAsync<StockInsuficienteException>()
                .WithMessage("*Producto Test*1*5*");
        }

        [Fact]
        public async Task CrearAsync_ProductoNoExiste_LanzaExcepcion()
        {
            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(999))
                .ReturnsAsync((ProdEntity?)null);

            var dto = new VentaCrearDto
            {
                IdSucursal = 1,
                IdCliente = 1,
                IdUsuario = 1,
                Items = new List<VentaDetalleCrearDto>
                {
                    new() { IdProducto = 999, Cantidad = 1 }
                }
            };

            var act = () => _servicio.CrearAsync(dto);

            await act.Should().ThrowAsync<ProductoNoEncontradoException>();
        }

        // ═══════════════════════════════════════════════════════════
        // RegistrarPagoAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarPagoAsync_VentaPagada_LanzaExcepcion()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 2));
            venta.MarcarPagada();

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1))
                .ReturnsAsync(venta);

            var act = () => _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 1, Monto = 200m }
            });

            await act.Should().ThrowAsync<VentaInvalidaException>()
                .WithMessage("*ya está pagada*");
        }

        [Fact]
        public async Task RegistrarPagoAsync_VentaAnulada_LanzaExcepcion()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 2));
            venta.Anular("Test", 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1))
                .ReturnsAsync(venta);

            var act = () => _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 1, Monto = 200m }
            });

            await act.Should().ThrowAsync<VentaInvalidaException>()
                .WithMessage("*está anulada*");
        }

        [Fact]
        public async Task RegistrarPagoAsync_MontoMenorAlTotal_LanzaExcepcion()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 2)); // Total final = 200
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1))
                .ReturnsAsync(venta);

            // Mock para el método de pago
            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(new MetodoPago
                {
                    Id = 1,
                    Nombre = "Efectivo",
                    Categoria = "Efectivo"
                });

            var act = () => _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 1, Monto = 100m } // Solo 100, necesita 200
            });

            await act.Should().ThrowAsync<NegocioException>()
                .WithMessage("*monto pagado*menor*");
        }

        [Fact]
        public async Task RegistrarPagoAsync_PagoExitoso_MarcaVentaPagada()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1)); // Total = 100
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1))
                .ReturnsAsync(venta);

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(2))
                .ReturnsAsync(new MetodoPago
                {
                    Id = 2,
                    Nombre = "Tarjeta Débito",
                    Categoria = "Tarjeta"
                });

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 100m }
            });

            venta.EsPagada.Should().BeTrue();
            venta.Estado.Should().Be(2);

            _mockVentaRepo.Verify(r => r.Actualizar(It.Is<Venta>(v => v.Estado == 2)), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.AtLeastOnce);
        }

        // ═══════════════════════════════════════════════════════════
        // CancelarAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task CancelarAsync_VentaPendiente_AnulaYDevuelveStock()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 2)); // cantidad 2
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1))
                .ReturnsAsync(venta);

            var producto = new ProdEntity
            {
                Id = 1,
                Nombre = "Producto Test",
                StockActual = 8,
                PrecioVentaActual = 100m,
                PrecioCostoActual = 50m
            };
            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(producto);

            await _servicio.CancelarAsync(1, "Error en la venta");

            venta.EsAnulada.Should().BeTrue();
            venta.MotivoAnulacion.Should().Be("Error en la venta");

            // Verificar que devolvió stock: 8 + 2 = 10
            producto.StockActual.Should().Be(10);

            _mockVentaRepo.Verify(r => r.Actualizar(It.Is<Venta>(v => v.Estado == 3)), Times.Once);
            _mockProductoRepo.Verify(r => r.Actualizar(It.Is<ProdEntity>(p => p.StockActual == 10)), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task CancelarAsync_VentaAnulada_LanzaExcepcion()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));
            venta.Anular("Ya anulada", 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1))
                .ReturnsAsync(venta);

            var act = () => _servicio.CancelarAsync(1, "Intento de doble anulación");

            await act.Should().ThrowAsync<VentaInvalidaException>()
                .WithMessage("*ya está anulada*");
        }

        [Fact]
        public async Task CancelarAsync_MotivoVacio_LanzaExcepcion()
        {
            var act = () => _servicio.CancelarAsync(1, "");
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*motivo de anulación es obligatorio*");
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerTotalDelDiaAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerTotalDelDiaAsync_DelegaEnRepositorio()
        {
            _mockVentaRepo
                .Setup(r => r.ObtenerTotalDelDiaAsync(1))
                .ReturnsAsync(15000.50m);

            var total = await _servicio.ObtenerTotalDelDiaAsync(1);

            total.Should().Be(15000.50m);
            _mockVentaRepo.Verify(r => r.ObtenerTotalDelDiaAsync(1), Times.Once);
        }

        // ═══════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════

        private static Venta CrearVentaPendiente()
            => Venta.Crear(idSucursal: 1, idCliente: 1, idUsuario: 1, idCaja: 5);

        private static VentaDetalle CrearDetalle(decimal precio, decimal costo, int cantidad)
        {
            var producto = new ProdEntity
            {
                Id = 1,
                Nombre = "Producto Test",
                PrecioVentaActual = precio,
                PrecioCostoActual = costo
            };
            return VentaDetalle.Crear(producto, cantidad, precio, costo, descuentoPorItem: 0);
        }
    }
}
