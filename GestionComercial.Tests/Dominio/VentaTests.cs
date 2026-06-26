using FluentAssertions;
using GestionComercial.Dominio.Entidades.Pagos;
using GestionComercial.Dominio.Entidades.Ventas;
using ProdEntity = GestionComercial.Dominio.Entidades.Producto.Producto;

namespace GestionComercial.Tests.Dominio
{
    public class VentaTests
    {
        // ═══════════════════════════════════════════════════════════
        // Factory method: Venta.Crear()
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void Crear_ConCaja_PropiedadesCorrectas()
        {
            var venta = Venta.Crear(idSucursal: 1, idCliente: 2, idUsuario: 3, idCaja: 5);

            venta.Id_sucursal.Should().Be(1);
            venta.Id_cliente.Should().Be(2);
            venta.Id_usuario.Should().Be(3);
            venta.Id_caja.Should().Be(5);
            venta.Estado.Should().Be(1); // Pendiente
            venta.EsPendiente.Should().BeTrue();
            venta.PuedePagarse.Should().BeFalse(); // Sin detalles
            venta.PuedeModificarse.Should().BeTrue();
            venta.TotalBruto.Should().Be(0);
            venta.TotalFinal.Should().Be(0);
        }

        [Fact]
        public void Crear_SinCaja_IdCajaEsNull()
        {
            var venta = Venta.Crear(idSucursal: 1, idCliente: 1, idUsuario: 1);

            venta.Id_caja.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Crear_IdSucursalInvalido_LanzaExcepcion(int idInvalido)
        {
            var act = () => Venta.Crear(idInvalido, idCliente: 1, idUsuario: 1);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ID de sucursal inválido*");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Crear_IdClienteInvalido_LanzaExcepcion(int idInvalido)
        {
            var act = () => Venta.Crear(idSucursal: 1, idInvalido, idUsuario: 1);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ID de cliente inválido*");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Crear_IdUsuarioInvalido_LanzaExcepcion(int idInvalido)
        {
            var act = () => Venta.Crear(idSucursal: 1, idCliente: 1, idInvalido);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*ID de usuario inválido*");
        }

        // ═══════════════════════════════════════════════════════════
        // AgregarDetalle + RecalcularTotales
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void AgregarDetalle_DetalleValido_ActualizaTotales()
        {
            var venta = Venta.Crear(1, 1, 1);
            var detalle = CrearDetalle(precio: 100m, costo: 50m, cantidad: 2);

            venta.AgregarDetalle(detalle);

            venta.Detalles.Should().HaveCount(1);
            venta.TotalBruto.Should().Be(200m);   // 100 * 2
            venta.TotalDescuento.Should().Be(0);
            venta.TotalFinal.Should().Be(200m);
            venta.PuedePagarse.Should().BeTrue();
        }

        [Fact]
        public void AgregarDetalle_MultiplesDetalles_SumaCorrectamente()
        {
            var venta = Venta.Crear(1, 1, 1);

            venta.AgregarDetalle(CrearDetalle(100m, 50m, 2));  // subtotal 200
            venta.AgregarDetalle(CrearDetalle(50m, 20m, 3));   // subtotal 150

            venta.TotalBruto.Should().Be(350m);
            venta.TotalFinal.Should().Be(350m);
            venta.Detalles.Should().HaveCount(2);
        }

        [Fact]
        public void AgregarDetalle_DetalleNulo_LanzaExcepcion()
        {
            var venta = Venta.Crear(1, 1, 1);
            var act = () => venta.AgregarDetalle(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AgregarDetalle_VentaPagada_LanzaInvalidOperation()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));

            // Pagar con factory method de estado (para testing)
            venta.MarcarPagada();

            var act = () => venta.AgregarDetalle(CrearDetalle(50m, 20m, 1));
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*No se puede modificar una venta pagada*");
        }

        [Fact]
        public void AgregarDetalle_VentaAnulada_LanzaInvalidOperation()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));
            venta.Anular("Test de anulación", 1);

            var act = () => venta.AgregarDetalle(CrearDetalle(50m, 20m, 1));
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*No se puede modificar una venta anulada*");
        }

        // ═══════════════════════════════════════════════════════════
        // QuitarDetalle
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void QuitarDetalle_DetalleExistente_RecalculaCorrectamente()
        {
            var venta = Venta.Crear(1, 1, 1);
            var detalle = CrearDetalle(100m, 50m, 2);
            venta.AgregarDetalle(detalle);
            detalle.GetType().GetProperty("Id")!.SetValue(detalle, 1); // Simular ID asignado

            venta.AgregarDetalle(CrearDetalle(50m, 20m, 1)); // subtotal 50

            venta.TotalBruto.Should().Be(250m);

            venta.QuitarDetalle(1);

            venta.Detalles.Should().HaveCount(1);
            venta.TotalBruto.Should().Be(50m);
        }

