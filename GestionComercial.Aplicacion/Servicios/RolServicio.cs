using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class RolServicio : IRolServicio
    {
        private readonly IUnitOfWork _uow;

        public RolServicio(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<RolListDto>> ObtenerRolesAsync()
        {
            var roles = await _uow.Roles.ObtenerTodosConPermisosAsync();
            return roles.Select(r => new RolListDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                CantidadPermisos = r.RolPermisos?.Select(rp => rp.Id_permiso).Distinct().Count() ?? 0,
            }).ToList();
        }

        public async Task<List<PermisoDisponibleDto>> ObtenerPermisosDisponiblesAsync()
        {
            var permisos = await _uow.Permisos.ObtenerTodosAsync();
            return permisos.Select(p => new PermisoDisponibleDto
            {
                Id = p.Id,
                Codigo = p.Nombre,
                Descripcion = p.Descripcion ?? string.Empty,
                Modulo = p.Nombre.Contains('.')
                    ? p.Nombre.Split('.')[0]
                    : p.Nombre,
            }).ToList();
        }

        public async Task<List<int>> ObtenerPermisosPorRolAsync(int rolId)
        {
            var roles = await _uow.Roles.ObtenerTodosConPermisosAsync();
            var rol = roles.FirstOrDefault(r => r.Id == rolId);
            return rol?.RolPermisos?
                .Select(rp => rp.Id_permiso)
                .Distinct()
                .ToList() ?? new List<int>();
        }

        public async Task AsignarPermisosARolAsync(int rolId, List<int> permisoIds)
        {
            // La migración 20260909000000 crea el índice UNIQUE (Id_rol, Id_permiso):
            // deduplicar acá evita violaciones si un caller pasa ids repetidos.
            var idsUnicos = permisoIds.Distinct().ToList();
            await _uow.Roles.ActualizarPermisosRolAsync(rolId, idsUnicos);
        }
    }
}
