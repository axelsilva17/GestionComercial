using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GestionComercial.UI.Views.Behaviors
{
    /// <summary>
    /// Behavior que hace responsive cualquier elemento de búsqueda o toolbar
    /// detectando el tamaño de la ventana y ajustando anchos automáticamente
    /// </summary>
    public static class ResponsiveBehavior
    {
        // DependencyProperty para habilitar responsive
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(ResponsiveBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);
        public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

        // Anchos mínimos y máximos configurables
        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.RegisterAttached(
                "MinWidth",
                typeof(double),
                typeof(ResponsiveBehavior),
                new PropertyMetadata(200.0));

        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.RegisterAttached(
                "MaxWidth",
                typeof(double),
                typeof(ResponsiveBehavior),
                new PropertyMetadata(800.0));

        public static double GetMinWidth(DependencyObject obj) => (double)obj.GetValue(MinWidthProperty);
        public static void SetMinWidth(DependencyObject obj, double value) => obj.SetValue(MinWidthProperty, value);

        public static double GetMaxWidth(DependencyObject obj) => (double)obj.GetValue(MaxWidthProperty);
        public static void SetMaxWidth(DependencyObject obj, double value) => obj.SetValue(MaxWidthProperty, value);

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement element) return;
            
            if ((bool)e.NewValue)
            {
                element.Loaded += Element_Loaded;
                if (element.IsLoaded)
                    AttachToSizeChanged(element);
            }
            else
            {
                element.Loaded -= Element_Loaded;
                DetachFromSizeChanged(element);
            }
        }

        private static void Element_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
                AttachToSizeChanged(element);
        }

        private static void AttachToSizeChanged(FrameworkElement element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            while (parent != null && parent is not Window)
            {
                var fe = parent as FrameworkElement;
                if (fe != null && fe.ActualWidth > 0)
                {
                    fe.SizeChanged += Element_SizeChanged;
                    UpdateElementWidth(element, fe.ActualWidth);
                    return;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }

            element.SizeChanged += Element_SizeChanged;
            UpdateElementWidth(element, element.ActualWidth);
        }

        private static void DetachFromSizeChanged(FrameworkElement element)
        {
            element.SizeChanged -= Element_SizeChanged;
        }

        private static void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                double availableWidth = e.NewSize.Width;
                var parent = VisualTreeHelper.GetParent(element);
                if (parent is Grid grid)
                {
                    UpdateGridChildren(grid, availableWidth);
                }
                else
                {
                    UpdateElementWidth(element, availableWidth);
                }
            }
        }

        private static void UpdateGridChildren(Grid grid, double availableWidth)
        {
            double fixedWidth = 0;
            int starColumns = 0;

            foreach (var col in grid.ColumnDefinitions)
            {
                if (col.Width.IsAuto || col.Width.Value >= 1)
                {
                    fixedWidth += col.ActualWidth > 0 ? col.ActualWidth : col.MinWidth;
                }
                else if (col.Width.IsStar)
                {
                    starColumns++;
                }
            }

            if (starColumns > 0)
            {
                double remaining = Math.Max(200, availableWidth - fixedWidth - 50);
                double starUnit = remaining / starColumns;

                foreach (var col in grid.ColumnDefinitions)
                {
                    if (col.Width.IsStar)
                    {
                        col.Width = new GridLength(Math.Max(col.MinWidth, starUnit), GridUnitType.Star);
                    }
                }
            }
        }

        private static void UpdateElementWidth(FrameworkElement element, double availableWidth)
        {
            double minW = GetMinWidth(element);
            double maxW = GetMaxWidth(element);

            double newWidth = availableWidth > maxW ? maxW : 
                          availableWidth < minW ? minW : 
                          availableWidth;

            if (element is Border border)
            {
                border.Width = (int)newWidth;
            }
            else if (element is TextBox textBox)
            {
                textBox.Width = (int)newWidth;
            }
        }
    }

    /// <summary>
    /// Behavior específico para toolbar de búsqueda y botones
    /// </summary>
    public static class SearchBarResponsiveBehavior
    {
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached(
                "Enable",
                typeof(bool),
                typeof(SearchBarResponsiveBehavior),
                new PropertyMetadata(false, OnEnableChanged));

        public static bool GetEnable(DependencyObject obj) => (bool)obj.GetValue(EnableProperty);
        public static void SetEnable(DependencyObject obj, bool value) => obj.SetValue(EnableProperty, value);

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && (bool)e.NewValue)
            {
                element.Loaded += (s, args) =>
                {
                    var window = Window.GetWindow(element);
                    if (window != null)
                    {
                        window.SizeChanged += (sender, sizeArgs) =>
                        {
                            AdjustSearchBar(element, sizeArgs.NewSize.Width);
                        };
                        AdjustSearchBar(element, window.ActualWidth);
                    }
                };
            }
        }

        private static void AdjustSearchBar(FrameworkElement element, double windowWidth)
        {
            double minWidth = 200;
            double maxWidth = 800;
            
            double newWidth = windowWidth switch
            {
                < 800 => 200,
                < 1000 => 300,
                < 1200 => 400,
                < 1400 => 500,
                _ => 600
            };

            newWidth = Math.Max(minWidth, Math.Min(maxWidth, newWidth));

            if (element is Border border)
                border.Width = (int)newWidth;
            else if (element is TextBox textBox)
                textBox.Width = (int)newWidth;
        }
    }

    /// <summary>
    /// Behavior para toolbar completa (search + buttons + combos)
    /// </summary>
    public static class ToolbarResponsiveBehavior
    {
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached(
                "Enable",
                typeof(bool),
                typeof(ToolbarResponsiveBehavior),
                new PropertyMetadata(false, OnEnableChanged));

        public static bool GetEnable(DependencyObject obj) => (bool)obj.GetValue(EnableProperty);
        public static void SetEnable(DependencyObject obj, bool value) => obj.SetValue(EnableProperty, value);

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && (bool)e.NewValue)
            {
                element.Loaded += (s, args) =>
                {
                    var window = Window.GetWindow(element);
                    if (window != null)
                    {
                        window.SizeChanged += (sender, sizeArgs) =>
                        {
                            AdjustToolbar(element, sizeArgs.NewSize.Width);
                        };
                        AdjustToolbar(element, window.ActualWidth);
                    }
                };
            }
        }

        private static void AdjustToolbar(FrameworkElement element, double windowWidth)
        {
            // Ajustar según ancho de ventana
            double searchWidth = windowWidth switch
            {
                < 800 => 200,
                < 950 => 250,
                < 1100 => 300,
                < 1250 => 350,
                < 1400 => 400,
                < 1550 => 450,
                < 1700 => 500,
                _ => 550
            };

            // Ajustar espaciado entre botones
            double spacing = windowWidth switch
            {
                < 900 => 6,
                < 1100 => 8,
                < 1300 => 10,
                _ => 12
            };

            // Buscar children y ajustar
            if (element is Panel panel)
            {
                for (int i = 0; i < panel.Children.Count; i++)
                {
                    var child = panel.Children[i];
                    
                    // Ajustar búsqueda (primer elemento)
                    if (i == 0 && child is Border searchBorder)
                    {
                        searchBorder.Width = (int)searchWidth;
                    }
                    
                    // Ajustar espaciado (Borders)
                    if (child is Border spacer && spacer.MinWidth == 0 && spacer.Width > 0)
                    {
                        spacer.Width = (int)spacing;
                    }
                }
            }
        }
    }
}