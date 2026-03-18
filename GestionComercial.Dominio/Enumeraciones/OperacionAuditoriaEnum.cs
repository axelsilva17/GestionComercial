namespace GestionComercial.Dominio.Entidades.Auditoria
{
    /// <summary>
    /// Tipos de operaciones que se registran en la auditoría.
    /// </summary>
    public enum OperacionAuditoriaEnum
    {
        /// <summary>
        /// Inserción de un nuevo registro.
        /// </summary>
        Insert = 1,

        /// <summary>
        /// Actualización de un registro existente.
        /// </summary>
        Update = 2,

        /// <summary>
        /// Eliminación de un registro.
        /// </summary>
        Delete = 3
    }
}
