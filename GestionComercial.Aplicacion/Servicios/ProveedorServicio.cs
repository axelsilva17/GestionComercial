using GestionComercial.Aplicacion.Interfaces;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ProveedorServicio : IProveedorServicio
    {
        private readonly IUnitOfWork _uow;
        public ProveedorServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<Proveedor>> ObtenerTodosAsync(int idEmpresa)
            => await _uow.Proveedores.ObtenerPorEmpresaAsync(idEmpresa);

        public async Task<Proveedor?> ObtenerPorIdAsync(int id)
            => await _uow.Proveedores.ObtenerPorIdAsync(id);

        public async Task<Proveedor> CrearAsync(Proveedor proveedor)
        {
            // ── Crear con factory method (DDD) ──
            var nuevo = Proveedor.Crear(
                nombre: proveedor.Nombre,
                idEmpresa: proveedor.Id_empresa,
                telefono: proveedor.Telefono,
                email: proveedor.Email,
                cuit: proveedor.CUIT
            );

            await _uow.Proveedores.AgregarAsync(nuevo);
            await _uow.GuardarCambiosAsync();
            return nuevo;
        }

        public async Task ActualizarAsync(Proveedor proveedor)
        {
            // ── Usar método de dominio (DDD) ──
            proveedor.Actualizar(
                nombre: proveedor.Nombre,
                telefono: proveedor.Telefono,
                email: proveedor.Email,
                cuit: proveedor.CUIT
            );
            _uow.Proveedores.Actualizar(proveedor);
            await _uow.GuardarCambiosAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var proveedor = await _uow.Proveedores.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Proveedor {id} no encontrado");
            
            // ── Usar método de dominio (DDD) ──
            proveedor.Inactivar();
            _uow.Proveedores.Actualizar(proveedor);
            await _uow.GuardarCambiosAsync();
        }
    }
}