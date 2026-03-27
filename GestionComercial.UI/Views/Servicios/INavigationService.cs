using System.Threading.Tasks;

namespace GestionComercial.UI.Views.Servicios
{
    /// <summary>
    /// Servicio de navegación entre secciones de la aplicación.
    /// Los ViewModels lo usan para navegar sin conocer la UI directamente.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>Navega a la sección Dashboard.</summary>
        Task IrDashboardAsync();

        /// <summary>Navega a la sección Ventas (Listado/Historial).</summary>
        Task IrVentasAsync();

        /// <summary>Navega a la sección Nueva Venta.</summary>
        Task IrNuevaVentaAsync();

        /// <summary>Navega a la sección Compras.</summary>
        Task IrComprasAsync();

        /// <summary>Navega a la sección Caja.</summary>
        Task IrCajaAsync();

        /// <summary>Navega a la sección Productos.</summary>
        Task IrProductosAsync();

        /// <summary>Navega a la sección Clientes.</summary>
        Task IrClientesAsync();

        /// <summary>Navega a la sección Proveedores.</summary>
        Task IrProveedoresAsync();

        /// <summary>Navega a la sección Reportes.</summary>
        Task IrReportesAsync();

        /// <summary>Navega a la sección Configuración.</summary>
        Task IrConfiguracionAsync();
    }
}
