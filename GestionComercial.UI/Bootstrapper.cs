// Bootstrapper.cs
using Caliburn.Micro;
using GestionComercial.UI.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Windows;

public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container;

    public Bootstrapper() => Initialize();

    protected override void Configure()
    {
        _container = new SimpleContainer();
        _container.Singleton<IWindowManager, WindowManager>();
        _container.Singleton<IEventAggregator, EventAggregator>();
        _container.PerRequest<ShellViewModel>();
        _container.PerRequest<LoginViewModel>();
    }

    protected override object GetInstance(Type service, string key)
    {
        var instance = _container.GetInstance(service, key);
        if (instance != null) return instance;
        throw new InvalidOperationException($"No instance for {service.Name}");
    }

 

    protected override IEnumerable<object> GetAllInstances(Type service) =>
        _container.GetAllInstances(service);

    protected override void BuildUp(object instance) =>
        _container.BuildUp(instance);

    protected override async void OnStartup(object sender, StartupEventArgs e)
    {
        await DisplayRootViewForAsync<LoginViewModel>();
    }
}