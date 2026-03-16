namespace GestionComercial.Aplicacion.DTOs.Clientes
{
    public class ClienteCrearDto
    {
        public string Nombre    { get; set; }
        public int    Documento { get; set; }
        public string Telefono  { get; set; }
        public string Email     { get; set; }
        public int    IdEmpresa { get; set; }
    }
}
