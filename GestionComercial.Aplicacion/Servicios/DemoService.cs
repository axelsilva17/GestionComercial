using System;
using System.IO;
using System.Reflection;

namespace GestionComercial.Aplicacion.Servicios
{
    /// <summary>
    /// Servicio que gestiona las limitaciones de la versión demo.
    /// Controla: período de prueba (30 días), máximo de productos, máximo de ventas.
    /// </summary>
    public class DemoService
    {
        private static readonly string DemoFlagPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".",
            "demo.dat");

        private const int DiasPrueba = 30;
        private const int MaxProductos = 100;
        private const int MaxVentas = 200;

        /// <summary>
        /// Indica si la aplicación está en modo demo.
        /// Si no existe el archivo demo.dat, es demo.
        /// Si existe y tiene una fecha dentro del rango, es demo.
        /// Si la fecha expiró, la demo expiró.
        /// </summary>
        public bool EsDemo => !File.Exists(Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".",
            "licencia.dat"));

        public bool DemoExpirada
        {
            get
            {
                if (!EsDemo) return false;
                if (!File.Exists(DemoFlagPath)) return false;

                var contenido = File.ReadAllText(DemoFlagPath);
                if (DateTime.TryParse(contenido, out var fechaInicio))
                    return DateTime.Now > fechaInicio.AddDays(DiasPrueba);

                return true;
            }
        }

        public int DiasRestantes
        {
            get
            {
                if (!EsDemo) return int.MaxValue;
                if (!File.Exists(DemoFlagPath)) return DiasPrueba;

                var contenido = File.ReadAllText(DemoFlagPath);
                if (DateTime.TryParse(contenido, out var fechaInicio))
                {
                    var restantes = (fechaInicio.AddDays(DiasPrueba) - DateTime.Now).Days;
                    return Math.Max(0, restantes);
                }

                return 0;
            }
        }

        /// <summary>
        /// Registra la primera ejecución de la demo.
        /// </summary>
        public void RegistrarInicioDemo()
        {
            if (!EsDemo) return;
            if (!File.Exists(DemoFlagPath))
                File.WriteAllText(DemoFlagPath, DateTime.Now.ToString("o"));
        }

        /// <summary>
        /// Verifica si se puede crear un producto nuevo.
        /// </summary>
        public bool PuedeCrearProducto(int productosActuales)
        {
            if (!EsDemo) return true;
            return productosActuales < MaxProductos;
        }

        /// <summary>
        /// Verifica si se puede registrar una venta nueva.
        /// </summary>
        public bool PuedeCrearVenta(int ventasActuales)
        {
            if (!EsDemo) return true;
            return ventasActuales < MaxVentas;
        }

        public string MensajeLimiteProductos => $"Versión demo: máximo {MaxProductos} productos.";
        public string MensajeLimiteVentas => $"Versión demo: máximo {MaxVentas} ventas.";

        /// <summary>
        /// Activa la licencia completa (para cuando el cliente pague).
        /// Crea el archivo licencia.dat.
        /// </summary>
        public void ActivarLicencia()
        {
            var licenciaPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".",
                "licencia.dat");
            File.WriteAllText(licenciaPath, DateTime.Now.ToString("o"));
        }

        // ── Showcase ─────────────────────────────────────────────────────────
        private static readonly string ShowcaseShownPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".",
            ".demo_shown");

        /// <summary>
        /// Indica si el showcase debe mostrarse (solo la primera vez).
        /// </summary>
        public bool MostrarShowcasePendiente => EsDemo && !File.Exists(ShowcaseShownPath);

        /// <summary>
        /// Marca el showcase como mostrado.
        /// </summary>
        public void MarcarShowcaseMostrado()
        {
            File.WriteAllText(ShowcaseShownPath, DateTime.Now.ToString("o"));
        }

        // ── Credenciales iniciales ───────────────────────────────────────────
        private static readonly string CredencialesPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".",
            "credenciales_iniciales.txt");

        /// <summary>
        /// Genera el archivo de credenciales iniciales si no existe.
        /// </summary>
        public void GenerarCredencialesIniciales()
        {
            if (File.Exists(CredencialesPath)) return;

            var contenido = @"╔══════════════════════════════════════════════════╗
║       GESTIONCOMERCIAL — CREDENCIALES DEMO      ║
╠══════════════════════════════════════════════════╣
║                                                  ║
║  USUARIOS DISPONIBLES:                           ║
║                                                  ║
║  👤 Administrador                                ║
║     Email:    admin@demo.com                     ║
║     Password: Admin123!                          ║
║                                                  ║
║  👤 Vendedor                                     ║
║     Email:    vendedor@demo.com                  ║
║     Password: Vendedor123!                       ║
║                                                  ║
║  👤 Gerente                                      ║
║     Email:    gerente@demo.com                   ║
║     Password: Gerente123!                        ║
║                                                  ║
╠══════════════════════════════════════════════════╣
║  Empresa:  Demo Store S.R.L.                     ║
║  Sucursal: Centro (Default)                      ║
║                                                  ║
║  Métodos de pago:                                ║
║    Efectivo, Tarjeta de Crédito,                 ║
║    Tarjeta de Débito, Transferencia              ║
║                                                  ║
╠══════════════════════════════════════════════════╣
║  NOTA: Esta es una versión de prueba.            ║
║  Máximo 100 productos y 200 ventas.              ║
║  Vigencia: 30 días desde la primera ejecución.   ║
║                                                  ║
║  Soporte: soporte@gestioncomercial.com           ║
╚══════════════════════════════════════════════════╝";

            File.WriteAllText(CredencialesPath, contenido);
        }
    }
}
