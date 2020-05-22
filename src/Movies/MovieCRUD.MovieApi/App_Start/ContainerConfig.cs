using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel;
using System.Reflection;
using System.Web.Http;

namespace MovieCRUD.Authentication.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            IServiceCollection services = new ServiceCollection();
            services.InstallTypesInAssembly();
            services.InstallGeneralTypes(Assembly.GetExecutingAssembly());

            var dependencyResolver = new WebApiDependencyResolver(services.BuildServiceProvider());
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
        }
    }
}