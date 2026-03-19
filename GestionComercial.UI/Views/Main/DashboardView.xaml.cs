using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Main;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.UI.Views.Main
{
    public partial class DashboardView : UserControl
    {
        public DashboardView() => InitializeComponent();

        private ShellViewModel Shell => IoC.Get<ShellViewModel>();

        private void NuevaVenta_Click(object sender, RoutedEventArgs e)
            => Shell?.IrVentas();

        private void IrVentasCompleto_Click(object sender, RoutedEventArgs e)
            => Shell?.IrVentas();

        private void IrCaja_Click(object sender, RoutedEventArgs e)
            => Shell?.IrCaja();

        private void IrClientes_Click(object sender, RoutedEventArgs e)
            => Shell?.IrClientes();

        private void IrCompras_Click(object sender, RoutedEventArgs e)
            => Shell?.IrCompras();

        private void IrProductos_Click(object sender, RoutedEventArgs e)
            => Shell?.IrProductos();

        private void IrProductosStock_Click(object sender, RoutedEventArgs e)
            => Shell?.IrProductos();

        private void IrInventario_Click(object sender, RoutedEventArgs e)
            => Shell?.IrInventario();

        private void IrReportes_Click(object sender, RoutedEventArgs e)
            => Shell?.IrReportes();

        private void IrConfiguracion_Click(object sender, RoutedEventArgs e)
            => Shell?.IrConfiguracion();

        // ── Vendedor ────────────────────────────────────────────────────────────
        private void CobrarPendiente_Click(object sender, RoutedEventArgs e)
            => Shell?.IrVentas(); // Navega a ventas (pendientes se filtran en el listado)

        private void NuevoCliente_Click(object sender, RoutedEventArgs e)
            => Shell?.IrClientes(); // Navega a clientes para crear nuevo

        // ── Administrador ────────────────────────────────────────────────────────
        private void NuevoProducto_Click(object sender, RoutedEventArgs e)
            => Shell?.IrProductos(); // Navega a productos para crear nuevo

        private void RegistrarCompra_Click(object sender, RoutedEventArgs e)
            => Shell?.IrCompras(); // Navega a compras para registrar nueva

        private void ResumenDiario_Click(object sender, RoutedEventArgs e)
            => Shell?.IrReportes(); // Navega a reportes operativos del día

        // ── Gerente ───────────────────────────────────────────────────────────────
        private void IrReporteGerencial_Click(object sender, RoutedEventArgs e)
            => Shell?.IrReportes(); // Ya diferencia a ReporteGerenciaViewModel

        private void IrCompararPeriodos_Click(object sender, RoutedEventArgs e)
            => Shell?.IrReportes(); // Navega a reportes (comparativa de períodos)

        private void IrVerEquipo_Click(object sender, RoutedEventArgs e)
            => Shell?.IrConfiguracion(); // Navega a configuración (gestión de usuarios/equipo)
    }
}
