using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.UI.ViewModels.Ventas;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GestionComercial.Tests.UI
{
    public class VentaListadoViewModelTests
    {
        private readonly Mock<IVentaServicio> _mockServicio;
        private readonly VentaListadoViewModel _vm;

        public VentaListadoViewModelTests()
        {
            _mockServicio = new Mock<IVentaServicio>();
            _mockServicio
                .Setup(s => s.ObtenerRecientesPorSucursalAsync(
                    It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new System.Collections.Generic.List<VentaResumenDto>());
            _vm = new VentaListadoViewModel(_mockServicio.Object, new SesionServicio());
        }

        [Fact]
        public void Constructor_SinFiltroCliente_ArrancaConUltimos30Dias()
        {
            // El defecto debe ser 30 días, no "hoy": en DBs con ventas viejas el rango
            // de un solo día arrancaba vacío (fix del historial de ventas).
            _vm.FechaDesde.Should().Be(DateTime.Today.AddDays(-30));
            _vm.FechaHasta.Should().Be(DateTime.Today.AddDays(1).AddSeconds(-1));
        }

        [Fact]
        public async Task CargarDetalleVentaAsync_ConVenta_CargaDetalleLazy()
        {
            var venta = new VentaResumenDto { IdVenta = 7, Estado = "Pagada" };
            var detalle = new VentaDto { IdVenta = 7, ClienteNombre = "Juan", Estado = "Pagada" };
            _mockServicio
                .Setup(s => s.ObtenerPorIdAsync(7, It.IsAny<CancellationToken>()))
                .ReturnsAsync(detalle);

            // El flujo real: SelectionChanged setea la selección antes de pedir el detalle.
            _vm.VentaSeleccionada = venta;
            await _vm.CargarDetalleVentaAsync(venta);

            _vm.DetalleVenta.Should().NotBeNull();
            _vm.DetalleVenta!.IdVenta.Should().Be(7);
            _mockServicio.Verify(s => s.ObtenerPorIdAsync(7, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CargarDetalleVentaAsync_RepetirOVentaNula_CierraDrawer()
        {
            var venta = new VentaResumenDto { IdVenta = 7, Estado = "Pagada" };
            _mockServicio
                .Setup(s => s.ObtenerPorIdAsync(7, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new VentaDto { IdVenta = 7 });

            // Flujo real: la fila está seleccionada antes de pedir el detalle.
            _vm.VentaSeleccionada = venta;
            await _vm.CargarDetalleVentaAsync(venta);
            _vm.DetalleVenta.Should().NotBeNull();

            // Re-seleccionar la misma venta hace toggle → cierra.
            await _vm.CargarDetalleVentaAsync(venta);
            _vm.DetalleVenta.Should().BeNull();

            // Selección nula cierra sin tocar el servicio.
            await _vm.CargarDetalleVentaAsync(null);
            _vm.DetalleVenta.Should().BeNull();
            _mockServicio.Verify(s => s.ObtenerPorIdAsync(7, It.IsAny<CancellationToken>()), Times.Once);

            // CerrarDetalle explícito también limpia.
            _vm.VentaSeleccionada = venta;
            await _vm.CargarDetalleVentaAsync(venta);
            _vm.CerrarDetalle();
            _vm.DetalleVenta.Should().BeNull();
        }

        [Fact]
        public async Task CargarDetalleVentaAsync_SiFalla_MuestraError()
        {
            _mockServicio
                .Setup(s => s.ObtenerPorIdAsync(9, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

            _vm.VentaSeleccionada = new VentaResumenDto { IdVenta = 9 };
            await _vm.CargarDetalleVentaAsync(new VentaResumenDto { IdVenta = 9 });

            _vm.TieneError.Should().BeTrue();
            _vm.MensajeError.Should().Be("boom");
            _vm.DetalleVenta.Should().BeNull();
        }

        [Fact]
        public void FiltrarEstaSemana_ArrancaEnLunes()
        {
            // El cálculo naive (-DayOfWeek + 1) empujaba el inicio al lunes SIGUIENTE los
            // domingos (DayOfWeek == 0), dejando un rango vacío.
            _vm.FiltrarEstaSemana();

            _vm.FechaDesde.DayOfWeek.Should().Be(DayOfWeek.Monday);
            _vm.FechaHasta.Should().BeAfter(_vm.FechaDesde);
        }
    }
}