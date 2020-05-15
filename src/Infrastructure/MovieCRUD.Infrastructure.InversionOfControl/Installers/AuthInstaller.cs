using Autofac;
using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Authentication.Clients;
using MovieCRUD.Infrastructure.InversionOfControl.Installers.Interfaces;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Infrastructure.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.InversionOfControl.Installers
{
    public class AuthInstaller : IInstaller
    {
        public void InstallTypes(IServiceCollection services)
        {
            var logger = new Logger();
            services.AddTransient<IUserRepository, UserRepository>()
                    .AddSingleton<IAuthApiClient, AuthApiClient>(provider =>
                    {
                        return new AuthApiClient(logger);
                    });
        }
    }
}
