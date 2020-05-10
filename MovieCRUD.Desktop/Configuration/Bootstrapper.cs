using Autofac;
using AutoMapper;
using MovieCRUD.Desktop.Models;
using MovieCRUD.Desktop.ViewModels;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Network;
using MovieCRUD.Infrastructure.Network.v1.Interfaces;
using Stylet;
using System.Collections.Generic;
using System.Reflection;

namespace MovieCRUD.Desktop.Configuration
{
    public class Bootstrapper : AutofacBootstrapper<MovieCRUDViewModel>
    {
        private ViewManager viewManager;
        private IWindowManagerConfig windowManagerConfig;

        protected override void ConfigureIoC(ContainerBuilder builder)
        {
            var desktopAssembly = this.GetType().Assembly;
            var viewManagerConfig = new ViewManagerConfig()
            {
                ViewFactory = GetInstance,
                ViewAssemblies = new List<Assembly>() { desktopAssembly }
            };

            viewManager = new ViewManager(viewManagerConfig);
            windowManagerConfig = new WindowManagerConfig();

            builder.RegisterInstance(this).ExternallyOwned();
            builder.RegisterInstance(viewManager).AsImplementedInterfaces();
            builder.RegisterType<EventAggregator>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MessageBoxViewModel>().As<IMessageBoxViewModel>().ExternallyOwned(); // Not singleton!
            builder.RegisterInstance(windowManagerConfig);
            builder.RegisterInstance(SetUpWindowManager());
            // builder.RegisterInstance(this);

            var logger = new Logger();
            builder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MovieApiClient>().As<IMovieApiClient>().SingleInstance().WithParameter("logger", logger);
            RegisterAutoMapper(builder);

            RegisterViewModels(builder, desktopAssembly);
        }

        private void RegisterAutoMapper(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes().AssignableTo(typeof(Profile));

            containerBuilder.Register(componentContext => new MapperConfiguration(config =>
            {
                config.AddMaps(Assembly.GetExecutingAssembly());
            })).AsSelf().SingleInstance();

            containerBuilder.Register(componentContext => componentContext.Resolve<MapperConfiguration>().CreateMapper(componentContext.Resolve)).As<IMapper>().InstancePerLifetimeScope();
        }

        private IWindowManager SetUpWindowManager() => new WindowManager(viewManager, () => new MessageBoxViewModel(), windowManagerConfig);
    }
}
