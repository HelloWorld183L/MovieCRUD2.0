using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Contracts.V1.Requests.Queries;
using MovieCRUD.Contracts.V1.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.Network.v1.Interfaces
{
    public interface IMovieApiClient
    {
        Task CreateMovieAsync(CreateMovieRequest movie);
        Task<MovieResponse> GetMovieAsync(int movieId);
        Task<IEnumerable<MovieResponse>> GetAllMoviesAsync(PaginationQuery paginationQuery, GetAllByGenreQuery genreQuery = null);
        Task DeleteMovieAsync(int movieId);
        Task EditMovieAsync(EditMovieRequest newMovie);
    }
}
