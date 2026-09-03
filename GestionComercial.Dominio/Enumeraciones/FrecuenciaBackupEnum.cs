using System.ComponentModel;

namespace GestionComercial.Dominio.Enumeraciones
{
    public enum FrecuenciaBackupEnum
    {
        [Description("Desactivado")]
        Desactivado = 0,

        [Description("Al iniciar app")]
        AlAbrirApp = 1,

        [Description("Diario")]
        Diario = 2,

        [Description("Semanal")]
        Semanal = 3
    }
}
