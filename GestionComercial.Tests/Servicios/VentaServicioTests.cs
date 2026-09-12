using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Ventas;
using ProdEntity = GestionComercial.Dominio.Entidades.Producto.Producto;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
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
        private readonly Mock<ISucursalRepositorio> _mockSucursalRepo = new();
        private readonly Mock<IMovimientoCajaRepositorio> _mockMovimientoCajaRepo = new();
        private readonly Mock<ICajaRepositorio> _mockCajaRepo = new();
        private readonly Mock<ICategoriaRepositorio> _mockCategoriaRepo = new();
        private readonly Mock<IServicioImpresion> _mockImpresion = new();
        private readonly Mock<IInventarioServicio> _mockInventario = new();
        private readonly Mock<IDescuentoConfiguracionServicio> _mockDescuentoConfig = new();
        private readonly Mock<ILogger<VentaServicio>> _mockLogger = new();
        private readonly SesionServicio _sesionServicio = new();
        private readonly VentaServicio _servicio;

        public VentaServicioTests()
        {
            _sesionServicio.IniciarSesion(new GestionComercial.Aplicacion.DTOs.Usuarios.UsuarioSesionDto
            {
                IdUsuario = 1,
                IdSucursal = 1,
                IdEmpresa = 1,
                Permisos = new HashSet<string> { "Ventas.Crear", "Ventas.Anular", "Caja.Abrir", "Caja.Cerrar" }
            });

            _mockUow.Setup(u => u.Ventas).Returns(_mockVentaRepo.Object);
            _mockUow.Setup(u => u.Productos).Returns(_mockProductoRepo.Object);
            _mockUow.Setup(u => u.Pagos).Returns(_mockPagoRepo.Object);
            _mockUow.Setup(u => u.MetodosPago).Returns(_mockMetodoPagoRepo.Object);
            _mockUow.Setup(u => u.Sucursales).Returns(_mockSucursalRepo.Object);
            _mockUow.Setup(u => u.MovimientosCaja).Returns(_mockMovimientoCajaRepo.Object);
            _mockUow.Setup(u => u.Cajas).Returns(_mockCajaRepo.Object);
            _mockUow.Setup(u => u.Categorias).Returns(_mockCategoriaRepo.Object);

            // Mock para EjecutarEnTransaccionAsync: ejecutar el callback inmediatamente
            _mockUow
                .Setup(u => u.EjecutarEnTransaccionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(async (callback, ct) => await callback());

            _servicio = new VentaServicio(
                _mockUow.Object,
                _mockImpresion.Object,
                _sesionServicio,
                _mockInventario.Object,
                _mockDescuentoConfig.Object,
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
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ventas);

            var resultado = await _servicio.ObtenerPorSucursalAsync(1, DateTime.Now.AddDays(-1), DateTime.Now);

            resultado.Should().HaveCount(2);
        }

        [Fact]
        public async Task ObtenerPorSucursalAsync_SinVentas_DevuelveVacio()
        {
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Venta>());

            await _servicio.ObtenerVentasAsync(1);

            // Verificar que usó el rango default (30 días atrás)
            _mockVentaRepo.Verify(r => r.ObtenerPorFechaAsync(
                It.Is<DateTime>(d => d.Date == DateTime.Today.AddDays(-30)),
                It.IsAny<DateTime>(),
                1,
                It.IsAny<CancellationToken>()), Times.Once);
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
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerConDetallesAsync(999, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(producto);

            _mockProductoRepo
                .Setup(r => r.BuscarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProdEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProdEntity> { producto });

            _mockVentaRepo
                .Setup(r => r.AgregarAsync(It.IsAny<Venta>(), It.IsAny<CancellationToken>()))
                .Returns<Venta, CancellationToken>((v, ct) => Task.FromResult(v));

            // Mock para fallback de busqueda post-creación
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Venta>());

            // Mock para venta recien creada (el fallback busca por CloseTo now)
            var ventaCreada = CrearVentaPendiente();
            ventaCreada.GetType().GetProperty("Id")!.SetValue(ventaCreada, 1);
            ventaCreada.AgregarDetalle(CrearDetalle(100m, 50m, 2));

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ventaCreada);

            // Mock para la búsqueda post-creación (ObtenerPorFechaAsync)
            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, It.IsAny<CancellationToken>()))
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
                false,
                It.IsAny<IUnitOfWork?>(),
                It.IsAny<bool>()), Times.Once);

            // Verificar que se guardó en repositorio
            _mockVentaRepo.Verify(r => r.AgregarAsync(It.IsAny<Venta>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CrearAsync_PasaUnidadTrabajoCompartida_AInventarioServicio()
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
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(producto);

            _mockProductoRepo
                .Setup(r => r.BuscarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProdEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProdEntity> { producto });

            _mockVentaRepo
                .Setup(r => r.AgregarAsync(It.IsAny<Venta>(), It.IsAny<CancellationToken>()))
                .Returns<Venta, CancellationToken>((v, ct) => Task.FromResult(v));

            var ventaCreada = CrearVentaPendiente();
            ventaCreada.GetType().GetProperty("Id")!.SetValue(ventaCreada, 1);
            ventaCreada.AgregarDetalle(CrearDetalle(100m, 50m, 2));

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ventaCreada);

            _mockVentaRepo
                .Setup(r => r.ObtenerPorFechaAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, It.IsAny<CancellationToken>()))
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

            await _servicio.CrearAsync(dto);

            // Verificar que se pasó una unidad de trabajo (no null) para que el movimiento
            // y la venta se persistan en el mismo contexto
            _mockInventario.Verify(i => i.RegistrarMovimientoAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                false,
                It.Is<IUnitOfWork?>(u => u != null),
                It.IsAny<bool>()), Times.AtLeastOnce);
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
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(producto);

            _mockProductoRepo
                .Setup(r => r.BuscarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProdEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProdEntity> { producto });

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
                .Setup(r => r.ObtenerPorIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProdEntity?)null);

            _mockProductoRepo
                .Setup(r => r.BuscarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ProdEntity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProdEntity>());

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
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            // Mock para el método de pago
            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
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
            _mockUow.Verify(u => u.GuardarCambiosAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        // ═══════════════════════════════════════════════════════════
        // CancelarAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task CancelarAsync_VentaPendiente_AnulaYDevuelveStock()
        {
            var venta = CrearVentaPendiente();
            // CrearDetalle ya crea un Producto con Id=1 dentro
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 2)); // cantidad 2
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            // El Producto dentro del detalle tiene StockActual = 0 (default)
            // Configurar ObtenerPorIdAsync para un producto con stock = 8
            // (ya no se usa en CancelarAsync — ahora usa detalle.Producto directamente)
            var producto = new ProdEntity
            {
                Id = 1,
                Nombre = "Producto Test",
                StockActual = 8,
                PrecioVentaActual = 100m,
                PrecioCostoActual = 50m
            };
            _mockProductoRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(producto);

            // Obtener el producto que viene en el detalle
            var detalleProducto = venta.Detalles.First().Producto;
            detalleProducto.StockActual = 8; // simular stock previo

            await _servicio.CancelarAsync(1, "Error en la venta");

            venta.EsAnulada.Should().BeTrue();
            venta.MotivoAnulacion.Should().Be("Error en la venta");

            // Verificar que devolvió stock: 8 + 2 = 10
            detalleProducto.StockActual.Should().Be(10);

            _mockVentaRepo.Verify(r => r.Actualizar(It.Is<Venta>(v => v.Estado == 3)), Times.Once);
            _mockProductoRepo.Verify(r => r.Actualizar(It.Is<ProdEntity>(p => p.StockActual == 10)), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelarAsync_VentaAnulada_LanzaExcepcion()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));
            venta.Anular("Ya anulada", 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
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
                .Setup(r => r.ObtenerTotalDelDiaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(15000.50m);

            var total = await _servicio.ObtenerTotalDelDiaAsync(1);

            total.Should().Be(15000.50m);
            _mockVentaRepo.Verify(r => r.ObtenerTotalDelDiaAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        // ═══════════════════════════════════════════════════════════
        // CobrarVentaAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task CobrarVentaAsync_VentaPendiente_MarcaComoPagada()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1)); // Total = 100
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal
                {
                    Id = 1,
                    Nombre = "Sucursal Test",
                    Id_empresa = 1
                });

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerTodosPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GestionComercial.Dominio.Entidades.Pagos.MetodoPago>
                {
                    new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo" }
                });

            _mockCajaRepo
                .Setup(r => r.ObtenerPorIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Caja.Caja
                {
                    Id = 5,
                    MontoInicial = 0,
                    MontoFinal = 0
                });

            await _servicio.CobrarVentaAsync(1);

            venta.Estado.Should().Be(2); // Pagada
            _mockVentaRepo.Verify(r => r.Actualizar(It.Is<Venta>(v => v.Estado == 2)), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CobrarVentaAsync_VentaNoPendiente_LanzaExcepcion()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));
            venta.MarcarPagada(); // Ya pagada

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            var act = () => _servicio.CobrarVentaAsync(1);

            await act.Should().ThrowAsync<VentaInvalidaException>()
                .WithMessage("*Solo se pueden cobrar ventas en proceso o pendientes*");
        }

        [Fact]
        public async Task CobrarVentaAsync_RegistraPagoEnEfectivo()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(200m, 50m, 1)); // Total = 200
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal
                {
                    Id = 1,
                    Nombre = "Sucursal Test",
                    Id_empresa = 1
                });

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerTodosPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<GestionComercial.Dominio.Entidades.Pagos.MetodoPago>
                {
                    new() { Id = 5, Nombre = "Efectivo", Categoria = "Efectivo" }
                });

            _mockCajaRepo
                .Setup(r => r.ObtenerPorIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Caja.Caja
                {
                    Id = 5,
                    MontoInicial = 0,
                    MontoFinal = 0
                });

            await _servicio.CobrarVentaAsync(1);

            _mockPagoRepo.Verify(r => r.AgregarAsync(
                It.Is<Pago>(p => p.Monto == 200 && p.Id_venta == 1), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CobrarVentaAsync_VentaNoExistente_LanzaExcepcion()
        {
            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Venta?)null);

            var act = () => _servicio.CobrarVentaAsync(999);

            await act.Should().ThrowAsync<VentaInvalidaException>()
                .WithMessage("*no encontrada*");
        }

        // ═══════════════════════════════════════════════════════════
        // Descuento por Método de Pago
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarPagoAsync_ConDescuentoMetodoPago_AplicaDescuentoAlTotalFinal()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(1000m, 500m, 1)); // TotalBruto=1000, TotalDescuento=0, TotalFinal=1000
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });

            // Mock sucursal → empresa
            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal
                {
                    Id = 1, Nombre = "Sucursal Test", Id_empresa = 1
                });

            _mockCategoriaRepo
                .Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            // Descuento 5% sobre el producto 1, restringido a débito (método 2)
            var descuento = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: 1, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            descuento.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockDescuentoConfig
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoConfig
                .Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(descuento);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 950m } // 1000 - 5% = 950
            });

            // DescuentoMetodoPago = 1000 * 5% = 50
            venta.DescuentoMetodoPago.Should().Be(50m);
            venta.Id_metodoPagoDescuento.Should().Be(2);
            venta.TotalFinal.Should().Be(950m); // 1000 - 0 - 50 = 950
            venta.EsPagada.Should().BeTrue();
        }

        [Fact]
        public async Task RegistrarPagoAsync_ConDescuento_ValidacionUsaTotalFinalPostDescuento()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(1000m, 500m, 1)); // TotalFinal=1000
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });

            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal
                {
                    Id = 1, Nombre = "Sucursal Test", Id_empresa = 1
                });

            _mockCategoriaRepo
                .Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            var descuento = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: 1, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            descuento.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockDescuentoConfig
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoConfig
                .Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(descuento);

            // Pago de 950 = total post-descuento (1000 - 50)
            // Sin descuento, 950 < 1000 lanzaría excepción
            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 950m }
            });

            venta.EsPagada.Should().BeTrue();
        }

        [Fact]
        public async Task RegistrarPagoAsync_SinDescuentoMetodoPago_NoModificaTotales()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1)); // TotalFinal=100
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });

            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal
                {
                    Id = 1, Nombre = "Sucursal Test", Id_empresa = 1
                });

            _mockCategoriaRepo
                .Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            // Sin descuentos configurados
            _mockDescuentoConfig
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion>());
            _mockDescuentoConfig
                .Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 100m }
            });

            venta.DescuentoMetodoPago.Should().Be(0m);
            venta.Id_metodoPagoDescuento.Should().BeNull();
            venta.TotalFinal.Should().Be(100m);
            venta.EsPagada.Should().BeTrue();
        }

        [Fact]
        public async Task RegistrarPagoAsync_PagosMixtos_NoAplicaDescuentoMetodoPago()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(1000m, 500m, 1)); // TotalBruto=1000, TotalFinal=1000
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo" });
            _mockMetodoPagoRepo
                .Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });

            _mockSucursalRepo
                .Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal
                {
                    Id = 1, Nombre = "Sucursal Test", Id_empresa = 1
                });

            _mockCategoriaRepo
                .Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            // Débito tiene descuento 5%, pero el pago es MIXTO → ObtenerDescuentoAplicableAsync devuelve null
            _mockDescuentoConfig
                .Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion>());
            _mockDescuentoConfig
                .Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);

            // Pago mixto: Efectivo $600 + Débito $400 = $1000 (sin descuento)
            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 1, Monto = 600m },
                new() { IdMetodoPago = 2, Monto = 400m }
            });

            // Pago mixto: NO se aplica descuento, total sin modificar
            venta.DescuentoMetodoPago.Should().Be(0m);
            venta.Id_metodoPagoDescuento.Should().BeNull();
            venta.TotalFinal.Should().Be(1000m);
            venta.EsPagada.Should().BeTrue();
        }

        // ═══════════════════════════════════════════════════════════
        // MarcarPendienteAsync — service-level guard
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task MarcarPendienteAsync_VentaEnProceso_CambiaAPendiente()
        {
            var venta = CrearVentaPendiente(); // Estado = EnProceso
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            await _servicio.MarcarPendienteAsync(1);

            venta.Estado.Should().Be(1); // Pendiente
            _mockVentaRepo.Verify(r => r.Actualizar(It.Is<Venta>(v => v.Estado == 1)), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MarcarPendienteAsync_VentaPagada_NoCambiaEstado()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));
            venta.MarcarPagada(); // Estado = Pagada
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            await _servicio.MarcarPendienteAsync(1);

            venta.Estado.Should().Be(2); // Still Pagada — no-op
            _mockVentaRepo.Verify(r => r.Actualizar(It.IsAny<Venta>()), Times.Never);
            _mockUow.Verify(u => u.GuardarCambiosAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task MarcarPendienteAsync_VentaYaPendiente_NoopIdempotente()
        {
            var venta = CrearVentaPendiente();
            venta.MarcarPendiente(); // Already Pendiente
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            await _servicio.MarcarPendienteAsync(1);

            venta.Estado.Should().Be(1); // Still Pendiente
            _mockVentaRepo.Verify(r => r.Actualizar(It.IsAny<Venta>()), Times.Never);
            _mockUow.Verify(u => u.GuardarCambiosAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task MarcarPendienteAsync_VentaAnulada_NoCambiaEstado()
        {
            var venta = CrearVentaPendiente();
            venta.Anular("Motivo", 1); // Estado = Anulada
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo
                .Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(venta);

            await _servicio.MarcarPendienteAsync(1);

            venta.Estado.Should().Be(3); // Still Anulada — no-op
            _mockVentaRepo.Verify(r => r.Actualizar(It.IsAny<Venta>()), Times.Never);
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

        // ═══════════════════════════════════════════════════════════
        // T4: Per-item payment discount — RegistrarPagoAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarPagoAsync_UnDetalle_AplicaDescuentoPorItem()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 1000, PrecioCostoActual = 500, Id_categoria = 1 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 1000, 500));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            var descuento = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: 1, aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 2 });
            descuento.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(descuento);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 950m }
            });

            venta.DescuentoMetodoPago.Should().Be(50m); // 1000 * 5%
            venta.TotalFinal.Should().Be(950m);
            venta.EsPagada.Should().BeTrue();
        }

        [Fact]
        public async Task RegistrarPagoAsync_DosDetallesDistintos_AplicaPorItem()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 1000, PrecioCostoActual = 500, Id_categoria = 1 };
            var prod2 = new ProdEntity { Id = 2, Nombre = "B", PrecioVentaActual = 2000, PrecioCostoActual = 1000, Id_categoria = 2 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 1000, 500));
            venta.AgregarDetalle(VentaDetalle.Crear(prod2, 1, 2000, 1000));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            // Descuento 10% en producto 1
            var desc1 = DescuentoConfiguracion.Crear(
                "Prod1 10%", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            // Descuento 5% en categoría 2
            var desc2 = DescuentoConfiguracion.Crear(
                "Cat2 5%", 5, 1, idCategoria: 2, aplicaCualquierMetodoPago: true);

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { desc1, desc2 });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(desc1);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 2, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(desc2);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 2800m } // 3000 - 100 - 100 = 2800
            });

            venta.DescuentoMetodoPago.Should().Be(200m); // 100 + 100
            venta.TotalFinal.Should().Be(2800m);
            venta.EsPagada.Should().BeTrue();
        }

