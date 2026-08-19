using FluentAssertions;
using GestionComercial.Dominio.Entidades.Descuento;

namespace GestionComercial.Tests.Dominio
{
    public class DescuentoConfiguracionTests
    {
        [Fact]
        public void Crear_ProductoValido_Success()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Leche 15%",
                valor: 15,
                idEmpresa: 1,
                idProducto: 42);

            descuento.Nombre.Should().Be("Leche 15%");
            descuento.Valor.Should().Be(15);
            descuento.Id_empresa.Should().Be(1);
            descuento.Id_producto.Should().Be(42);
            descuento.Id_categoria.Should().BeNull();
            descuento.AplicaCualquierMetodoPago.Should().BeTrue();
            descuento.Activo.Should().BeTrue();
            descuento.EstaVigente.Should().BeTrue();
            descuento.ModoDescuento.Should().Be(ModoDescuentoEnum.Porcentaje);
        }

        [Fact]
        public void Crear_CategoriaValida_Success()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Lácteos 10%",
                valor: 10,
                idEmpresa: 1,
                idCategoria: 5);

            descuento.Id_categoria.Should().Be(5);
            descuento.Id_producto.Should().BeNull();
            descuento.AplicaCualquierMetodoPago.Should().BeTrue();
        }

        [Fact]
        public void Crear_ProductoConMetodosEspecificos_Success()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Visa 10%",
                valor: 10,
                idEmpresa: 1,
                idProducto: 42,
                aplicaCualquierMetodoPago: false,
                idsMetodosPago: new List<int> { 10, 11 });

            descuento.AplicaCualquierMetodoPago.Should().BeFalse();
            descuento.Id_producto.Should().Be(42);
        }

        [Fact]
        public void Crear_ValorCero_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 0, idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Crear_Valor101_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 101, idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Crear_ValorNegativo_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: -5, idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Crear_SinScope_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*producto o categoría*");
        }

        [Fact]
        public void Crear_MetodoPago_SinProductoCategoria_Ok()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Visa 5%", valor: 5, idEmpresa: 1,
                idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 10 },
                alcance: AlcanceDescuentoEnum.MetodoPago);

            descuento.Alcance.Should().Be(AlcanceDescuentoEnum.MetodoPago);
            descuento.Id_producto.Should().BeNull();
            descuento.Id_categoria.Should().BeNull();
            descuento.AplicaCualquierMetodoPago.Should().BeFalse();
        }

        [Fact]
        public void Crear_MetodoPago_ConAplicaCualquier_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Visa 5%", valor: 5, idEmpresa: 1,
                idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: true, idsMetodosPago: null,
                alcance: AlcanceDescuentoEnum.MetodoPago);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*no puede aplicar a cualquier método*");
        }

        [Fact]
        public void Crear_MetodoPago_ConProducto_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Visa 5%", valor: 5, idEmpresa: 1,
                idProducto: 42, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int> { 10 },
                alcance: AlcanceDescuentoEnum.MetodoPago);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*no puede asignar producto o categoría*");
        }

        [Fact]
        public void Crear_MetodoPago_SinMetodos_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Visa 5%", valor: 5, idEmpresa: 1,
                idProducto: null, idCategoria: null,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int>(),
                alcance: AlcanceDescuentoEnum.MetodoPago);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*al menos un método*");
        }

        [Fact]
        public void Crear_ProductoYCategoriaJuntos_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1,
                idProducto: 1, idCategoria: 2);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*simultáneamente*");
        }

        [Fact]
        public void Crear_SinMetodosPagoYNoAplicaCualquiera_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1,
                aplicaCualquierMetodoPago: false, idsMetodosPago: new List<int>());

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*cualquier método o seleccionar al menos una tarjeta*");
        }

        [Fact]
        public void Crear_IdEmpresaCero_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 0, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*empresa*");
        }

        [Fact]
        public void Crear_FechasInvalidas_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1,
                fechaDesde: new DateTime(2026, 8, 20),
                fechaHasta: new DateTime(2026, 8, 10));

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*FechaHasta*");
        }

        [Fact]
        public void Crear_FechasNulas_Success()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1,
                fechaDesde: null, fechaHasta: null);

            descuento.FechaDesde.Should().BeNull();
            descuento.FechaHasta.Should().BeNull();
            descuento.EstaVigente.Should().BeTrue();
        }

        [Fact]
        public void EstaVigente_ActivoSinFechas_ReturnsTrue()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1);

            descuento.EstaVigente.Should().BeTrue();
        }

        [Fact]
        public void EstaVigente_Inactivo_ReturnsFalse()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1);
            descuento.Inactivar();

            descuento.EstaVigente.Should().BeFalse();
        }

        [Fact]
        public void EstaVigente_FueraDeRango_ReturnsFalse()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1,
                fechaHasta: DateTime.Now.AddDays(-1));

            descuento.EstaVigente.Should().BeFalse();
        }

        [Fact]
        public void Actualizar_ModificaCampos()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Viejo", valor: 10, idEmpresa: 1, idProducto: 1);

            descuento.Actualizar("Nuevo", 25, 1, null, false, null, null);

            descuento.Nombre.Should().Be("Nuevo");
            descuento.Valor.Should().Be(25);
            descuento.Id_producto.Should().Be(1);
            descuento.AplicaCualquierMetodoPago.Should().BeFalse();
        }

        [Fact]
        public void Actualizar_CambiaScopeACategoria()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Original", valor: 10, idEmpresa: 1, idProducto: 1);

            descuento.Actualizar("Cambiado", 20, null, 5, true, null, null);

            descuento.Id_categoria.Should().Be(5);
            descuento.Id_producto.Should().BeNull();
            descuento.Valor.Should().Be(20);
        }

        [Fact]
        public void Actualizar_ValorInvalido_Throws()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1);

            var act = () => descuento.Actualizar("Test", 0, 1, null, true, null, null);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Actualizar_SinScope_Throws()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1);

            var act = () => descuento.Actualizar("Test", 10, null, null, true, null, null);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*producto o categoría*");
        }

        [Fact]
        public void Actualizar_MetodoPago_ConProducto_Throws()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1);

            var act = () => descuento.Actualizar(
                "Nuevo", 10, 1, null, false, null, null,
                alcance: AlcanceDescuentoEnum.MetodoPago);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*no puede asignar producto o categoría*");
        }

        [Fact]
        public void Actualizar_MetodoPago_SinProductoCategoria_Ok()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1);

            descuento.Actualizar(
                "Visa 5%", 5, null, null, false, null, null,
                alcance: AlcanceDescuentoEnum.MetodoPago);

            descuento.Alcance.Should().Be(AlcanceDescuentoEnum.MetodoPago);
            descuento.Id_producto.Should().BeNull();
            descuento.Id_categoria.Should().BeNull();
            descuento.AplicaCualquierMetodoPago.Should().BeFalse();
        }

        [Fact]
        public void Actualizar_ProductoYCategoriaJuntos_Throws()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", valor: 10, idEmpresa: 1, idProducto: 1);

            var act = () => descuento.Actualizar("Test", 10, 1, 2, true, null, null);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*simultáneamente*");
        }

        [Fact]
        public void Crear_NombreVacio_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "", valor: 10, idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*nombre*");
        }
    }
}