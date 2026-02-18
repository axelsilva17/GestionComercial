using Caliburn.Micro;
using GestionComercial.UI.ViewModel.Main;
using System;
using System.Collections.Generic;
using System.Linq;
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

            // ── Mapeo de namespaces ViewModel → View ──────────────────────────
            var config = new TypeMappingConfiguration
            {
                DefaultSubNamespaceForViewModels = "ViewModel",
                DefaultSubNamespaceForViews = "Views"
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            // Mapeos explícitos por módulo (subcarpetas)
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Main",          "GestionComercial.UI.Views.Main");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Productos",     "GestionComercial.UI.Views.Main.Productos");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Ventas",        "GestionComercial.UI.Views.Main.Ventas");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Compras",       "GestionComercial.UI.Views.Main.Compras");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Caja",          "GestionComercial.UI.Views.Main.Caja");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Clientes",      "GestionComercial.UI.Views.Main.Clientes");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Proveedores",   "GestionComercial.UI.Views.Main.Proveedores");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Reportes",      "GestionComercial.UI.Views.Main.Reportes");
            ViewLocator.AddNamespaceMapping("GestionComercial.UI.ViewModel.Configuracion", "GestionComercial.UI.Views.Main.Configuracion");

            // ── Registro dinámico de todos los ViewModels del ensamblado ──────
            // Busca todas las clases que hereden de Screen o Conductor en el namespace ViewModel
            var assembly = Assembly.GetExecutingAssembly();
            var viewModelTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                         && !t.IsAbstract
                         && t.Namespace != null
                         && t.Namespace.StartsWith("GestionComercial.UI.ViewModel")
                         && (IsSubclassOfRawGeneric(typeof(Conductor<>), t)
                             || typeof(Screen).IsAssignableFrom(t)));

            foreach (var vmType in viewModelTypes)
            {
                // ShellViewModel como Singleton, el resto PerRequest
                if (vmType == typeof(ShellViewModel))
                    _container.Singleton(vmType, vmType);
                else
                    _container.PerRequest(vmType, vmType);
            }
        }

        // Helper para detectar herencia de genéricos (Conductor<object>)
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
