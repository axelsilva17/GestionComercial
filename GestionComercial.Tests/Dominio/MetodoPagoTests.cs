using FluentAssertions;
using GestionComercial.Dominio.Entidades.Pagos;

namespace GestionComercial.Tests.Dominio
{
    public class MetodoPagoTests
    {
        [Fact]
        public void Constructor_ValoresDefault_Correctos()
        {
            var mp = new MetodoPago();

            mp.Id.Should().Be(0);
            mp.Nombre.Should().BeEmpty();
            mp.Categoria.Should().Be("Otro");
            mp.Activo.Should().BeTrue();
            mp.Id_empresa.Should().Be(0);
            mp.Pagos.Should().BeEmpty();
        }

        [Fact]
        public void Propiedades_SeAsignanCorrectamente()
        {
            var mp = new MetodoPago
            {
                Id = 1,
                Nombre = "Efectivo",
                Categoria = "Efectivo",
                Activo = true,
                Id_empresa = 1
            };

            mp.Id.Should().Be(1);
            mp.Nombre.Should().Be("Efectivo");
            mp.Categoria.Should().Be("Efectivo");
            mp.Id_empresa.Should().Be(1);
        }

        [Fact]
        public void Categoria_PermiteCualquierValor()
        {
            var mp = new MetodoPago();
            mp.Categoria = "QR";
            mp.Categoria.Should().Be("QR");

            mp.Categoria = "Cripto";
            mp.Categoria.Should().Be("Cripto");

            mp.Categoria = "";
            mp.Categoria.Should().Be("");
        }
    }
}
