using FluentAssertions;
using GestionComercial.Dominio.Entidades.Pagos.Strategies;

namespace GestionComercial.Tests.Dominio
{
    public class PaymentStrategyFactoryTests
    {
        private readonly PaymentStrategyFactory _factory = new();

        // ═══════════════════════════════════════════════════════════
        // Resolución por categoría
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void Resolve_Efectivo_ReturnsEfectivoStrategy()
        {
            var strategy = _factory.Resolve("Efectivo");
            strategy.Should().BeOfType<EfectivoStrategy>();
            strategy.Categoria.Should().Be("Efectivo");
            strategy.AfectaCajaFisica.Should().BeTrue();
        }

        [Fact]
        public void Resolve_Tarjeta_ReturnsTarjetaStrategy()
        {
            var strategy = _factory.Resolve("Tarjeta");
            strategy.Should().BeOfType<TarjetaStrategy>();
            strategy.Categoria.Should().Be("Tarjeta");
            strategy.AfectaCajaFisica.Should().BeFalse();
        }

        [Fact]
        public void Resolve_Transferencia_ReturnsTransferenciaStrategy()
        {
            var strategy = _factory.Resolve("Transferencia");
            strategy.Should().BeOfType<TransferenciaStrategy>();
            strategy.Categoria.Should().Be("Transferencia");
            strategy.AfectaCajaFisica.Should().BeFalse();
        }

        [Fact]
        public void Resolve_Otro_ReturnsOtroStrategy()
        {
            var strategy = _factory.Resolve("Otro");
            strategy.Should().BeOfType<OtroStrategy>();
            strategy.Categoria.Should().Be("Otro");
            strategy.AfectaCajaFisica.Should().BeFalse();
        }

        // ═══════════════════════════════════════════════════════════
        // Case insensitive
        // ═══════════════════════════════════════════════════════════

        [Theory]
        [InlineData("efectivo")]
        [InlineData("EFECTIVO")]
        [InlineData("EfEcTiVo")]
        public void Resolve_EfectivoCaseInsensitive_ReturnsEfectivoStrategy(string input)
        {
            var strategy = _factory.Resolve(input);
            strategy.Should().BeOfType<EfectivoStrategy>();
        }

        // ═══════════════════════════════════════════════════════════
        // Fallback: categoría desconocida
        // ═══════════════════════════════════════════════════════════

        [Theory]
        [InlineData("QR")]
        [InlineData("Cripto")]
        [InlineData("Cheque")]
        [InlineData("")]
        [InlineData("INEXISTENTE")]
        public void Resolve_CategoriaDesconocida_FallbackAOtro(string categoria)
        {
            var strategy = _factory.Resolve(categoria);
            strategy.Should().BeOfType<OtroStrategy>();
            strategy.AfectaCajaFisica.Should().BeFalse();
        }

        // ═══════════════════════════════════════════════════════════
        // Singleton behavior: each Resolve returns same instance
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void Resolve_SiempreMismaInstancia_EsSingleton()
        {
            var instance1 = _factory.Resolve("Efectivo");
            var instance2 = _factory.Resolve("Efectivo");

            instance1.Should().BeSameAs(instance2);
        }
    }
}
