using FluentAssertions;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Pagos.Strategies;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Dominio
{
    public class EfectivoStrategyTests
    {
        private readonly EfectivoStrategy _strategy = new();
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<IMovimientoCajaRepositorio> _mockMovimientoRepo = new();
        private readonly Mock<ICajaRepositorio> _mockCajaRepo = new();
        private readonly Caja _cajaMock = Caja.Crear(idSucursal: 1, idUsuarioApertura: 1, montoInicial: 0);

        public EfectivoStrategyTests()
        {
            // Set Id to match what tests use
            typeof(Caja).GetProperty(nameof(Caja.Id))!.SetValue(_cajaMock, 5);

            _mockCajaRepo
                .Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_cajaMock);

            _mockUow.Setup(u => u.MovimientosCaja).Returns(_mockMovimientoRepo.Object);
            _mockUow.Setup(u => u.Cajas).Returns(_mockCajaRepo.Object);
        }

        [Fact]
        public void Categoria_EsEfectivo()
        {
            _strategy.Categoria.Should().Be("Efectivo");
        }

        [Fact]
        public void AfectaCajaFisica_EsTrue()
        {
            _strategy.AfectaCajaFisica.Should().BeTrue();
        }

        [Fact]
        public async Task ProcesarPagoAsync_SinCaja_NoCreaMovimiento()
        {
            var venta = Venta.Crear(idSucursal: 1, idCliente: 1, idUsuario: 1, idCaja: null);
            var pago = Pago.Crear(monto: 1000m, idVenta: 1, idMetodoPago: 1);

            await _strategy.ProcesarPagoAsync(pago, venta, _mockUow.Object);

            _mockMovimientoRepo.Verify(r => r.AgregarAsync(It.IsAny<TipoMovimientoCaja>()), Times.Never);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Never);
        }

        [Fact]
        public async Task ProcesarPagoAsync_ConCajaSinVuelto_CreaMovimientoIngreso()
        {
            var venta = Venta.Crear(idSucursal: 1, idCliente: 1, idUsuario: 1, idCaja: 5);
            var pago = Pago.Crear(monto: 1000m, idVenta: 1, idMetodoPago: 1);

            // Simular que el movimiento se guarda con Id = 42
            TipoMovimientoCaja? movGuardado = null;
            _mockMovimientoRepo
                .Setup(r => r.AgregarAsync(It.IsAny<TipoMovimientoCaja>()))
                .Returns<TipoMovimientoCaja>(m =>
                {
                    typeof(TipoMovimientoCaja).GetProperty("Id")!.SetValue(m, 42);
                    movGuardado = m;
                    return Task.FromResult(m);
                });

            await _strategy.ProcesarPagoAsync(pago, venta, _mockUow.Object);

            // Verificar que se creó el movimiento de ingreso
            _mockMovimientoRepo.Verify(r => r.AgregarAsync(It.IsAny<TipoMovimientoCaja>()), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);

            // Validar datos del movimiento
            movGuardado.Should().NotBeNull();
            movGuardado!.Tipo.Should().Be(1); // Ingreso
            movGuardado.Monto.Should().Be(1000m);
            movGuardado.Id_venta.Should().Be(0); // venta.Id = 0 porque no se persistió
            movGuardado.Id_caja.Should().Be(5);
            movGuardado.Id_usuario.Should().Be(1);

            // Verificar que el pago se vinculó
            pago.Linkeado.Should().BeTrue();
            pago.Id_movimientoCaja.Should().Be(42);
        }

        [Fact]
        public async Task ProcesarPagoAsync_ConVuelto_CreaMovimientoIngresoYEgreso()
        {
            var venta = Venta.Crear(idSucursal: 1, idCliente: 1, idUsuario: 1, idCaja: 5);
            var pago = Pago.Crear(monto: 1000m, idVenta: 1, idMetodoPago: 1);
            pago.Vuelto = 200m;

            var movimientosGuardados = new List<TipoMovimientoCaja>();
            _mockMovimientoRepo
                .Setup(r => r.AgregarAsync(It.IsAny<TipoMovimientoCaja>()))
                .Returns<TipoMovimientoCaja>(m =>
                {
                    typeof(TipoMovimientoCaja).GetProperty("Id")!.SetValue(m, 99 + movimientosGuardados.Count);
                    movimientosGuardados.Add(m);
                    return Task.FromResult(m);
                });

            await _strategy.ProcesarPagoAsync(pago, venta, _mockUow.Object);

            // Verificar que se crearon DOS movimientos
            _mockMovimientoRepo.Verify(r => r.AgregarAsync(It.IsAny<TipoMovimientoCaja>()), Times.Exactly(2));
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);

            // Primer movimiento: Ingreso por monto neto (1000 - 200 = 800)
            movimientosGuardados.Should().HaveCount(2);
            movimientosGuardados[0].Tipo.Should().Be(1); // Ingreso
            movimientosGuardados[0].Monto.Should().Be(800m);

            // Segundo movimiento: Egreso por vuelto
            movimientosGuardados[1].Tipo.Should().Be(2); // Egreso
            movimientosGuardados[1].Monto.Should().Be(200m);
            movimientosGuardados[1].Concepto.Should().Contain("Vuelto");
        }

        [Fact]
        public async Task ProcesarPagoAsync_ActualizaEfectivoRecibidoEnVenta()
        {
            var venta = Venta.Crear(idSucursal: 1, idCliente: 1, idUsuario: 1, idCaja: 5);
            var pago1 = Pago.Crear(monto: 500m, idVenta: 1, idMetodoPago: 1);
            var pago2 = Pago.Crear(monto: 300m, idVenta: 1, idMetodoPago: 1);

            _mockMovimientoRepo
                .Setup(r => r.AgregarAsync(It.IsAny<TipoMovimientoCaja>()))
                .Returns<TipoMovimientoCaja>(m =>
                {
                    typeof(TipoMovimientoCaja).GetProperty("Id")!.SetValue(m, 1);
                    return Task.FromResult(m);
                });

            venta.EfectivoRecibido.Should().BeNull();

            await _strategy.ProcesarPagoAsync(pago1, venta, _mockUow.Object);
            venta.EfectivoRecibido.Should().Be(500m);

            await _strategy.ProcesarPagoAsync(pago2, venta, _mockUow.Object);
            venta.EfectivoRecibido.Should().Be(800m); // Acumulado
        }
    }

    public class TarjetaStrategyTests
    {
        private readonly TarjetaStrategy _strategy = new();

        [Fact]
        public void Categoria_EsTarjeta()
        {
            _strategy.Categoria.Should().Be("Tarjeta");
        }

        [Fact]
        public void AfectaCajaFisica_EsFalse()
        {
            _strategy.AfectaCajaFisica.Should().BeFalse();
        }

        [Fact]
        public async Task ProcesarPagoAsync_NoHaceNada()
        {
            var venta = Venta.Crear(1, 1, 1);
            var pago = Pago.Crear(1000m, 1, 2);
            var mockUow = new Mock<IUnitOfWork>();

            await _strategy.ProcesarPagoAsync(pago, venta, mockUow.Object);

            // No debe interactuar con el UnitOfWork
            mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Never);
        }
    }

    public class TransferenciaStrategyTests
    {
        private readonly TransferenciaStrategy _strategy = new();

        [Fact]
        public void Categoria_EsTransferencia()
        {
            _strategy.Categoria.Should().Be("Transferencia");
        }

        [Fact]
        public void AfectaCajaFisica_EsFalse()
        {
            _strategy.AfectaCajaFisica.Should().BeFalse();
        }

        [Fact]
        public async Task ProcesarPagoAsync_NoHaceNada()
        {
            var venta = Venta.Crear(1, 1, 1);
            var pago = Pago.Crear(1000m, 1, 4);
            var mockUow = new Mock<IUnitOfWork>();

            await _strategy.ProcesarPagoAsync(pago, venta, mockUow.Object);

            mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Never);
        }
    }

    public class OtroStrategyTests
    {
        private readonly OtroStrategy _strategy = new();

        [Fact]
        public void Categoria_EsOtro()
        {
            _strategy.Categoria.Should().Be("Otro");
        }

        [Fact]
        public void AfectaCajaFisica_EsFalse()
        {
            _strategy.AfectaCajaFisica.Should().BeFalse();
        }

        [Fact]
        public async Task ProcesarPagoAsync_NoHaceNada()
        {
            var venta = Venta.Crear(1, 1, 1);
            var pago = Pago.Crear(1000m, 1, 5);
            var mockUow = new Mock<IUnitOfWork>();

            await _strategy.ProcesarPagoAsync(pago, venta, mockUow.Object);

            mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Never);
        }
    }
}
