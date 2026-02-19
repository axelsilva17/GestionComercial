namespace GestionComercial.Aplicacion.DTOs.Clientes
{
    public class ClienteDto
    {
        public int    Id        { get; set; }
        public string Nombre    { get; set; }
        public int    Documento { get; set; }
        public string Telefono  { get; set; }
        public string Email     { get; set; }
        public bool   Activo    { get; set; }
        public int    IdEmpresa { get; set; }
        public string Inicial   => string.IsNullOrEmpty(Nombre) ? "?" : Nombre[0].ToString().ToUpper();
    }
}
