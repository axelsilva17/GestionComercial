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
                tipo: TipoDescuentoEnum.Producto,
                valor: 15,
                idEmpresa: 1,
                idProducto: 42);

            descuento.Nombre.Should().Be("Leche 15%");
            descuento.Tipo.Should().Be(TipoDescuentoEnum.Producto);
            descuento.Valor.Should().Be(15);
            descuento.Id_empresa.Should().Be(1);
            descuento.Id_producto.Should().Be(42);
            descuento.Id_categoria.Should().BeNull();
            descuento.Activo.Should().BeTrue();
            descuento.EstaVigente.Should().BeTrue();
            descuento.Prioridad.Should().Be(0);
            descuento.ModoDescuento.Should().Be(ModoDescuentoEnum.Porcentaje);
        }

        [Fact]
        public void Crear_CategoriaValida_Success()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Lácteos 10%",
                tipo: TipoDescuentoEnum.Categoria,
                valor: 10,
                idEmpresa: 1,
                idCategoria: 5);

            descuento.Tipo.Should().Be(TipoDescuentoEnum.Categoria);
            descuento.Id_categoria.Should().Be(5);
            descuento.Id_producto.Should().BeNull();
        }

        [Fact]
        public void Crear_ValorCero_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 0,
                idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Crear_Valor101_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 101,
                idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Crear_ValorNegativo_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: -5,
                idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Crear_ProductoSinIdProducto_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: null);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Id_producto*");
        }

        [Fact]
        public void Crear_CategoriaSinIdCategoria_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Categoria, valor: 10,
                idEmpresa: 1, idCategoria: null);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*Id_categoria*");
        }

        [Fact]
        public void Crear_IdEmpresaCero_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 0, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*empresa*");
        }

        [Fact]
        public void Crear_FechasInvalidas_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1,
                fechaDesde: new DateTime(2026, 8, 20),
                fechaHasta: new DateTime(2026, 8, 10));

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*FechaHasta*");
        }

        [Fact]
        public void Crear_FechasNulas_Success()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1,
                fechaDesde: null, fechaHasta: null);

            descuento.FechaDesde.Should().BeNull();
            descuento.FechaHasta.Should().BeNull();
            descuento.EstaVigente.Should().BeTrue();
        }

        [Fact]
        public void EstaVigente_ActivoSinFechas_ReturnsTrue()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1);

            descuento.EstaVigente.Should().BeTrue();
        }

        [Fact]
        public void EstaVigente_Inactivo_ReturnsFalse()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1);
            descuento.Inactivar();

            descuento.EstaVigente.Should().BeFalse();
        }

        [Fact]
        public void EstaVigente_FueraDeRango_ReturnsFalse()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1,
                fechaHasta: DateTime.Now.AddDays(-1));

            descuento.EstaVigente.Should().BeFalse();
        }

        [Fact]
        public void Actualizar_ModificaCampos()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Viejo", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1);

            descuento.Actualizar("Nuevo", 25, null, null, 5);

            descuento.Nombre.Should().Be("Nuevo");
            descuento.Valor.Should().Be(25);
            descuento.Prioridad.Should().Be(5);
        }

        [Fact]
        public void Actualizar_ValorInvalido_Throws()
        {
            var descuento = DescuentoConfiguracion.Crear(
                nombre: "Test", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1);

            var act = () => descuento.Actualizar("Test", 0, null, null, 0);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*valor*");
        }

        [Fact]
        public void Crear_NombreVacio_Throws()
        {
            var act = () => DescuentoConfiguracion.Crear(
                nombre: "", tipo: TipoDescuentoEnum.Producto, valor: 10,
                idEmpresa: 1, idProducto: 1);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*nombre*");
        }
    }
}
