using Autofac;
using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Infrastructure.InversionOfControl.Installers.Interfaces;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Infrastructure.Persistence.Services;
using MovieCRUD.Movies.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.InversionOfControl.Installers
{
    public class MovieInstaller : IInstaller
    {
        public void InstallTypes(IServiceCollection services)
        {
            var logger = new Logger();
            services.AddTransient<IMovieRepository, MovieRepository>()
                    .AddSingleton<IMovieApiClient, MovieApiClient>(provider =>
                    {
                        return new MovieApiClient(logger);
                    });
        }
    }
}
