using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Win32;
using System;
using System.Windows;

namespace GestionComercial.UI
{
    public partial class App : Application
    {
        private readonly Bootstrapper _bootstrapper;

        public App()
        {
            _bootstrapper = new Bootstrapper();

            LiveCharts.Configure(config =>
                config.AddSkiaSharp()
                      .AddDefaultMappers()
                      .AddDarkTheme());
            LiveCharts.Configure(config => config.AddSkiaSharp().AddDefaultMappers().AddDarkTheme());
        }

        internal void ApplyTheme()
        {
            bool isDark = IsDarkModeEnabled();
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name!;
            string path = isDark
                ? "/Views/Recursos/Estilos/ThemeDark.xaml"
                : "/Views/Recursos/Estilos/ThemeLight.xaml";

            var theme = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/{assemblyName};component/{path}", UriKind.Absolute)
            };

            var merged = Resources.MergedDictionaries;
            if (merged.Count > 0)
                merged[0] = theme;
            else
                merged.Add(theme);
        }

        private bool IsDarkModeEnabled()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                if (key?.GetValue("AppsUseLightTheme") is int val)
                    return val == 0;
            }
            catch { }
            return false;
        }
    }
}