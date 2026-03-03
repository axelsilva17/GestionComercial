using Caliburn.Micro;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Repositorio;
using GestionComercial.Persistencia.Contexto;
using GestionComercial.UI.ViewModels.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Windows;

namespace GestionComercial.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = null!;

        public Bootstrapper() => Initialize();

        protected override void Configure()
        {
            _container = new SimpleContainer();

            // ── Caliburn ──────────────────────────────────────────────────────
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // ── Configuración ─────────────────────────────────────────────────
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection")!;

            // ── DbContext ─────────────────────────────────────────────────────
            _container.Handler<GestionComercialContext>(
                _ => new GestionComercialContext(
                    new DbContextOptionsBuilder<GestionComercialContext>()
                        .UseSqlServer(connectionString)
                        .Options));

            // ── UnitOfWork ────────────────────────────────────────────────────
            _container.Handler<IUnitOfWork>(
                c => new UnitOfWork(c.GetInstance<GestionComercialContext>()));

            // ── Servicios ─────────────────────────────────────────────────────
            _container.PerRequest<AutenticacionServicio>();
            _container.PerRequest<ClienteServicio>();
            _container.PerRequest<ProductoServicio>();
            _container.PerRequest<VentaServicio>();
            _container.PerRequest<CompraServicio>();
            _container.PerRequest<CajaServicio>();
            _container.PerRequest<ProveedorServicio>();
            _container.PerRequest<StockServicio>();
            _container.PerRequest<ReporteServicio>();
            _container.PerRequest<UsuarioServicio>();

            // ── ViewModels ────────────────────────────────────────────────────
            var assembly = Assembly.GetExecutingAssembly();
            var viewModelTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                         && !t.IsAbstract
                         && t.Namespace != null
                         && t.Namespace.StartsWith("GestionComercial.UI.ViewModels")
                         && typeof(Screen).IsAssignableFrom(t));

            foreach (var vmType in viewModelTypes)
            {
                if (vmType == typeof(ShellViewModel))
                    _container.RegisterSingleton(vmType, null, vmType);
                else
                    _container.RegisterPerRequest(vmType, null, vmType);
            }

            // ── ViewLocator ───────────────────────────────────────────────────
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var vmName = modelType.FullName ?? string.Empty;
                var viewName = vmName
                    .Replace(".ViewModels.", ".Views.")
                    .Replace("ViewModel", "View");
                var viewType = modelType.Assembly.GetType(viewName);
                System.Diagnostics.Debug.WriteLine(
                    $"[ViewLocator] {modelType.Name} -> {viewType?.Name ?? "NO ENCONTRADO"}");
                return viewType;
            };
        }

        protected override object GetInstance(Type service, string key)
            => _container.GetInstance(service, key);

        protected override IEnumerable<object> GetAllInstances(Type service)
            => _container.GetAllInstances(service);

        protected override void BuildUp(object instance)
            => _container.BuildUp(instance);

        protected override async void OnStartup(object sender, StartupEventArgs e)
            => await DisplayRootViewForAsync<LoginViewModel>();
    }
}