        // ═══════════════════════════════════════════════════════════
        // MarcarPagada
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void MarcarPagada_VentaPendiente_CambiaEstado()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.EsPagada.Should().BeFalse();

            venta.MarcarPagada();

            venta.EsPagada.Should().BeTrue();
            venta.EsPendiente.Should().BeFalse();
            venta.Estado.Should().Be(2); // Pagada
        }

        [Fact]
        public void MarcarPagada_VentaYaPagada_LanzaExcepcion()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));
            venta.MarcarPagada();

            var act = () => venta.MarcarPagada();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void MarcarPagada_VentaAnulada_LanzaExcepcion()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));
            venta.Anular("Motivo", 1);

            var act = () => venta.MarcarPagada();
            act.Should().Throw<InvalidOperationException>();
        }

        // ═══════════════════════════════════════════════════════════
        // Anular
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void Anular_VentaPendiente_CamposCorrectos()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.Anular("Cliente no vino", idUsuarioAnulacion: 3);

            venta.EsAnulada.Should().BeTrue();
            venta.MotivoAnulacion.Should().Be("Cliente no vino");
            venta.UsuarioAnulacionId.Should().Be(3);
            venta.FechaAnulacion.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Anular_VentaYaAnulada_LanzaExcepcion()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.Anular("Primera vez", 1);

            var act = () => venta.Anular("Segunda vez", 2);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*La venta ya está anulada*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Anular_MotivoVacio_LanzaExcepcion(string? motivoInvalido)
        {
            var venta = Venta.Crear(1, 1, 1);
            var act = () => venta.Anular(motivoInvalido!, 1);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*motivo de anulación es requerido*");
        }

        // ═══════════════════════════════════════════════════════════
        // MarcarPendiente
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void MarcarPendiente_VentaPagada_RevierteEstado()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.MarcarPagada();

            venta.MarcarPendiente();

            venta.EsPendiente.Should().BeTrue();
            venta.Estado.Should().Be(1);
        }

        [Fact]
        public void MarcarPendiente_VentaAnulada_LanzaExcepcion()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.Anular("Motivo", 1);

            var act = () => venta.MarcarPendiente();
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*No se puede revertir una venta anulada*");
        }

        // ═══════════════════════════════════════════════════════════
        // Propiedades calculadas
        // ═══════════════════════════════════════════════════════════

        [Fact]
        public void TotalPagado_SinPagos_EsCero()
        {
            // EstaPagada = TotalPagado >= TotalFinal.
            // Con ambos en 0: 0 >= 0 = true (comportamiento actual del dominio).
            var venta = Venta.Crear(1, 1, 1);
            venta.TotalPagado.Should().Be(0);
            venta.TotalFinal.Should().Be(0);
            venta.EstaPagada.Should().BeTrue(); // 0 >= 0
        }

        [Fact]
        public void Cambio_CuandoPagoSuperaTotal_CalculaCorrectamente()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.AgregarDetalle(CrearDetalle(precio: 100m, costo: 50m, cantidad: 1));
            venta.TotalFinal.Should().Be(100m);

            // Simular pago mayor al total
            var pago = Pago.Crear(150m, 1, 1);
            venta.AgregarPago(pago);

            venta.TotalPagado.Should().Be(150m);
            venta.EstaPagada.Should().BeTrue();
            venta.Cambio.Should().Be(50m);
        }

        [Fact]
        public void Cambio_CuandoPagoEsExacto_EsCero()
        {
            var venta = Venta.Crear(1, 1, 1);
            venta.AgregarDetalle(CrearDetalle(100m, 50m, 1));

            var pago = Pago.Crear(100m, 1, 1);
            venta.AgregarPago(pago);

            venta.TotalPagado.Should().Be(100m);
            venta.EstaPagada.Should().BeTrue();
            venta.Cambio.Should().Be(0);
        }

        // ═══════════════════════════════════════════════════════════
        // Helpers
        // ═══════════════════════════════════════════════════════════

        private static VentaDetalle CrearDetalle(decimal precio, decimal costo, int cantidad)
        {
            var producto = new ProdEntity
            {
                Id = 1,
                Nombre = "Producto Test",
                PrecioVentaActual = precio,
                PrecioCostoActual = costo
            };
            return VentaDetalle.Crear(producto, cantidad, precio, costo, descuentoPorItem: 0);
        }

        // Helper para VentaDetalleDescuento (cuando se necesita)
        private static VentaDetalleDescuento CrearDescuento(decimal monto, string descripcion)
        {
            return VentaDetalleDescuento.PorMonto(monto, idDetalle: 0, descripcion);
        }
    }
}
