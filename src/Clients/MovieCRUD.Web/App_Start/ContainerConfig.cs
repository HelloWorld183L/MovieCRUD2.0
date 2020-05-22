using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace MovieCRUD.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            IServiceCollection services = new ServiceCollection();

            var executingAssembly = Assembly.GetExecutingAssembly();
            services.InstallTypesInAssembly();
            services.InstallGeneralTypes(executingAssembly);

            var dependencyResolver = new MvcDependencyResolver(services.BuildServiceProvider());
            DependencyResolver.SetResolver(dependencyResolver);
        }
    }
}