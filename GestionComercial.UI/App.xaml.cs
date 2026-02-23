using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Win32;
using System;
using System.Windows;

namespace GestionComercial.UI
{
    public partial class App : Application
    {
     

        public App()
        {
            LiveCharts.Configure(config => config.AddSkiaSharp().AddDefaultMappers().AddDarkTheme());
            ApplyTheme();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Detecta cambios en modo del sistema
            SystemEvents.UserPreferenceChanged += (s, args) =>
            {
                if (args.Category == UserPreferenceCategory.General)
                {
                    ApplyTheme();
                }
            };
        }
        private void ApplyTheme()
        {
            bool isDark = IsDarkModeEnabled();
            var dictionaries = Resources.MergedDictionaries;
            dictionaries.Clear();

            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string path = isDark
                ? "/Views/Recursos/Estilos/ThemeDark.xaml"
                : "/Views/Recursos/Estilos/ThemeLight.xaml";

            var theme = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/{assemblyName};component/{path}", UriKind.Absolute)
            };

            dictionaries.Add(theme);
        }
        private bool IsDarkModeEnabled()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

                object value = key?.GetValue("AppsUseLightTheme");

                if (value is int intValue)
                {
                    return intValue == 0; // 0 = Dark
                }
            }
            catch
            {
            }

            return false;
        }
    }
}
