using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Infrastructure.Persistence.Services;
using MovieCRUD.Movies.Clients;
using MovieCRUD.SharedKernel;
using MovieCRUD.SharedKernel.Enums;

namespace MovieCRUD.MovieApi
{
    public class MovieInstaller : IInstaller
    {
        public InstallOrder InstallOrder { get; set; } = InstallOrder.Trivial;

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
