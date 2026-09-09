using FluentAssertions;
using GestionComercial.Aplicacion.DTOs.Usuarios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Enumeraciones;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Regresión E2E — Caja: el movimiento manual de ingreso/egreso debe persistir con
    /// Id_usuario. Bug original: Id_usuario NOT NULL sin asignar → FOREIGN KEY constraint failed.
    /// </summary>
    public class CajaRegresionTests : IClassFixture<SqliteTestDatabase>
    {
        private readonly SqliteTestDatabase _db;

        public CajaRegresionTests(SqliteTestDatabase db) => _db = db;

        private async Task<(int CajaId, int UsuarioId, int EmpresaId, int SucursalId)> SembrarCajaAbiertaAsync()
        {
            await using var ctx = _db.CreateContext();
            var escenario = await EscenarioIntegracion.SembrarOrganizacionAsync(ctx);
            var caja = Caja.Crear(escenario.SucursalId, escenario.UsuarioId, 1000m);
            ctx.Cajas.Add(caja);
            await ctx.SaveChangesAsync();
            return (caja.Id, escenario.UsuarioId, escenario.EmpresaId, escenario.SucursalId);
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_ConSesion_PersisteMovimientoConIdUsuarioDeSesion()
        {
            var (cajaId, usuarioId, empresaId, sucursalId) = await SembrarCajaAbiertaAsync();

            var sesion = new SesionServicio();
            sesion.IniciarSesion(new UsuarioSesionDto
            {
                IdUsuario = usuarioId,
                Nombre = "Ana",
                Apellido = "Prueba",
                IdEmpresa = empresaId,
                IdSucursal = sucursalId
            });

            using var uow = _db.CreateUnitOfWork();
            var servicio = new CajaServicio(uow, sesion);
            await servicio.RegistrarMovimientoAsync(cajaId, TipoMovimientoCajaEnum.Ingreso, 500m, "Ingreso regresión");

            using var ctx = _db.CreateContext();
            var movimiento = ctx.MovimientosCaja.Single(m => m.Id_caja == cajaId);
            movimiento.Id_usuario.Should().Be(usuarioId);
            movimiento.Monto.Should().Be(500m);
            movimiento.Tipo.Should().Be((int)TipoMovimientoCajaEnum.Ingreso);
        }

        [Fact]
        public async Task RegistrarMovimientoAsync_SinSesionUsaUsuarioQueAbrioLaCaja()
        {
            var (cajaId, usuarioId, _, _) = await SembrarCajaAbiertaAsync();

            // Sesión vacía (dev/contexto sin usuario real): fallback a UsuarioApertura_id
            using var uow = _db.CreateUnitOfWork();
            var servicio = new CajaServicio(uow, new SesionServicio());
            await servicio.RegistrarMovimientoAsync(cajaId, TipoMovimientoCajaEnum.Egreso, 200m, "Egreso regresión");

            using var ctx = _db.CreateContext();
            var movimiento = ctx.MovimientosCaja.Single(m => m.Id_caja == cajaId);
            movimiento.Id_usuario.Should().Be(usuarioId);
            movimiento.Monto.Should().Be(200m);
            movimiento.Tipo.Should().Be((int)TipoMovimientoCajaEnum.Egreso);
        }
    }
}