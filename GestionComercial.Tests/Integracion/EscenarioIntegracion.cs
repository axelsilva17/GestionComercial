using GestionComercial.Dominio.Entidades.Organizacion;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Seguridad;
using GestionComercial.Persistencia.Contexto;

namespace GestionComercial.Tests.Integracion
{
    /// <summary>
    /// Ids del escenario base (organización) sembrado para los tests de integración.
    /// </summary>
    internal sealed record EscenarioBase(
        int EmpresaId,
        int SucursalId,
        int RolId,
        int UsuarioId,
        int CategoriaId,
        int UnidadMedidaId);

    /// <summary>
    /// Helper para sembrar entidades PROPIAS, aisladas de las seeds HasData del modelo
    /// (las seeds usan empresa/sucursal/usuario/categorías/unidades con ids fijos).
    /// </summary>
    internal static class EscenarioIntegracion
    {
        // Sufijo único por llamada: el fixture se comparte entre los tests de una misma
        // clase (IClassFixture) y varias columnas tienen UNIQUE (Empresa.CUIT, Rol.Nombre,
        // Usuario.Email, Producto.CodigoBarra).
        private static int _contador;

        public static async Task<EscenarioBase> SembrarOrganizacionAsync(GestionComercialContext ctx)
        {
            var n = Interlocked.Increment(ref _contador);
            var cuit = $"30-{n:D8}-9";
            var empresa = Empresa.Crear("Empresa Regresión", cuit, "Av. Test 123");
            ctx.Empresas.Add(empresa);
            await ctx.SaveChangesAsync();

            var rol = new Rol { Nombre = $"Rol Regresión {n}" };
            ctx.Roles.Add(rol);
            await ctx.SaveChangesAsync();

            var sucursal = Sucursal.Crear("Sucursal Regresión", "Calle 456", empresa.Id);
            ctx.Sucursales.Add(sucursal);
            await ctx.SaveChangesAsync();

            var unidad = UnidadMedida.Crear("Unidad", "UN");
            var categoria = Categoria.Crear("Categoría Regresión", empresa.Id);
            ctx.UnidadesMedida.Add(unidad);
            ctx.Categorias.Add(categoria);
            await ctx.SaveChangesAsync();

            var usuario = Usuario.Crear("Ana", "Prueba", $"ana{n:D4}@regresion.test", "hash", sucursal.Id, rol.Id);
            ctx.Usuarios.Add(usuario);
            await ctx.SaveChangesAsync();

            return new EscenarioBase(empresa.Id, sucursal.Id, rol.Id, usuario.Id, categoria.Id, unidad.Id);
        }
    }
}