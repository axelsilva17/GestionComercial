using FluentAssertions;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regression — Descuentos: ActualizarAsync must PERSIST Nombre and Valor.
    /// Original bug: the entity was read via ObtenerPorIdAsync (AsNoTracking, detached),
    /// mutated, and never re-attached — GuardarCambiosAsync had nothing to update, so
    /// edits silently vanished on reload.
    /// Fix: DescuentoConfiguracionRepositorio.Actualizar(descuento) re-attaches the
    /// entity before GuardarCambiosAsync (same pattern already used by Eliminar/Activar).
    /// </summary>
    public class DescuentoRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public DescuentoRegresionTests(SqliteTestDatabase db) => _db = db;

        [Fact]
        public async Task ActualizarAsync_ModificaNombreYValor_YLosPersiste()
        {
            // Arrange: descuento propio de scope Categoría (aplica a cualquier método)
            int empresaId, descuentoId, categoriaId;

            await using (var setup = _db.CreateContext())
            {
                var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(setup);
                empresaId = escenario.EmpresaId;
                categoriaId = escenario.CategoriaId;

                var descuento = DescuentoConfiguracion.Crear(
                    nombre: "Descuento Original",
                    valor: 10m,
                    idEmpresa: escenario.EmpresaId,
                    idProducto: null,
                    idCategoria: categoriaId,
                    aplicaCualquierMetodoPago: true,
                    idsMetodosPago: null,
                    fechaDesde: null,
                    fechaHasta: null);
                setup.DescuentoConfiguraciones.Add(descuento);
                await setup.SaveChangesAsync();
                descuentoId = descuento.Id;
            }

            // Act: editar nombre + valor a través del servicio (sin sesión en tests)
            using (var uow = _db.CreateUnitOfWork())
            {
                var servicio = new DescuentoConfiguracionServicio(uow);
                await servicio.ActualizarAsync(
                    id: descuentoId,
                    nombre: "Descuento Editado",
                    valor: 15m,
                    idProducto: null,
                    idCategoria: categoriaId,
                    aplicaCualquierMetodoPago: true,
                    idsMetodosPago: null,
                    fechaDesde: null,
                    fechaHasta: null);
            }

            // Assert: los cambios sobrevivieron a la persistencia (contexto nuevo, sin tracking)
            using var verif = _db.CreateContext();
            var persistido = await verif.DescuentoConfiguraciones
                .AsNoTracking()
                .SingleAsync(d => d.Id == descuentoId);
            persistido.Nombre.Should().Be("Descuento Editado");
            persistido.Valor.Should().Be(15m);
        }
    }
}