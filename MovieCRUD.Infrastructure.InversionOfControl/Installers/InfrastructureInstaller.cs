using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Infrastructure.InversionOfControl.Installers.Interfaces;
using MovieCRUD.Infrastructure.Logging;

namespace MovieCRUD.Infrastructure.InversionOfControl.Installers
{
    public class InfrastructureInstaller : IInstaller
    {
        public void InstallTypes(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
            services.AddTransient<ApplicationDbContext>();
        }
    }
}
