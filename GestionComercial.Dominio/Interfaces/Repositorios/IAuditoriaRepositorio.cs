using GestionComercial.Dominio.Entidades.Auditoria;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    ///     /// Interfaz para el repositorio de auditoría.
    public interface IAuditoriaRepositorio
    {
        ///         /// Registra un cambio en una entidad.
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

        ///         /// Obtiene el historial de auditoría de una tabla específica.
        Task<IEnumerable<AuditoriaLog>> ObtenerPorTablaYRegistroAsync(
            string nombreTabla,
            int registroId);

        ///         /// Obtiene el historial de auditoría de un usuario.
        Task<IEnumerable<AuditoriaLog>> ObtenerPorUsuarioAsync(
            int idUsuario,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        ///         /// Obtiene el historial de auditoría de una empresa.
        Task<IEnumerable<AuditoriaLog>> ObtenerPorEmpresaAsync(
            int idEmpresa,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        ///         /// Obtiene el historial de auditoría de una sucursal.
        Task<IEnumerable<AuditoriaLog>> ObtenerPorSucursalAsync(
            int idSucursal,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        ///         /// Obtiene los cambios en la tabla Caja.
        Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaCajaAsync(
            int? idCaja = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        ///         /// Obtiene los cambios en la tabla MovimientoCaja.
        Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaMovimientoCajaAsync(
            int? idMovimiento = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        ///         /// Obtiene auditoría filtrada por múltiples criterios (todos los registros).
        Task<IEnumerable<AuditoriaLog>> ObtenerAuditoriaFiltradaAsync(
            int? idUsuario = null,
            int? tipoOperacion = null,
            string? nombreTabla = null,
            DateTime? fechaDesde = null,
            DateTime? fechaHasta = null);

        ///         /// Obtiene auditoría filtrada con paginación en SQL (sin materializar todo).
        /// Devuelve los items de la página + el total de registros.
        Task<(IEnumerable<AuditoriaLog> Items, int Total)> ObtenerAuditoriaPaginadaAsync(
            int? idUsuario,
            int? tipoOperacion,
            string? nombreTabla,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina,
            int tamanioPagina);
    }
}
