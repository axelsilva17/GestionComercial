using GestionComercial.Aplicacion.Servicios;

namespace GestionComercial.UI.Helpers
{
    /// <summary>
    /// Controla qué funcionalidades están habilitadas en modo demo por rol.
    /// En versión completa (EsDemo=false), todo está habilitado.
    /// </summary>
    public class DemoFeatureService
    {
        private readonly DemoService _demoService;

        public DemoFeatureService(DemoService demoService)
        {
            _demoService = demoService;
        }

        public bool EsDemo => _demoService.EsDemo;

        public const string MensajeDemo = "Esta funcionalidad está disponible en la versión completa.";

        /// <summary>
        /// Verifica si un módulo está habilitado para el rol dado en modo demo.
        /// </summary>
        public bool PuedeAcceder(string modulo, string rol)
        {
            if (!EsDemo) return true;

            return rol.ToLower() switch
            {
                // Vendedor: solo POS, Caja, Dashboard
                "vendedor" => modulo.ToLower() switch
                {
                    "dashboard" => true,
                    "ventas" => true,
                    "caja" => true,
                    _ => false
                },
                // Admin: Dashboard, Productos (sin ajuste masivo/import), Reportes (solo lectura)
                "administrador" or "admin" => modulo.ToLower() switch
                {
                    "dashboard" => true,
                    "productos" => true,
                    "clientes" => true,
                    "reportes" => true,
                    _ => false
                },
                // Gerente: Dashboard, Configuración (sin backup/roles), Reportes (solo día)
                "gerente" => modulo.ToLower() switch
                {
                    "dashboard" => true,
                    "configuracion" => true,
                    "reportes" => true,
                    _ => false
                },
                _ => false
            };
        }

        /// <summary>
        /// Verifica si una acción específica está habilitada en modo demo.
        /// </summary>
        public bool PuedeEjecutarAccion(string modulo, string accion)
        {
            if (!EsDemo) return true;

            return (modulo.ToLower(), accion.ToLower()) switch
            {
                // Productos: sin ajuste masivo, sin importar Excel
                ("productos", "ajuste_masivo") => false,
                ("productos", "importar") => false,
                // Configuración: sin backup, sin gestión de roles
                ("configuracion", "backup") => false,
                ("configuracion", "roles") => false,
                _ => true
            };
        }
    }
}
