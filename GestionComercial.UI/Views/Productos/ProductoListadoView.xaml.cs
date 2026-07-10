using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.UI.ViewModels.Productos;

namespace GestionComercial.UI.Views.Productos
{
    public partial class ProductoListadoView : UserControl
    {
        private ProductoListadoViewModel ViewModel => DataContext as ProductoListadoViewModel;

        public ProductoListadoView()
        {
            InitializeComponent();
        }

        // Conecta el botón "Nuevo Producto" → ProductoListadoViewModel.NuevoProducto()
        private async void NuevoProducto_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.NuevoProducto();
        }

        // Buscar al presionar Enter
        private async void TextoBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ViewModel != null)
                await ViewModel.Buscar();
        }

        // Buscar con botón
        private async void Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.Buscar();
        }

        // Cerrar sidebar
        private void CerrarDetalle_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.CerrarDetalle();
        }

        // Editar producto desde sidebar
        private async void EditarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.EditarProducto();
        }

        // Ver movimientos desde sidebar
        private async void VerMovimientos_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.VerMovimientos();
        }

        // Desactivar producto desde sidebar
        private void DesactivarProducto_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.DesactivarProducto();
        }

        // Paginación
        private async void PaginaAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.PaginaAnterior();
        }

        private async void PaginaSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.PaginaSiguiente();
        }

        // Selección en la tabla → abre sidebar
        private void ProductoSeleccionado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel == null) return;
            var grid = sender as DataGrid;
            ViewModel.ProductoSeleccionado = grid?.SelectedItem as GestionComercial.Aplicacion.DTOs.Productos.ProductoListadoDto;
        }

        // ── Ajuste Masivo de Precios ───────────────────────────────
        private void AbrirAjusteMasivo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.AbrirAjusteMasivo();
        }

        private void GenerarPreview_Click(object sender, RoutedEventArgs e)
        {
            // Determine tipo de ajuste según radio buttons seleccionados
            if (TipoPorcentaje.IsChecked == true)
            {
                ViewModel.TipoAjuste = "porcentaje";
                PorcentajeInput.Visibility = Visibility.Visible;
                SignoLabel.Visibility = Visibility.Visible;
                MontoFijoInput.Visibility = Visibility.Collapsed;
                SignoPesos.Visibility = Visibility.Collapsed;
                SignoLabel.Text = "%";
            }
            else if (TipoFijo.IsChecked == true)
            {
                ViewModel.TipoAjuste = "fijo";
                PorcentajeInput.Visibility = Visibility.Collapsed;
                SignoLabel.Visibility = Visibility.Collapsed;
                MontoFijoInput.Visibility = Visibility.Visible;
                SignoPesos.Visibility = Visibility.Visible;
            }
            
            ViewModel?.GenerarPreviewAjuste();
        }

        private async void CancelarAjuste_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.CancelarAjusteMasivo();
        }

        private async void ConfirmarAjuste_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.ConfirmarAjusteMasivo();
        }

        // ── Gestión de Categorías ─────────────────────────────────────
        private async void CrearCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.AbrirPopupCategorias();
        }

        private async void CrearCategoriaConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.CrearCategoria();
        }

        private void CancelarCrearCategoria_Click(object sender, RoutedEventArgs e)
        {
            ViewModel?.CerrarPopupCategorias();
        }

        private async void NuevaCategoria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ViewModel != null)
                await ViewModel.CrearCategoria();
        }

        private async void EliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;
            var btn = sender as FrameworkElement;
            var categoria = btn?.DataContext as CategoriaItemDto;
            if (categoria != null)
                await ViewModel.EliminarCategoria(categoria);
        }

        private async void RenombrarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;
            var btn = sender as FrameworkElement;
            var categoria = btn?.DataContext as CategoriaItemDto;
            if (categoria != null)
                await ViewModel.RenombrarCategoria(categoria);
        }

        private async void EliminarTodosProductos_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;
            var btn = sender as FrameworkElement;
            var categoria = btn?.DataContext as CategoriaItemDto;
            if (categoria != null)
                await ViewModel.EliminarTodosLosProductos(categoria);
        }
    }
}
