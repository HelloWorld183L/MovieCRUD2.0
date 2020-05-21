using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Infrastructure.Logging
{
    public class LoggingInstaller : IInstaller
    {
        public void InstallTypes(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
        }
    }
}
