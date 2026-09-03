using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GestionComercial.UI.Views.Controls
{
    public partial class ClockTimePicker : UserControl
    {
        private const int ClockSize = 180;
        private const int NumberRadius = 68;
        private const int HandLength = 52;
        private const int Center = 90;
        private const int NumberSize = 22;
        private const int NumberHalf = 11;

        private bool _isSelectingHours = true;
        private Line _hand = null!;
        private Border[] _hourBorders = new Border[24];
        private Border[] _minuteBorders = new Border[12];

        public static readonly DependencyProperty HourProperty =
            DependencyProperty.Register(nameof(Hour), typeof(int), typeof(ClockTimePicker),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHourChanged));

        public static readonly DependencyProperty MinuteProperty =
            DependencyProperty.Register(nameof(Minute), typeof(int), typeof(ClockTimePicker),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMinuteChanged));

        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set => SetValue(HourProperty, Math.Clamp(value, 0, 23));
        }

        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, Math.Clamp(value, 0, 59));
        }

        public ClockTimePicker()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CreateHand();
            CreateClockNumbers();
            UpdateVisual();
        }

        private static void OnHourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClockTimePicker picker) picker.UpdateVisual();
        }

        private static void OnMinuteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClockTimePicker picker) picker.UpdateVisual();
        }

        private void CreateHand()
        {
            _hand = new Line
            {
                X1 = Center,
                Y1 = Center,
                X2 = Center,
                Y2 = Center - HandLength,
                Stroke = (Brush)FindResource("PrimaryBrush"),
                StrokeThickness = 2,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                IsHitTestVisible = false
            };
            ClockCanvas.Children.Add(_hand);
        }

        private void CreateClockNumbers()
        {
            var textPrimary = (Brush)FindResource("TextPrimaryBrush");

            for (int i = 0; i < 24; i++)
            {
                var (x, y) = GetPosition(i, 24, NumberRadius);
                var border = CreateNumberBorder(i.ToString("D2"), 9, textPrimary);
                Canvas.SetLeft(border, x - NumberHalf);
                Canvas.SetTop(border, y - NumberHalf);
                border.MouseLeftButtonDown += HourNumber_Click;
                border.MouseEnter += Number_MouseEnter;
                border.MouseLeave += Number_MouseLeave;
                border.Tag = i;
                ClockCanvas.Children.Add(border);
                _hourBorders[i] = border;
            }

            for (int i = 0; i < 12; i++)
            {
                int minuteValue = i * 5;
                var (x, y) = GetPosition(i, 12, NumberRadius - 16);
                var border = CreateNumberBorder(minuteValue.ToString("D2"), 10, textPrimary);
                Canvas.SetLeft(border, x - NumberHalf);
                Canvas.SetTop(border, y - NumberHalf);
                border.MouseLeftButtonDown += MinuteNumber_Click;
                border.MouseEnter += Number_MouseEnter;
                border.MouseLeave += Number_MouseLeave;
                border.Tag = minuteValue;
                border.Visibility = Visibility.Collapsed;
                ClockCanvas.Children.Add(border);
                _minuteBorders[i] = border;
            }
        }

        private Border CreateNumberBorder(string text, double fontSize, Brush foreground)
        {
            var tb = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                Foreground = foreground,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            return new Border
            {
                Width = NumberSize,
                Height = NumberSize,
                CornerRadius = new CornerRadius(11),
                Background = Brushes.Transparent,
                Child = tb,
                Cursor = Cursors.Hand
            };
        }

        private static (double x, double y) GetPosition(int index, int total, int radius)
        {
            double angle = (index * 360.0 / total) * Math.PI / 180.0;
            double x = Center + radius * Math.Sin(angle);
            double y = Center - radius * Math.Cos(angle);
            return (x, y);
        }

        private void HourNumber_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is int hour)
            {
                Hour = hour;
            }
        }

        private void MinuteNumber_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is int minute)
            {
                Minute = minute;
            }
        }

        private void HourBorder_Click(object sender, MouseButtonEventArgs e)
        {
            _isSelectingHours = true;
            UpdateVisual();
        }

        private void MinuteBorder_Click(object sender, MouseButtonEventArgs e)
        {
            _isSelectingHours = false;
            UpdateVisual();
        }

        private void Number_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.Child is TextBlock tb)
            {
                tb.Foreground = (Brush)FindResource("PrimaryBrush");
            }
        }

        private void Number_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && border.Child is TextBlock tb)
            {
                bool isSelected = false;
                if (_isSelectingHours && border.Tag is int h && h == Hour)
                    isSelected = true;
                else if (!_isSelectingHours && border.Tag is int m && m == Minute)
                    isSelected = true;

                tb.Foreground = isSelected
                    ? (Brush)FindResource("PrimaryBrush")
                    : (Brush)FindResource("TextPrimaryBrush");
            }
        }

        private void UpdateVisual()
        {
            if (_hand == null || _hourBorders[0] == null) return;

            HourText.Text = Hour.ToString("D2");
            MinuteText.Text = Minute.ToString("D2");

            int selected = _isSelectingHours ? Hour : Minute;
            int total = _isSelectingHours ? 24 : 12;

            double angle = (selected * 360.0 / total) * Math.PI / 180.0;
            _hand.X2 = Center + HandLength * Math.Sin(angle);
            _hand.Y2 = Center - HandLength * Math.Cos(angle);

            HourText.Foreground = _isSelectingHours
                ? (Brush)FindResource("PrimaryBrush")
                : (Brush)FindResource("TextPrimaryBrush");
            MinuteText.Foreground = !_isSelectingHours
                ? (Brush)FindResource("PrimaryBrush")
                : (Brush)FindResource("TextSecondaryBrush");

            for (int i = 0; i < 24; i++)
            {
                _hourBorders[i].Visibility = _isSelectingHours ? Visibility.Visible : Visibility.Collapsed;
                if (_isSelectingHours)
                {
                    bool isSel = i == Hour;
                    var tb = (TextBlock)_hourBorders[i].Child!;
                    tb.Foreground = isSel
                        ? (Brush)FindResource("PrimaryBrush")
                        : (Brush)FindResource("TextPrimaryBrush");
                    _hourBorders[i].Background = isSel
                        ? new SolidColorBrush(Color.FromArgb(30, 0, 120, 215))
                        : Brushes.Transparent;
                }
            }

            for (int i = 0; i < 12; i++)
            {
                _minuteBorders[i].Visibility = !_isSelectingHours ? Visibility.Visible : Visibility.Collapsed;
                if (!_isSelectingHours)
                {
                    bool isSel = (i * 5) == Minute;
                    var tb = (TextBlock)_minuteBorders[i].Child!;
                    tb.Foreground = isSel
                        ? (Brush)FindResource("PrimaryBrush")
                        : (Brush)FindResource("TextPrimaryBrush");
                    _minuteBorders[i].Background = isSel
                        ? new SolidColorBrush(Color.FromArgb(30, 0, 120, 215))
                        : Brushes.Transparent;
                }
            }
        }
    }
}
