using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using GestionComercial.Aplicacion.Interfaces.Autenticacion;
using GestionComercial.Aplicacion.Servicios;
using System.Windows;

namespace GestionComercial.UI
{
    public partial class App : Application
    {
        private readonly Bootstrapper _bootstrapper;
        private IAuthService _authService;
        public bool IsCurrentUserAdmin => _authService?.IsCurrentUserAdmin() ?? false;

        public App()
        {
            _bootstrapper = new Bootstrapper();
            // Initialize authentication adapter (compatibility layer)
            _authService = new AuthAdapter();

            LiveCharts.Configure(config =>
                config.AddSkiaSharp()
                      .AddDefaultMappers()
                      .AddDarkTheme());
        }
    }
}
