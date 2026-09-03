using GestionComercial.Dominio.Entidades;
using System;

namespace GestionComercial.Dominio.Entidades.Mantenimiento
{
    public class MantenimientoLog : EntidadBase
    {
        public string Tipo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
        public string? Detalles { get; set; }
        public int? IdUsuario { get; set; }
    }
}
