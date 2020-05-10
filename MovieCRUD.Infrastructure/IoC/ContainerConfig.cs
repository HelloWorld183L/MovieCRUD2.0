using MovieCRUD.Infrastructure.Network;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using AutoMapper;
using System.Web.Mvc;
using Autofac;
using Stylet;
using System;
using MovieCRUD.Infrastructure.Services;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Network.v1.Interfaces;
using MovieCRUD.Infrastructure.Persistence.Services;
using Autofac.Core;
using MovieCRUD.Infrastructure.Network.v1;

namespace MovieCRUD.Infrastructure.IoC
{
    public class ContainerConfig
    {
        private static IContainer _container;
        private static ICollection<Assembly> _assembliesUsed = new List<Assembly>();

        public static ContainerBuilder RegisterWPFContainer(Assembly viewAssembly, IWindowManagerConfig windowManagerConfig)
        {
            var containerBuilder = new ContainerBuilder();

            RegisterWPFTypes(containerBuilder, viewAssembly, windowManagerConfig);

            RegisterCommonTypes(containerBuilder);

            return containerBuilder;
        }

        public static void RegisterMvcContainer(Assembly controllerAssembly)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterControllers(controllerAssembly);

            _assembliesUsed.Add(controllerAssembly);
            RegisterCommonTypes(containerBuilder);

            _container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
        }

        public static void RegisterMovieApiContainer(Assembly controllerAssembly, HttpConfiguration config)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterApiControllers(controllerAssembly);
            _assembliesUsed.Add(controllerAssembly);
            RegisterCommonTypes(containerBuilder);

            _container = containerBuilder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
        }

        public static void RegisterAuthApiContainer(Assembly controllerAssembly, HttpConfiguration config)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterApiControllers(controllerAssembly);
            _assembliesUsed.Add(controllerAssembly);

            containerBuilder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<UserRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var logger = new Logger();
            containerBuilder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<AuthApiClient>().As<IAuthApiClient>().SingleInstance().WithParameter("logger", logger);
            _assembliesUsed.Add(typeof(ApplicationDbContext).Assembly);
            RegisterAutoMapper(containerBuilder);

            _container = containerBuilder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(_container);
        }

        private static void RegisterCommonTypes(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<MovieRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var logger = new Logger();
            containerBuilder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<MovieApiClient>().As<IMovieApiClient>().SingleInstance().WithParameter("logger", logger);

            _assembliesUsed.Add(typeof(ApplicationDbContext).Assembly);

            RegisterAutoMapper(containerBuilder);
        }

        // Copied from https://github.com/AutoMapper/AutoMapper/issues/1109
        private static void RegisterAutoMapper(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes().AssignableTo(typeof(Profile));

            containerBuilder.Register(componentContext => new MapperConfiguration(config =>
            {
                config.AddMaps(_assembliesUsed);
            })).AsSelf().SingleInstance();

            containerBuilder.Register(componentContext => componentContext.Resolve<MapperConfiguration>().CreateMapper(componentContext.Resolve)).As<IMapper>().InstancePerLifetimeScope();
        }
        
        private static void RegisterWPFTypes(ContainerBuilder containerBuilder, Assembly viewAssembly, IWindowManagerConfig config)
        {
            var viewManagerConfig = new ViewManagerConfig()
            {
                ViewFactory = GetInstance,
                ViewAssemblies = new List<Assembly>() { viewAssembly }
            };

            containerBuilder.RegisterType<WindowManager>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterInstance(config).ExternallyOwned();
            containerBuilder.RegisterInstance<IViewManager>(new ViewManager(viewManagerConfig)).AsImplementedInterfaces();
            containerBuilder.RegisterType<EventAggregator>().AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<MessageBoxViewModel>().As<IMessageBoxViewModel>().ExternallyOwned(); // Not singleton!
            _assembliesUsed.Add(viewAssembly);
        }

        private static object GetInstance(Type type)
        {
            return _container.Resolve(type);
        }
    }
}