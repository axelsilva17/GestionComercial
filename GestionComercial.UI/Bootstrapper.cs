using Caliburn.Micro;
using FluentValidation;
using GestionComercial.Aplicacion.DTOs.Clientes;
using GestionComercial.Aplicacion.DTOs.Compras;
using GestionComercial.Aplicacion.DTOs.Productos;
using GestionComercial.Aplicacion.DTOs.Proveedores;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Aplicacion.DTOs.Auditoria;
using GestionComercial.Aplicacion.Validators;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.Dominio.Repositorio;
using GestionComercial.Persistencia.Contexto;
using GestionComercial.Persistencia.Repositorio;
using GestionComercial.UI.ViewModels.Main;
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
      
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();

            // ── Base de datos ─────────────────────────────────────────────────
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection")!;

            _container.Handler<GestionComercialContext>(
                _ => new GestionComercialContext(
                    new DbContextOptionsBuilder<GestionComercialContext>()
                        .UseSqlServer(connectionString)
                        .Options));

            _container.Handler<IUnitOfWork>(
                c => new UnitOfWork(c.GetInstance<GestionComercialContext>()));

            // ── Servicios ─────────────────────────────────────────────────────
            _container.Singleton<SesionServicio>();
            _container.PerRequest<AutenticacionServicio>();
            _container.PerRequest<IClienteServicio, ClienteServicio>();
            _container.PerRequest<IVentaServicio, VentaServicio>();
            _container.PerRequest<ICompraServicio, CompraServicio>();
            _container.PerRequest<IProductoServicio, ProductoServicio>();
            _container.PerRequest<ICajaServicio, CajaServicio>();
            _container.PerRequest<IAuditoriaServicio, AuditoriaServicio>();
            _container.PerRequest<IAuditoriaAppService, AuditoriaAppService>();
            _container.PerRequest<IProveedorServicio, ProveedorServicio>();
            _container.PerRequest<IStockServicio, StockServicio>();
            _container.PerRequest<IReporteServicio, ReporteServicio>();
            _container.PerRequest<IUsuarioServicio, UsuarioServicio>();
            _container.PerRequest<RecuperacionContrasenaServicio>();
            _container.PerRequest<IAuditoriaAppService, AuditoriaAppService>();

            // ── Validators (FluentValidation) ─────────────────────────────────
            // Nota: c.GetInstance<IUnitOfWork>().Clientes asume que IUnitOfWork
            // expone los repositorios como propiedades. Ajustá los nombres
            // según tu implementación de UnitOfWork.
            _container.Handler<IValidator<ClienteCrearDto>>(
                c => new ClienteValidator(c.GetInstance<IUnitOfWork>().Clientes));

            _container.Handler<IValidator<ProductoCrearDto>>(
                c => new ProductoValidator(c.GetInstance<IUnitOfWork>().Productos));

            _container.Handler<IValidator<UsuarioCrearDto>>(
                c => new UsuarioValidator(c.GetInstance<IUnitOfWork>().Usuarios));

            _container.Handler<IValidator<VentaCrearDto>>(
                c => new VentaValidator(c.GetInstance<IUnitOfWork>().Productos));

            _container.Handler<IValidator<CompraCrearDto>>(
                c => new CompraValidator(c.GetInstance<IUnitOfWork>().Proveedores));

            _container.Handler<IValidator<ProveedorCrearDto>>(
                c => new ProveedorValidator());

            _container.Handler<IValidator<ProveedorActualizarDto>>(
                c => new ProveedorActualizarValidator());

            _container.Handler<IValidator<CajaAbrirDto>>(
                c => new CajaValidator(c.GetInstance<IUnitOfWork>().Cajas));

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

            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var vmName = modelType.FullName ?? string.Empty;
                var viewName = vmName
                    .Replace(".ViewModels.", ".Views.")   // plural → singular no, Views directo
                    .Replace(".ViewModel.", ".Views.")   // por si alguno usa singular
                    .Replace("ViewModel", "View");
                var viewType = modelType.Assembly.GetType(viewName);
                
                if (viewType == null)
                {
                    // Intentar con namespace completo
                    viewType = Assembly.GetExecutingAssembly().GetType(viewName);
                }
                
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
        {
            (Application.Current as App)?.ApplyTheme();
            await DisplayRootViewForAsync<LoginViewModel>();
        }
    }
}