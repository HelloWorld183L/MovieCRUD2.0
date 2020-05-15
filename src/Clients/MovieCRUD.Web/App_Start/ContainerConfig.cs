using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Web;

namespace MovieCRUD.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            IServiceCollection services = new ServiceCollection();
            services.InstallTypesInAssembly();
            services.AddAutoMapper(typeof(ContainerConfig).Assembly);
        }
    }
}