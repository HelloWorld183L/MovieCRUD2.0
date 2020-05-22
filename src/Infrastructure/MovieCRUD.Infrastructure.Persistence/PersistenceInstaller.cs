using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel;
using MovieCRUD.SharedKernel.Enums;

namespace MovieCRUD.Infrastructure.Persistence
{
    public class PersistenceInstaller : IInstaller
    {
        public InstallOrder InstallOrder { get; set; } = InstallOrder.Important;

        public void InstallTypes(IServiceCollection services)
        {
            services.AddTransient<ApplicationDbContext>();
        }
    }
}