[Fact]
        public async Task RegistrarPagoAsync_DosDetallesSinDescuento_NoAplicaNada()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 1000, PrecioCostoActual = 500, Id_categoria = 1 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 1000, 500));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion>());
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 1000m }
            });

            venta.DescuentoMetodoPago.Should().Be(0m);
            venta.TotalFinal.Should().Be(1000m);
            venta.EsPagada.Should().BeTrue();
        }

        // ═══════════════════════════════════════════════════════════
        // T5: CobrarVentaAsync per-item loop
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task CobrarVentaAsync_ConDescuento_AplicaPorItem()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 1000, PrecioCostoActual = 500, Id_categoria = 1 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 1000, 500));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockMetodoPagoRepo.Setup(r => r.ObtenerTodosPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MetodoPago> { new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo" } });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            var descuento = DescuentoConfiguracion.Crear(
                "Efectivo 10%", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuento });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(descuento);

            await _servicio.CobrarVentaAsync(1);

            venta.DescuentoMetodoPago.Should().Be(100m); // 1000 * 10%
            venta.TotalFinal.Should().Be(900m);
            venta.Estado.Should().Be(2);
        }

        [Fact]
        public async Task CobrarVentaAsync_SinDescuento_TotalFinalIgual()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 500, PrecioCostoActual = 250, Id_categoria = 1 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 500, 250));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockMetodoPagoRepo.Setup(r => r.ObtenerTodosPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<MetodoPago> { new() { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo" } });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion>());
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);

            await _servicio.CobrarVentaAsync(1);

            venta.DescuentoMetodoPago.Should().Be(0m);
            venta.TotalFinal.Should().Be(500m);
            venta.Estado.Should().Be(2);
        }

        // ═══════════════════════════════════════════════════════════
        // T5: REGLA B — descuento total-venta (scope Método de Pago)
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarPagoAsync_PerItemFound_SkipsTotalVenta()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 1000, PrecioCostoActual = 500, Id_categoria = 1 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 1000, 500));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            // Descuento per-item 10% (producto) — gana, no debe llamar total-venta
            var perItem = DescuentoConfiguracion.Crear(
                "Prod 10%", 10, 1, idProducto: 1, aplicaCualquierMetodoPago: true);
            var totalVenta = DescuentoConfiguracion.Crear(
                "Visa 5%", 5, 1, idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 2 },
                alcance: AlcanceDescuentoEnum.MetodoPago);
            totalVenta.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { perItem, totalVenta });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(perItem);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 900m }
            });

            // Per-item gana: 1000 * 10% = 100; total-venta NO se llama
            venta.DescuentoMetodoPago.Should().Be(100m);
            venta.TotalFinal.Should().Be(900m);
            _mockDescuentoConfig.Verify(s => s.ObtenerDescuentoTotalVentaAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task RegistrarPagoAsync_SinPerItem_ConMetodo_AplicaTotalVenta()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 1000, PrecioCostoActual = 500, Id_categoria = 1 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 1000, 500));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            // Sin descuento per-item; descuento total-venta 5% para método 2
            var totalVenta = DescuentoConfiguracion.Crear(
                "Visa 5%", 5, 1, idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 2 },
                alcance: AlcanceDescuentoEnum.MetodoPago);
            totalVenta.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { totalVenta });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoTotalVentaAsync(
                    1, 2, It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalVenta);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 950m }
            });

            // base = TotalBruto(1000) - TotalDescuento(0) = 1000 → 5% = 50
            venta.DescuentoMetodoPago.Should().Be(50m);
            venta.Id_metodoPagoDescuento.Should().Be(2);
            venta.TotalFinal.Should().Be(950m);
            venta.EsPagada.Should().BeTrue();
        }

        [Fact]
        public async Task RegistrarPagoAsync_PagoMixto_SinTotalVenta()
        {
            var venta = CrearVentaPendiente();
            var prod1 = new ProdEntity { Id = 1, Nombre = "A", PrecioVentaActual = 1000, PrecioCostoActual = 500, Id_categoria = 1 };
            venta.AgregarDetalle(VentaDetalle.Crear(prod1, 1, 1000, 500));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo" });
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            var totalVenta = DescuentoConfiguracion.Crear(
                "Visa 5%", 5, 1, idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 2 },
                alcance: AlcanceDescuentoEnum.MetodoPago);
            totalVenta.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { totalVenta });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoTotalVentaAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(totalVenta);

            // Pago mixto: Efectivo 600 + Débito 400 → sin descuento total-venta
            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 1, Monto = 600m },
                new() { IdMetodoPago = 2, Monto = 400m }
            });

            venta.DescuentoMetodoPago.Should().Be(0m);
            venta.Id_metodoPagoDescuento.Should().BeNull();
            venta.TotalFinal.Should().Be(1000m);
            _mockDescuentoConfig.Verify(s => s.ObtenerDescuentoTotalVentaAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        // ═══════════════════════════════════════════════════════════
        // REGLA C: Compra Mayor discount
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarPagoAsync_ConCompraMayor_AplicaDescuento()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(1000m, 500m, 1)); // TotalBruto=1000
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            // Compra mayor: 10% when total >= 500
            var compraMayor = DescuentoConfiguracion.Crear(
                "Compra Mayor 10%", 10, 1,
                alcance: AlcanceDescuentoEnum.CompraMayor,
                montoMinimoCompra: 500m);

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { compraMayor });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoTotalVentaAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoCompraMayorAsync(
                    1, It.IsAny<decimal>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(compraMayor);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 1, Monto = 900m } // 1000 - 10% = 900
            });

            // Compra mayor: 1000 * 10% = 100
            venta.DescuentoMetodoPago.Should().Be(100m);
            venta.Id_metodoPagoDescuento.Should().BeNull(); // Compra mayor no tiene método de pago asociado
            venta.TotalFinal.Should().Be(900m);
            venta.EsPagada.Should().BeTrue();
        }

        [Fact]
        public async Task RegistrarPagoAsync_CompraMayor_NoSuperaUmbral_NoAplica()
        {
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(300m, 150m, 1)); // TotalBruto=300
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            var compraMayor = DescuentoConfiguracion.Crear(
                "Compra Mayor 10%", 10, 1,
                alcance: AlcanceDescuentoEnum.CompraMayor,
                montoMinimoCompra: 500m);

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { compraMayor });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoTotalVentaAsync(
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoCompraMayorAsync(
                    1, It.IsAny<decimal>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 1, Monto = 300m }
            });

            // Umbbral no superado (300 < 500) → sin descuento
            venta.DescuentoMetodoPago.Should().Be(0m);
            venta.TotalFinal.Should().Be(300m);
            venta.EsPagada.Should().BeTrue();
        }

        [Fact]
        public async Task RegistrarPagoAsync_CompraMayor_Exclusividad_GanaCompraMayor()
        {
            // Compra mayor 10% vs método de pago 5%: compra mayor gana (100 > 50)
            var venta = CrearVentaPendiente();
            venta.AgregarDetalle(CrearDetalle(1000m, 500m, 1));
            venta.GetType().GetProperty("Id")!.SetValue(venta, 1);

            _mockVentaRepo.Setup(r => r.ObtenerConDetallesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(venta);
            _mockMetodoPagoRepo.Setup(r => r.ObtenerPorIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta" });
            _mockSucursalRepo.Setup(r => r.ObtenerPorIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GestionComercial.Dominio.Entidades.Organizacion.Sucursal { Id = 1, Id_empresa = 1 });
            _mockCategoriaRepo.Setup(r => r.ObtenerPorEmpresaAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Categoria>());

            var descuentoMtp = DescuentoConfiguracion.Crear(
                "Débito 5%", 5, 1, idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 2 },
                alcance: AlcanceDescuentoEnum.MetodoPago);
            descuentoMtp.DescuentosMetodosPago.Add(new DescuentoMetodoPago { Id_metodoPago = 2 });

            var compraMayor = DescuentoConfiguracion.Crear(
                "Compra Mayor 10%", 10, 1,
                alcance: AlcanceDescuentoEnum.CompraMayor,
                montoMinimoCompra: 500m);

            _mockDescuentoConfig.Setup(s => s.ObtenerTodosAsync(1, It.IsAny<bool?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DescuentoConfiguracion> { descuentoMtp, compraMayor });
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoAplicableAsync(
                    1, 1, It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<bool>(),
                    It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<Dictionary<int, Categoria>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DescuentoConfiguracion?)null);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoTotalVentaAsync(
                    1, 2, It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(descuentoMtp);
            _mockDescuentoConfig.Setup(s => s.ObtenerDescuentoCompraMayorAsync(
                    1, It.IsAny<decimal>(), It.IsAny<List<DescuentoConfiguracion>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(compraMayor);

            await _servicio.RegistrarPagoAsync(1, new List<PagoItemDto>
            {
                new() { IdMetodoPago = 2, Monto = 900m } // 1000 - 10% = 900
            });

            // Compra mayor gana: 1000 * 10% = 100 > 1000 * 5% = 50
            venta.DescuentoMetodoPago.Should().Be(100m);
            venta.Id_metodoPagoDescuento.Should().BeNull(); // Compra mayor no asigna método
            venta.TotalFinal.Should().Be(900m);
            venta.EsPagada.Should().BeTrue();
        }
    }
}
