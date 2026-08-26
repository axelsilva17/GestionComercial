using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Caja;
using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Auditoria;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using Moq;

namespace GestionComercial.Tests.Servicios
{
    public class CajaServicioTests
    {
        private readonly Mock<IUnitOfWork> _mockUow = new();
        private readonly Mock<ICajaRepositorio> _mockCajaRepo = new();
        private readonly Mock<IMovimientoCajaRepositorio> _mockMovRepo = new();
        private readonly Mock<IAuditoriaRepositorio> _mockAuditoria = new();
        private readonly Mock<ISucursalRepositorio> _mockSucursalRepo = new();
        private readonly Mock<IVentaRepostorio> _mockVentaRepo = new();
        private readonly Mock<IPagoRepositorio> _mockPagoRepo = new();
        private readonly Mock<IMetodoPagoRepositorio> _mockMetodoPagoRepo = new();
        private readonly SesionServicio _sesionServicio = new();
        private readonly CajaServicio _servicio;

        public CajaServicioTests()
        {
            _mockUow.Setup(u => u.Cajas).Returns(_mockCajaRepo.Object);
            _mockUow.Setup(u => u.MovimientosCaja).Returns(_mockMovRepo.Object);
            _mockUow.Setup(u => u.Auditoria).Returns(_mockAuditoria.Object);
            _mockUow.Setup(u => u.Sucursales).Returns(_mockSucursalRepo.Object);
            _mockUow.Setup(u => u.Ventas).Returns(_mockVentaRepo.Object);
            _mockUow.Setup(u => u.Pagos).Returns(_mockPagoRepo.Object);
            _mockUow.Setup(u => u.MetodosPago).Returns(_mockMetodoPagoRepo.Object);

            _mockAuditoria
                .Setup(a => a.RegistrarAuditoriaAsync(
                    It.IsAny<string>(), It.IsAny<int>(), It.IsAny<OperacionAuditoriaEnum>(),
                    It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(Task.CompletedTask);

            _servicio = new CajaServicio(_mockUow.Object, _sesionServicio);
        }

        // ═══════════════════════════════════════════════════════════
        // RegistrarMovimientoAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task RegistrarMovimientoAsync_Ingreso_AumentaMontoFinal()
        {
            var caja = CrearCajaAbierta(montoInicial: 1000, montoFinal: 1000);

            _mockCajaRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(caja);

            await _servicio.RegistrarMovimientoAsync(
                1, TipoMovimientoCajaEnum.Ingreso, 500, "Ingreso manual");

            caja.MontoFinal.Should().Be(1500);
            _mockCajaRepo.Verify(r => r.Actualizar(It.Is<Caja>(c => c.MontoFinal == 1500)), Times.Once);
            _mockMovRepo.Verify(r => r.AgregarAsync(It.Is<TipoMovimientoCaja>(m =>
                m.Tipo == (int)TipoMovimientoCajaEnum.Ingreso && m.Monto == 500)), Times.Once);
            _mockUow.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_Egreso_DisminuyeMontoFinal()
        {
            var caja = CrearCajaAbierta(montoInicial: 1000, montoFinal: 1000);

            _mockCajaRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(caja);

            await _servicio.RegistrarMovimientoAsync(
                1, TipoMovimientoCajaEnum.Egreso, 200, "Egreso manual");

            caja.MontoFinal.Should().Be(800);
            _mockCajaRepo.Verify(r => r.Actualizar(It.Is<Caja>(c => c.MontoFinal == 800)), Times.Once);
            _mockMovRepo.Verify(r => r.AgregarAsync(It.Is<TipoMovimientoCaja>(m =>
                m.Tipo == (int)TipoMovimientoCajaEnum.Egreso && m.Monto == 200)), Times.Once);
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_CajaCerrada_LanzaExcepcion()
        {
            var caja = CrearCajaAbierta(montoInicial: 1000, montoFinal: 1000);
            caja.Cerrar(1, 1000);

            _mockCajaRepo
                .Setup(r => r.ObtenerPorIdAsync(1))
                .ReturnsAsync(caja);

            var act = () => _servicio.RegistrarMovimientoAsync(
                1, TipoMovimientoCajaEnum.Ingreso, 500, "Test");

            await act.Should().ThrowAsync<CajaNoAbiertaException>();
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_CajaNoExiste_LanzaExcepcion()
        {
            _mockCajaRepo
                .Setup(r => r.ObtenerPorIdAsync(999))
                .ReturnsAsync((Caja?)null);

            var act = () => _servicio.RegistrarMovimientoAsync(
                999, TipoMovimientoCajaEnum.Ingreso, 500, "Test");

            await act.Should().ThrowAsync<CajaNoAbiertaException>();
        }

        // ═══════════════════════════════════════════════════════════
        // ObtenerMovimientosAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task ObtenerMovimientosAsync_ConMovimientos_DevuelveDtoCorrectos()
        {
            var movimientos = new List<TipoMovimientoCaja>
            {
                new() { Id = 1, Tipo = (int)TipoMovimientoCajaEnum.Apertura, Monto = 1000, Concepto = "Apertura" },
                new() { Id = 2, Tipo = (int)TipoMovimientoCajaEnum.Ingreso, Monto = 500, Concepto = "Venta #1" },
                new() { Id = 3, Tipo = (int)TipoMovimientoCajaEnum.Egreso, Monto = 100, Concepto = "Vuelto" },
                new() { Id = 4, Tipo = (int)TipoMovimientoCajaEnum.Cierre, Monto = 1400, Concepto = "Cierre" },
            };

            _mockMovRepo
                .Setup(r => r.ObtenerPorCajaAsync(1))
                .ReturnsAsync(movimientos);

            var resultado = (await _servicio.ObtenerMovimientosAsync(1)).ToList();

            resultado.Should().HaveCount(4);
            resultado[0].Tipo.Should().Be("Apertura");
            resultado[0].EsApertura.Should().BeTrue();
            resultado[0].EsIngreso.Should().BeFalse();
            resultado[1].Tipo.Should().Be("Ingreso");
            resultado[1].EsIngreso.Should().BeTrue();
            resultado[2].Tipo.Should().Be("Egreso");
            resultado[2].EsIngreso.Should().BeFalse();
            resultado[3].Tipo.Should().Be("Cierre");
            resultado[3].EsApertura.Should().BeFalse();
            resultado[3].EsIngreso.Should().BeFalse();
        }

        // ═══════════════════════════════════════════════════════════
        // Saldo computation (pure function test)
        // ═══════════════════════════════════════════════════════════

        [Theory]
        [InlineData(1000, 500, 200, 1300)]   // 1000 + 500 - 200
        [InlineData(0, 100, 50, 50)]          // 0 + 100 - 50
        [InlineData(1000, 0, 0, 1000)]        // Sin movimientos adicionales
        [InlineData(500, 0, 300, 200)]        // 500 + 0 - 300
        public void CalcularSaldo_DeberiaSerCorrecto(
            decimal montoInicial, decimal totalIngresos, decimal totalEgresos, decimal esperado)
        {
            var saldo = CalcularSaldo(montoInicial, totalIngresos, totalEgresos);
            saldo.Should().Be(esperado);
        }

        [Fact]
        public void CalcularSaldo_ConMovimientosVarios_CalculaCorrectamente()
        {
            // Simular movimientos: Apertura 1000, Ingreso 500, Egreso 200, Cierre 1300
            var movimientos = new List<MovimientoCajaDto>
            {
                new() { Tipo = "Apertura", Monto = 1000 },
                new() { Tipo = "Ingreso", Monto = 500 },
                new() { Tipo = "Egreso", Monto = 200 },
                new() { Tipo = "Cierre", Monto = 1300 },
            };

            var montoInicial = movimientos.First(m => m.EsApertura).Monto;
            var totalIngresos = movimientos
                .Where(m => m.EsIngreso && !m.EsApertura && !m.Tipo.Contains("Cierre"))
                .Sum(m => m.Monto);
            var totalEgresos = movimientos
                .Where(m => !m.EsIngreso && !m.EsApertura && !m.Tipo.Contains("Cierre"))
                .Sum(m => m.Monto);

            var saldo = CalcularSaldo(montoInicial, totalIngresos, totalEgresos);

            saldo.Should().Be(1300); // 1000 + 500 - 200
        }

        [Fact]
        public void CalcularSaldo_AperturaYCierre_NoAfectanSaldo()
        {
            var movimientos = new List<MovimientoCajaDto>
            {
                new() { Tipo = "Apertura", Monto = 2000 },
                new() { Tipo = "Ingreso", Monto = 300 },
                new() { Tipo = "Egreso", Monto = 100 },
                new() { Tipo = "Cierre", Monto = 2200 },
            };

            var montoInicial = movimientos.First(m => m.EsApertura).Monto;
            var totalIngresos = movimientos
                .Where(m => m.EsIngreso && !m.EsApertura && !m.Tipo.Contains("Cierre"))
                .Sum(m => m.Monto);
            var totalEgresos = movimientos
                .Where(m => !m.EsIngreso && !m.EsApertura && !m.Tipo.Contains("Cierre"))
                .Sum(m => m.Monto);

            // Apertura and Cierre should NOT be counted in ingresos/egresos
            totalIngresos.Should().Be(300);
            totalEgresos.Should().Be(100);

            var saldo = CalcularSaldo(montoInicial, totalIngresos, totalEgresos);
            saldo.Should().Be(2200); // 2000 + 300 - 100
        }

        // ═══════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════

        // ═══════════════════════════════════════════════════════════
        // EliminarCajaAsync
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public async Task EliminarCajaAsync_CajaNoPrimariaCerradaSinMovimientos_Elimina()
        {
            var caja = CrearCajaCerrada(esPrimaria: false);
            _mockCajaRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(caja);
            _mockMovRepo.Setup(r => r.ObtenerPorCajaAsync(1))
                .ReturnsAsync(new List<GestionComercial.Dominio.Entidades.Caja.TipoMovimientoCaja>());

            await _servicio.EliminarCajaAsync(1);

            caja.Activo.Should().BeFalse();
            _mockCajaRepo.Verify(r => r.Actualizar(caja), Times.Once);
        }

        [Fact]
        public async Task EliminarCajaAsync_CajaPrimaria_LanzaExcepcion()
        {
            var caja = CrearCajaCerrada(esPrimaria: true);
            _mockCajaRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(caja);

            var act = () => _servicio.EliminarCajaAsync(1);
            await act.Should().ThrowAsync<GestionComercial.Aplicacion.Excepciones.NegocioException>()
                .WithMessage("*primaria*");
        }

        [Fact]
        public async Task EliminarCajaAsync_CajaAbierta_LanzaExcepcion()
        {
            var caja = CrearCajaAbierta();
            _mockCajaRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(caja);

            var act = () => _servicio.EliminarCajaAsync(1);
            await act.Should().ThrowAsync<GestionComercial.Aplicacion.Excepciones.NegocioException>()
                .WithMessage("*abierta*");
        }

        [Fact]
        public async Task EliminarCajaAsync_CajaConMovimientos_LanzaExcepcion()
        {
            var caja = CrearCajaCerrada(esPrimaria: false);
            _mockCajaRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(caja);
            _mockMovRepo.Setup(r => r.ObtenerPorCajaAsync(1))
                .ReturnsAsync(new List<GestionComercial.Dominio.Entidades.Caja.TipoMovimientoCaja>
                {
                    new() { Id = 1, Tipo = 1, Monto = 500, Id_caja = 1 }
                });

            var act = () => _servicio.EliminarCajaAsync(1);
            await act.Should().ThrowAsync<GestionComercial.Aplicacion.Excepciones.NegocioException>()
                .WithMessage("*movimientos*");
        }

        [Fact]
        public async Task EliminarCajaAsync_CajaNoEncontrada_LanzaExcepcion()
        {
            _mockCajaRepo.Setup(r => r.ObtenerPorIdAsync(999))
                .ReturnsAsync((Caja?)null);

            var act = () => _servicio.EliminarCajaAsync(999);
            await act.Should().ThrowAsync<GestionComercial.Aplicacion.Excepciones.NegocioException>()
                .WithMessage("*no encontrada*");
        }

        private static Caja CrearCajaCerrada(bool esPrimaria = false)
        {
            var caja = Caja.Crear(idSucursal: 1, idUsuarioApertura: 1, montoInicial: 1000, esPrimaria: esPrimaria);
            caja.Cerrar(idUsuarioCierre: 1, montoFinal: 1000);
            return caja;
        }

        // ═══════════════════════════════════════════════════════════

        private static Caja CrearCajaAbierta(decimal montoInicial = 1000, decimal? montoFinal = null)
        {
            var caja = Caja.Crear(idSucursal: 1, idUsuarioApertura: 1, montoInicial: montoInicial);
            caja.MontoFinal = montoFinal ?? montoInicial;
            return caja;
        }

        /// <summary>
        /// Pure function mirroring CajaViewModel.RecalcularSaldo().
        /// Extracted for testability.
        /// </summary>
        private static decimal CalcularSaldo(decimal montoInicial, decimal totalIngresos, decimal totalEgresos)
            => montoInicial + totalIngresos - totalEgresos;
    }
}
