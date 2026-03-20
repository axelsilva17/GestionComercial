using GestionComercial.Dominio.Entidades.Auditoria;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    /// <summary>
    /// Interfaz para el repositorio de auditoría.
    /// </summary>
    public interface IAuditoriaRepositorio
    {
        /// <summary>
        /// Registra un cambio en una entidad.
        /// </summary>
        Task RegistrarAuditoriaAsync(
            string nombreTabla,
            int registroId,
            OperacionAuditoriaEnum tipoOperacion,
            int? idUsuario,
            string? nombreUsuario,
            string? valoresAnteriores,
            string? valoresNuevos,
            string? workstation = null,
            int? idEmpresa = null,
            int? idSucursal = null);

        /// <summary>
        /// Obtiene el historial de auditoría de una tabla específica.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerPorTablaYRegistroAsync(
            string nombreTabla,
            int registroId);

        /// <summary>
        /// Obtiene el historial de auditoría de un usuario.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerPorUsuarioAsync(
            int idUsuario,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        /// <summary>
        /// Obtiene el historial de auditoría de una empresa.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerPorEmpresaAsync(
            int idEmpresa,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        /// <summary>
        /// Obtiene el historial de auditoría de una sucursal.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerPorSucursalAsync(
            int idSucursal,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        /// <summary>
        /// Obtiene los cambios en la tabla Caja.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaCajaAsync(
            int? idCaja = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        /// <summary>
        /// Obtiene los cambios en la tabla MovimientoCaja.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaMovimientoCajaAsync(
            int? idMovimiento = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        /// <summary>
        /// Obtiene auditoría filtrada por múltiples criterios.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaFiltradaAsync(
            int? idUsuario = null,
            int? tipoOperacion = null,
            string? nombreTabla = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);
    }
}
