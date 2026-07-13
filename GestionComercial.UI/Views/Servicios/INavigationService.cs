using System.Threading.Tasks;

namespace GestionComercial.UI.Views.Servicios
{
    ///     /// Servicio de navegación entre secciones de la aplicación.
    /// Los ViewModels lo usan para navegar sin conocer la UI directamente.
    public interface INavigationService
    {
        /// Navega a la sección Dashboard.
        Task IrDashboardAsync();

        /// Navega a la sección Ventas (Listado/Historial).
        Task IrVentasAsync();

        /// Navega a la sección Nueva Venta.
        Task IrNuevaVentaAsync();

        /// Navega a la sección Compras.
        Task IrComprasAsync();

        /// Navega a la sección Caja.
        Task IrCajaAsync();

        /// Navega a la sección Productos.
        Task IrProductosAsync();

        /// Navega a la sección Clientes.
        Task IrClientesAsync();

        /// Navega a la sección Proveedores.
        Task IrProveedoresAsync();

        /// Navega a la sección Reportes.
        Task IrReportesAsync();

        /// Navega a la sección Configuración.
        Task IrConfiguracionAsync();
    }
}
