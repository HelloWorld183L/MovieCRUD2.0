using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Authentication.Clients;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Infrastructure.Persistence.Services;
using MovieCRUD.SharedKernel;
using MovieCRUD.SharedKernel.Enums;

namespace MovieCRUD.Infrastructure.InversionOfControl.Installers
{
    public class AuthInstaller : IInstaller
    {
        public InstallOrder InstallOrder { get; set; } = InstallOrder.Trivial;

        public void InstallTypes(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>()
                    .AddSingleton<IAuthApiClient, AuthApiClient>(provider =>
                    {
                        return new AuthApiClient(new Logger());
                    });

        }
    }
}
