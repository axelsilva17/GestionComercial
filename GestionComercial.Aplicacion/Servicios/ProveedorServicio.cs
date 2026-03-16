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
            await _uow.Proveedores.AgregarAsync(proveedor);
            await _uow.GuardarCambiosAsync();
            return proveedor;
        }

        public async Task ActualizarAsync(Proveedor proveedor)
        {
            _uow.Proveedores.Actualizar(proveedor);
            await _uow.GuardarCambiosAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var proveedor = await _uow.Proveedores.ObtenerPorIdAsync(id)
                ?? throw new KeyNotFoundException($"Proveedor {id} no encontrado");
            proveedor.Activo = false;
            _uow.Proveedores.Actualizar(proveedor);
            await _uow.GuardarCambiosAsync();
        }
    }
}
