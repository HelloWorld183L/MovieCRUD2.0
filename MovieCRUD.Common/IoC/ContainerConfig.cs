using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using MovieCRUD.Common;
using MovieCRUD.Data;
using MovieCRUD.Data.Services;
using MovieCRUD.Domain;
using MovieCRUD.Infrastructure.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace MovieCRUD.Common.IoC
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Repository>().
               AsImplementedInterfaces().
               InstancePerRequest();
            builder.RegisterType<ApplicationDbContext>().
                AsImplementedInterfaces().
                InstancePerRequest();

            var container = builder.Build();
        }

        public static void RegisterMvcContainer(Type classType)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetAssembly(classType));

            builder.RegisterType<ApiClient>()
                .AsImplementedInterfaces()
                .InstancePerRequest();
            builder.RegisterType<ApplicationDbContext>()
                .AsSelf()
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void RegisterApiContainer(Assembly controllerAssembly)
        {
            var builder = new ContainerBuilder();

            var config = new HttpConfiguration();

            builder.RegisterApiControllers(controllerAssembly);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}