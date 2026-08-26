using Caliburn.Micro;
using System.Collections.Generic;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class PagoNodoJerarquico : PropertyChangedBase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool EsHoja { get; set; }
        public int? MetodoPagoId { get; set; }
        public string Icono { get; set; } = "💳";
        public PagoNodoJerarquico? Padre { get; set; }
        public List<PagoNodoJerarquico> Hijos { get; set; } = new();
    }
}
