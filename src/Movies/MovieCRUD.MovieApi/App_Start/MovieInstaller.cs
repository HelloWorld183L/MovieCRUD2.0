using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Infrastructure.Persistence.Services;
using MovieCRUD.Movies.Clients;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.MovieApi
{
    public class MovieInstaller : IInstaller
    {
        public void InstallTypes(IServiceCollection services)
        {
            services.AddTransient<IMovieRepository, MovieRepository>()
                    .AddSingleton<IMovieApiClient, MovieApiClient>(provider =>
                    {
                        return new MovieApiClient(new Logger());
                    });
        }
    }
}
