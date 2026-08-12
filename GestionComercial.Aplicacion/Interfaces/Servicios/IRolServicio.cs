namespace GestionComercial.Aplicacion.Interfaces.Servicios
{
    /// <summary>
    /// DTO para listar roles con cantidad de permisos (no confundir con ConfiguracionDto.RolDto).
    /// </summary>
    public class RolListDto
    {
        public int    Id               { get; set; }
        public string Nombre           { get; set; } = string.Empty;
        public string? Descripcion     { get; set; }
        public int CantidadPermisos    { get; set; }
    }

    public class PermisoDisponibleDto
    {
        public int    Id          { get; set; }
        public string Codigo      { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Modulo      { get; set; } = string.Empty;
    }

    public interface IRolServicio
    {
        Task<List<RolListDto>> ObtenerRolesAsync();
        Task<List<PermisoDisponibleDto>> ObtenerPermisosDisponiblesAsync();
        Task<List<int>> ObtenerPermisosPorRolAsync(int rolId);
        Task AsignarPermisosARolAsync(int rolId, List<int> permisoIds);
    }
}
