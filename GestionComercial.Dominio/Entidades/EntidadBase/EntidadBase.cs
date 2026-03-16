namespace GestionComercial.Dominio.Entidades
{
    public abstract class EntidadBase
    {
        public int      Id        { get; set; }
        public DateTime FechaAlta { get; set; } = DateTime.Now;
        public bool    Activo    { get; set; } = true;
    }
}
