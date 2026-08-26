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
            mp.Subcategoria.Should().BeNull();
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

        [Fact]
        public void Subcategoria_DefaultEsNulo()
        {
            var mp = new MetodoPago();
            mp.Subcategoria.Should().BeNull();
        }

        [Fact]
        public void Subcategoria_SeAsignaCorrectamente()
        {
            var mp = new MetodoPago();
            mp.Subcategoria = "Debito";
            mp.Subcategoria.Should().Be("Debito");

            mp.Subcategoria = "Credito";
            mp.Subcategoria.Should().Be("Credito");
        }

        [Fact]
        public void Subcategoria_EsIndependienteDeCategoria()
        {
            var mp = new MetodoPago { Categoria = "Tarjeta", Subcategoria = "Debito" };

            mp.Categoria = "Efectivo";

            mp.Subcategoria.Should().Be("Debito");
            mp.Categoria.Should().Be("Efectivo");
        }

        [Fact]
        public void Semilla_Debito_TieneSubcategoriaDebito()
        {
            var seed = new MetodoPago { Id = 2, Nombre = "Débito", Categoria = "Tarjeta", Subcategoria = "Debito" };
            seed.Subcategoria.Should().Be("Debito");
            seed.Categoria.Should().Be("Tarjeta");
        }

        [Fact]
        public void Semilla_Credito_TieneSubcategoriaCredito()
        {
            var seed = new MetodoPago { Id = 3, Nombre = "Crédito", Categoria = "Tarjeta", Subcategoria = "Credito" };
            seed.Subcategoria.Should().Be("Credito");
            seed.Categoria.Should().Be("Tarjeta");
        }

        [Fact]
        public void Semilla_Efectivo_SubcategoriaEsNulo()
        {
            var seed = new MetodoPago { Id = 1, Nombre = "Efectivo", Categoria = "Efectivo", Subcategoria = null };
            seed.Subcategoria.Should().BeNull();
        }

        [Fact]
        public void Semilla_Transferencia_SubcategoriaEsNulo()
        {
            var seed = new MetodoPago { Id = 4, Nombre = "Transferencia", Categoria = "Transferencia", Subcategoria = null };
            seed.Subcategoria.Should().BeNull();
        }

        [Fact]
        public void Semilla_MercadoPago_SubcategoriaEsNulo()
        {
            var seed = new MetodoPago { Id = 5, Nombre = "Mercado Pago", Categoria = "Otro", Subcategoria = null };
            seed.Subcategoria.Should().BeNull();
        }
    }
}
