using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel;
using MovieCRUD.SharedKernel.Enums;

namespace MovieCRUD.Infrastructure.Logging
{
    public class LoggingInstaller : IInstaller
    {
        public InstallOrder InstallOrder { get; set; } = InstallOrder.Important;

        public void InstallTypes(IServiceCollection services)
        {
            services.AddSingleton<ILogger, Logger>();
        }
    }
}
