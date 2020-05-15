using MovieCRUD.Movies.Requests;
using MovieCRUD.Movies.Requests.Queries;
using MovieCRUD.Movies.Responses;
using MovieCRUD.SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCRUD.Movies.Clients
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
