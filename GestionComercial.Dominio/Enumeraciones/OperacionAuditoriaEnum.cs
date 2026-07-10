namespace GestionComercial.Dominio.Entidades.Auditoria
{
    ///     /// Tipos de operaciones que se registran en la auditoría.
    public enum OperacionAuditoriaEnum
    {
        ///         /// Inserción de un nuevo registro.
        Insert = 1,

        ///         /// Actualización de un registro existente.
        Update = 2,

        ///         /// Eliminación de un registro.
        Delete = 3
    }
}
