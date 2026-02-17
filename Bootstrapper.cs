// C#
using Caliburn.Micro;

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

        // Registrar Singletons / Transients según corresponda
        _container.Singleton<ShellViewModel>();
        _container.PerRequest<LoginViewModel>();
    }

    protected override object GetInstance(Type service, string key)
    {
        return _container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return _container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        _container.BuildUp(instance);
    }

    protected override async void OnStartup(object sender, StartupEventArgs e)
    {
        // Inicia la aplicación con el Shell (recomendado) o con Login si prefieres:
        await DisplayRootViewForAsync<ShellViewModel>();
    }
}