using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel;
using System;

namespace MovieCRUD.Infrastructure.Persistence
{
    public class PersistenceInstaller : IInstaller
    {
        public void InstallTypes(IServiceCollection services)
        {
            services.AddTransient<ApplicationDbContext>();
        }
    }
}
