using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Configuracion;
using GestionComercial.Aplicacion.DTOs.Ventas;

namespace GestionComercial.Tests.Dominio
{
    public class MetodoPagoDtoTests
    {
        [Fact]
        public void MetodoPagoDto_Subcategoria_DefaultEsNulo()
        {
            var dto = new MetodoPagoDto();
            dto.Subcategoria.Should().BeNull();
        }

        [Fact]
        public void MetodoPagoDto_Subcategoria_SeAsignaCorrectamente()
        {
            var dto = new MetodoPagoDto { Subcategoria = "Debito" };
            dto.Subcategoria.Should().Be("Debito");
        }

        [Fact]
        public void MetodoPagoDto_Icono_Tarjeta_MuestraTarjeta()
        {
            var dto = new MetodoPagoDto { Categoria = "Tarjeta" };
            dto.Icono.Should().Be("💳");
        }

        [Fact]
        public void MetodoPagoDto_Icono_Efectivo_MuestraEfectivo()
        {
            var dto = new MetodoPagoDto { Categoria = "Efectivo" };
            dto.Icono.Should().Be("💵");
        }

        [Fact]
        public void MetodoPagoDto_TipoTexto_NoDependeDeSubcategoria()
        {
            var dto = new MetodoPagoDto { Categoria = "Tarjeta", Subcategoria = "Credito" };
            dto.TipoTexto.Should().Be("Tarjeta");
        }

        [Fact]
        public void MetodoPagoDto_RoundTrip()
        {
            var dto = new MetodoPagoDto
            {
                IdMetodoPago = 1,
                Nombre = "Mastercard",
                Categoria = "Tarjeta",
                Subcategoria = "Credito",
                IdEmpresa = 1
            };

            dto.IdMetodoPago.Should().Be(1);
            dto.Nombre.Should().Be("Mastercard");
            dto.Categoria.Should().Be("Tarjeta");
            dto.Subcategoria.Should().Be("Credito");
            dto.Icono.Should().Be("💳");
            dto.TipoTexto.Should().Be("Tarjeta");
        }

        [Fact]
        public void MetodoPagoDto_NonCard_SubcategoriaNull()
        {
            var dto = new MetodoPagoDto { Categoria = "Efectivo", Subcategoria = null };
            dto.Subcategoria.Should().BeNull();
            dto.Icono.Should().Be("💵");
        }

        [Fact]
        public void PagoItemDto_Subcategoria_DefaultEsNulo()
        {
            var dto = new PagoItemDto();
            dto.Subcategoria.Should().BeNull();
        }

        [Fact]
        public void PagoItemDto_Subcategoria_SeAsignaCorrectamente()
        {
            var dto = new PagoItemDto { Subcategoria = "Credito" };
            dto.Subcategoria.Should().Be("Credito");
        }

        [Fact]
        public void PagoItemDto_RoundTrip()
        {
            var dto = new PagoItemDto
            {
                IdMetodoPago = 3,
                NombreMetodo = "Crédito",
                Categoria = "Tarjeta",
                Subcategoria = "Credito",
                Monto = 1000m
            };

            dto.IdMetodoPago.Should().Be(3);
            dto.NombreMetodo.Should().Be("Crédito");
            dto.Categoria.Should().Be("Tarjeta");
            dto.Subcategoria.Should().Be("Credito");
            dto.Monto.Should().Be(1000m);
        }
    }
}
