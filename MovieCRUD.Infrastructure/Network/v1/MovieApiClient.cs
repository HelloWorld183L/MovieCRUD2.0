using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Contracts.V1.Requests.Queries;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Network.v1.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.Network
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

            var response = await _restClient.PostAsync(MovieApiRoutes.MovieRoutes.Post, movieData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo("Sending HTTP POST request to create a movie");
        }

        public async Task<MovieResponse> GetMovieAsync(int movieId)
        {
            var response = await _restClient.GetAsync(MovieApiRoutes.MovieRoutes.MovieBase + $"/{movieId}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP GET request to {MovieApiRoutes.MovieRoutes.Get}/{movieId}");

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

            var response = await _restClient.GetAsync($"{MovieApiRoutes.MovieRoutes.GetAll}?{genreQuerySection}pageIndex={paginationQuery.PageNumber}&pageSize={paginationQuery.PageSize}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP GET request to {MovieApiRoutes.MovieRoutes.GetAll} in order to get all movies");

            var movies = await response.Content.ReadAsAsync<IEnumerable<MovieResponse>>();

            return movies;
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            var response = await _restClient.DeleteAsync(MovieApiRoutes.MovieRoutes.Delete + $"/{movieId}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP DELETE request to {MovieApiRoutes.MovieRoutes.Delete}/{movieId}");
        }

        public async Task EditMovieAsync(EditMovieRequest newMovie)
        {
            var serializedMovie = JsonConvert.SerializeObject(newMovie);
            _logger.LogInfo("Serializing EditMovieRequest into JSON");

            var movieData = new StringContent(serializedMovie, Encoding.UTF8, "application/json");

            var response = await _restClient.PutAsync(MovieApiRoutes.MovieRoutes.Put, movieData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP PUT request to {MovieApiRoutes.MovieRoutes.Put} to edit the movie with an id of {newMovie.Id}");
        }
    }
}
