namespace GestionComercial.Aplicacion.DTOs.Configuracion
{
    // ── EMPRESA ──────────────────────────────────────────────────────────────
    public class EmpresaDto
    {
        public int    IdEmpresa  { get; set; }
        public string Nombre     { get; set; } = string.Empty;
        public string CUIT       { get; set; } = string.Empty;
        public string Direccion  { get; set; } = string.Empty;
        public string? Email     { get; set; }
        public string? Telefono  { get; set; }
        public string? LogoUrl   { get; set; }
        public bool   Activa     { get; set; }

        public bool CUITValido => !string.IsNullOrEmpty(CUIT)
            && System.Text.RegularExpressions.Regex.IsMatch(CUIT, @"^\d{2}-\d{8}-\d{1}$");
    }

    // ── SUCURSAL ─────────────────────────────────────────────────────────────
    public class SucursalDto
    {
        public int    IdSucursal { get; set; }
        public string Nombre     { get; set; } = string.Empty;
        public string Direccion  { get; set; } = string.Empty;
        public bool   Activa     { get; set; }
        public int    IdEmpresa  { get; set; }
        public string RolNombre  { get; set; } = string.Empty;
        public string EstadoTexto => Activa ? "Activa" : "Inactiva";
    }

    // ── ROL ──────────────────────────────────────────────────────────────────
    public class RolDto
    {
        public int    IdRol  { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    // ── MÉTODO DE PAGO ───────────────────────────────────────────────────────
    public class MetodoPagoDto
    {
        public int    IdMetodoPago { get; set; }
        public string Nombre       { get; set; } = string.Empty;
        public string Categoria    { get; set; } = "Otro";
        public int    IdEmpresa    { get; set; }
        public string Icono        => Categoria switch
        {
            "Efectivo"      => "💵",
            "Tarjeta"       => "💳",
            "Transferencia" => "🏦",
            _               => "💳"
        };
        public string TipoTexto    => Categoria switch
        {
            "Efectivo"      => "Efectivo",
            "Tarjeta"       => "Tarjeta",
            "Transferencia" => "Transferencia",
            _               => "Otro"
        };
    }

    // ── PERFIL ───────────────────────────────────────────────────────────────
    public class CambiarPasswordDto
    {
        public int    IdUsuario          { get; set; }
        public string PasswordActual     { get; set; } = string.Empty;
        public string PasswordNuevo      { get; set; } = string.Empty;
        public string PasswordConfirmar  { get; set; } = string.Empty;
    }
}
