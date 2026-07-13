namespace GestionComercial.Aplicacion.DTOs.Proveedores
{
    /// DTO liviano para listado y ComboBox.
    public class ProveedorItemDto
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public int TotalCompras { get; set; }
        public string Inicial => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }

    /// DTO completo para el formulario de detalle.
    public class ProveedorDto
    {
        public int    IdProveedor  { get; set; }
        public string Nombre       { get; set; } = string.Empty;
        public string Telefono     { get; set; } = string.Empty;
        public string Email        { get; set; } = string.Empty;
        public bool   Activo       { get; set; }
        public int    IdEmpresa    { get; set; }
        public int    TotalCompras { get; set; }
    }

    /// DTO para crear un proveedor nuevo.
    public class ProveedorCrearDto
    {
        public string Nombre    { get; set; } = string.Empty;
        public string Telefono  { get; set; } = string.Empty;
        public string Email     { get; set; } = string.Empty;
        public int    IdEmpresa { get; set; }
    }

    /// DTO para actualizar un proveedor existente.
    public class ProveedorActualizarDto
    {
        public int    IdProveedor { get; set; }
        public string Nombre      { get; set; } = string.Empty;
        public string Telefono    { get; set; } = string.Empty;
        public string Email       { get; set; } = string.Empty;
        public bool   Activo      { get; set; }
    }
}
