using FluentAssertions;
using GestionComercial.Dominio.Entidades.Pagos;

namespace GestionComercial.Tests.Dominio
{
    public class PagoTests
    {
        // ═══════════════════════════════════════════════════════════
        // Factory method: Pago.Crear()
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void Crear_PagoValido_PropiedadesCorrectas()
        {
            var pago = Pago.Crear(monto: 1500m, idVenta: 1, idMetodoPago: 2);

            pago.Monto.Should().Be(1500m);
            pago.Id_venta.Should().Be(1);
            pago.Id_metodoPago.Should().Be(2);
            pago.Fecha.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
            pago.Id.Should().Be(0); // Sin persistir
            pago.Linkeado.Should().BeFalse();
            pago.Vuelto.Should().Be(0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(-0.01)]
        public void Crear_MontoInvalido_LanzaExcepcion(decimal montoInvalido)
        {
            var act = () => Pago.Crear(montoInvalido, idVenta: 1, idMetodoPago: 1);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*monto debe ser mayor a 0*");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Crear_IdVentaInvalido_LanzaExcepcion(int idVenta)
        {
            var act = () => Pago.Crear(monto: 100, idVenta, idMetodoPago: 1);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ID de venta inválido*");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Crear_IdMetodoPagoInvalido_LanzaExcepcion(int idMetodoPago)
        {
            var act = () => Pago.Crear(monto: 100, idVenta: 1, idMetodoPago);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ID de método de pago inválido*");
        }

        // ═══════════════════════════════════════════════════════════
        // Propiedad Vuelto
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void Vuelto_ValorPositivo_SeAsignaCorrectamente()
        {
            var pago = Pago.Crear(500m, 1, 1);
            pago.Vuelto = 200m;
            pago.Vuelto.Should().Be(200m);
        }

        [Fact]
        public void Vuelto_Cero_EsValido()
        {
            var pago = Pago.Crear(500m, 1, 1);
            pago.Vuelto = 0;
            pago.Vuelto.Should().Be(0);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-0.01)]
        [InlineData(-500)]
        public void Vuelto_Negativo_LanzaExcepcion(decimal vueltoNegativo)
        {
            var pago = Pago.Crear(500m, 1, 1);
            var act = () => pago.Vuelto = vueltoNegativo;
            act.Should().Throw<ArgumentException>()
                .WithMessage("*vuelto no puede ser negativo*");
        }

        // ═══════════════════════════════════════════════════════════
        // VincularMovimientoCaja
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void VincularMovimientoCaja_IdValido_SeVincula()
        {
            var pago = Pago.Crear(500m, 1, 1);
            pago.Linkeado.Should().BeFalse();

            pago.VincularMovimientoCaja(42);

            pago.Linkeado.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void VincularMovimientoCaja_IdInvalido_LanzaExcepcion(int idInvalido)
        {
            var pago = Pago.Crear(500m, 1, 1);
            var act = () => pago.VincularMovimientoCaja(idInvalido);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ID de movimiento de caja inválido*");
        }

        // ═══════════════════════════════════════════════════════════
        // Propiedad Monto (setter directo con validación)
        // ═══════════════════════════════════════════════════════════

        [Theory]
        [InlineData(0)]
        [InlineData(-0.01)]
        [InlineData(-1000)]
        public void Monto_ValorInvalido_LanzaExcepcion(decimal montoInvalido)
        {
            var pago = Pago.Crear(100m, 1, 1);
            var act = () => pago.Monto = montoInvalido;
            act.Should().Throw<ArgumentException>()
                .WithMessage("*monto debe ser mayor a 0*");
        }

        [Fact]
        public void Monto_ValorValido_SeAsignaCorrectamente()
        {
            var pago = Pago.Crear(100m, 1, 1);
            pago.Monto = 2500.50m;
            pago.Monto.Should().Be(2500.50m);
        }
    }
}
