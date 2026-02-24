namespace GestionComercial.Aplicacion.DTOs.Configuracion
{
    // ── EMPRESA ──────────────────────────────────────────────────────────────
    public class EmpresaDto
    {
        public int    IdEmpresa  { get; set; }
        public string Nombre     { get; set; } = string.Empty;
        public string CUIT       { get; set; } = string.Empty;
        public string Direccion  { get; set; } = string.Empty;
        public bool   Activa     { get; set; }
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
        public bool   EsEfectivo   { get; set; }
        public int    IdEmpresa    { get; set; }
        public string Icono        => EsEfectivo ? "💵" : "💳";
        public string TipoTexto    => EsEfectivo ? "Efectivo" : "Electrónico";
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
