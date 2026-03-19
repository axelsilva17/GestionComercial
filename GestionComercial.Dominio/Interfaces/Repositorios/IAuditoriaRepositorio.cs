using GestionComercial.Dominio.Entidades.Auditoria;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestionComercial.Dominio.Interfaces.Repositorios
{
    /// <summary>
    /// Interface para el repositorio de auditoría.
    /// </summary>
    public interface IAuditoriaRepositorio
    {
        /// <summary>
        /// Obtiene todos los registros de auditoría en un rango de fechas.
        /// </summary>
        Task<IEnumerable<AuditoriaLog>> ObtenerPorRangoFechasAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Agrega un nuevo registro de auditoría.
        /// </summary>
        Task<AuditoriaLog> AgregarAsync(
            AuditoriaLog auditoriaLog,
            CancellationToken cancellationToken = default);
    }
}
