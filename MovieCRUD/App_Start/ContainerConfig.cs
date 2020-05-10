using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using MovieCRUD.Infrastructure;
using MovieCRUD.Infrastructure.Services;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Network;
using System.Reflection;
using System.Web.Mvc;
using MovieCRUD.Infrastructure.Network.v1.Interfaces;

namespace MovieCRUD.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterMvcContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());

            RegisterCommonTypes(containerBuilder);

            var container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        // Copied from https://github.com/AutoMapper/AutoMapper/issues/1109
        private static void RegisterAutoMapper(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes().AssignableTo(typeof(Profile));

            containerBuilder.Register(componentContext => new MapperConfiguration(config =>
            {
                config.AddMaps(Assembly.GetExecutingAssembly());
            })).AsSelf().SingleInstance();

            containerBuilder.Register(componentContext => componentContext.Resolve<MapperConfiguration>().CreateMapper(componentContext.Resolve)).As<IMapper>().InstancePerLifetimeScope();
        }

        private static void RegisterCommonTypes(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<MovieRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var logger = new Logger();
            containerBuilder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<MovieApiClient>().As<IMovieApiClient>().SingleInstance().WithParameter("logger", logger);
            RegisterAutoMapper(containerBuilder);
        }
    }
}