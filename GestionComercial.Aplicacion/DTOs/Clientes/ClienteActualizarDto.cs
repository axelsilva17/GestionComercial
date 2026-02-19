namespace GestionComercial.Aplicacion.DTOs.Clientes
{
    public class ClienteActualizarDto
    {
        public int    Id        { get; set; }
        public string Nombre    { get; set; }
        public int    Documento { get; set; }
        public string Telefono  { get; set; }
        public string Email     { get; set; }
        public bool   Activo    { get; set; }
    }
}
