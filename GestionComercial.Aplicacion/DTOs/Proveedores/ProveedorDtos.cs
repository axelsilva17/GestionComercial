namespace GestionComercial.Aplicacion.DTOs.Proveedores
{
    /// <summary>DTO liviano para listado y ComboBox.</summary>
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

    /// <summary>DTO completo para el formulario de detalle.</summary>
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

    /// <summary>DTO para crear un proveedor nuevo.</summary>
    public class ProveedorCrearDto
    {
        public string Nombre    { get; set; } = string.Empty;
        public string Telefono  { get; set; } = string.Empty;
        public string Email     { get; set; } = string.Empty;
        public int    IdEmpresa { get; set; }
    }

    /// <summary>DTO para actualizar un proveedor existente.</summary>
    public class ProveedorActualizarDto
    {
        public int    IdProveedor { get; set; }
        public string Nombre      { get; set; } = string.Empty;
        public string Telefono    { get; set; } = string.Empty;
        public string Email       { get; set; } = string.Empty;
        public bool   Activo      { get; set; }
    }
}
