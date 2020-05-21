using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel;
using System.Reflection;

namespace MovieCRUD.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            IServiceCollection services = new ServiceCollection();

            var executingAssembly = Assembly.GetExecutingAssembly();
            services.InstallTypesInAssembly();
            services.AddAutoMapper(executingAssembly);
        }
    }
}