using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.Views;
using System.Reflection;
using System.Windows;


namespace GestionComercial.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
   

            var config = new TypeMappingConfiguration
            {
                DefaultSubNamespaceForViewModels = "ViewModels",
                DefaultSubNamespaceForViews = "Views"
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Main", "GestionComercial.UI.Views.Main");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Productos", "GestionComercial.UI.Views.Productos");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Ventas", "GestionComercial.UI.Views.Ventas");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Compras", "GestionComercial.UI.Views.Compras");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Caja", "GestionComercial.UI.Views.Caja");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Clientes", "GestionComercial.UI.Views.Clientes");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Proveedores", "GestionComercial.UI.Views.Proveedores");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Reportes", "GestionComercial.UI.Views.Reportes");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Configuracion", "GestionComercial.UI.Views.Configuracion");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModels.Inventario", "GestionComercial.UI.Views.Inventario");

            // Debug hook
            var baseLocate = ViewLocator.LocateForModelType;
            ViewLocator.LocateForModelType = (modelType, displayLocation, context) =>
            {
                var view = baseLocate(modelType, displayLocation, context);
                System.Diagnostics.Debug.WriteLine(
                    $"[ViewLocator] VM={modelType.FullName} → Vista={(view?.GetType().FullName ?? "NULL")}");
                return view;
            };

            // Registro dinámico — sin duplicados manuales
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
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        protected override object GetInstance(Type service, string key)
            => _container.GetInstance(service, key);

        protected override IEnumerable<object> GetAllInstances(Type service)
            => _container.GetAllInstances(service);

        protected override void BuildUp(object instance)
            => _container.BuildUp(instance);

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync<LoginViewModel>();
        }
    }
}