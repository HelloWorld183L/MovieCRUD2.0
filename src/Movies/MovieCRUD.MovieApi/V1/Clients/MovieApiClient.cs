using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Movies.Requests;
using MovieCRUD.Movies.Requests.Queries;
using MovieCRUD.Movies.Responses;
using MovieCRUD.Movies.V1;
using MovieCRUD.SharedKernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MovieCRUD.Movies.Clients
{
    public class MovieApiClient : IMovieApiClient
    {
        private static HttpClient _restClient;
        private readonly ILogger _logger;
        private const int _portNumber = 54093;

        public MovieApiClient(ILogger logger)
        {
            _restClient = new HttpClient();
            _restClient.BaseAddress = new Uri($"http://localhost:{_portNumber}/");
            _restClient.DefaultRequestHeaders.Accept.Clear();
            _restClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _logger = logger;
        }

        public async Task CreateMovieAsync(CreateMovieRequest movie)
        {
            var serializedMovie = JsonConvert.SerializeObject(movie);
            _logger.LogInfo("Serializing CreateMovieRequest into JSON");

            var movieData = new StringContent(serializedMovie, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(MovieRoutes.Post, movieData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo("Sending HTTP POST request to create a movie");
        }

        public async Task<MovieResponse> GetMovieAsync(int movieId)
        {
            var response = await _restClient.GetAsync(MovieRoutes.Get + $"/{movieId}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP GET request to {MovieRoutes.Get}/{movieId}");

            var movie = await response.Content.ReadAsAsync<MovieResponse>();

            return movie;
        }

        public async Task<IEnumerable<MovieResponse>> GetAllMoviesAsync(PaginationQuery paginationQuery, GetAllByGenreQuery genreQuery = null)
        {
            string genreQuerySection = "";

            if (genreQuery != null)
            {
                genreQuerySection = $"genre={genreQuery.Genre}&";
            }

            var response = await _restClient.GetAsync($"{MovieRoutes.GetAll}?{genreQuerySection}pageIndex={paginationQuery.PageNumber}&pageSize={paginationQuery.PageSize}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP GET request to {MovieRoutes.GetAll} in order to get all movies");

            var movies = await response.Content.ReadAsAsync<IEnumerable<MovieResponse>>();

            return movies;
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            var response = await _restClient.DeleteAsync(MovieRoutes.Delete + $"/{movieId}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP DELETE request to {MovieRoutes.Delete}/{movieId}");
        }

        public async Task EditMovieAsync(EditMovieRequest newMovie)
        {
            var serializedMovie = JsonConvert.SerializeObject(newMovie);
            _logger.LogInfo("Serializing EditMovieRequest into JSON");

            var movieData = new StringContent(serializedMovie, Encoding.UTF8, "application/json");

            var response = await _restClient.PutAsync(MovieRoutes.Put, movieData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP PUT request to {MovieRoutes.Put} to edit the movie with an id of {newMovie.Id}");
        }
    }
}
