using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

namespace DepInj;

public class Injector
{
    public static Injector CreateDefaultBuilder() => new();

    public Injector ConfigureServices(Func<Service, Service> func)
    {
        Services = func(Services);
        return this;
    }

    public Injector Build() => this;

    public Service Services { get; private set; } = new Service();
}

public class Service
{
    public Scope CreateScope() => new Scope(this);

    public Service AddSingleton<T>()
    {
        singletons[typeof(T)] = typeof(T);
        return this;
    }
    public Service AddSingleton<TInterface, TImplementation>()
    {
        singletons[typeof(TInterface)] = typeof(TImplementation);
        return this;
    }

    public TInterface GetSingleton<TInterface>()
    {
        var type = singletons[typeof(TInterface)]; 
        // IMainVM - TInterface
        //  MainVM - TImplementation

        if (!implToObject.ContainsKey(type))
        {
            var ctors = type.GetConstructors();

            if (ctors.Length != 1)
            {
                var ctor = ctors.Last();
                var parameters = ctor.GetParameters();
                object[] objs = new object[parameters.Length];

                for (int i = 0; i < objs.Length; i++)
                    if (singletons.ContainsKey(parameters[i].ParameterType))
                    {
                        objs[i] = Activator.CreateInstance(singletons[parameters[i].ParameterType])
                            ?? throw new NullReferenceException();
                    }

                implToObject[type] = ctor.Invoke(objs);
            }
            else
                implToObject[type] = Activator.CreateInstance(type)
                    ?? throw new NullReferenceException();
        }
        return (TInterface)implToObject[type];
    }

    private readonly Dictionary<Type, Type> singletons = new();
    private readonly Dictionary<Type, object> implToObject = new();
}

public class Scope : IDisposable
{
    public T GetService<T>() => srv.GetSingleton<T>();

    public Scope(Service srv) => this.srv = srv;
    public void Dispose() { }
    public Scope ServiceProvider => this;

    private readonly Service srv;
}

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App() =>
        this.host = Injector
            .CreateDefaultBuilder()
            .ConfigureServices(srv => srv
                .AddSingleton<MainWindow>()
                .AddSingleton<IMainVM, MainVM>()
            )
            .Build();

    protected override void OnStartup(StartupEventArgs e)
    {
        using var scope = host.Services.CreateScope();
        var window = scope.ServiceProvider.GetService<MainWindow>();
        var vm = scope.ServiceProvider.GetService<IMainVM>();
        window?.Show();
        base.OnStartup(e);
    }

    private Injector host;
    //private readonly IHost host;
}
