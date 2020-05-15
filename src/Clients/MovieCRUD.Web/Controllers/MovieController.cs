using AutoMapper;
using MovieCRUD.DTOs;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Movies.Clients;
using MovieCRUD.Movies.Requests;
using MovieCRUD.Movies.Responses;
using MovieCRUD.SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MovieCRUD.Controllers
{
    [RequireHttps]
    [Authorize]
    public class MovieController : Controller
    {
        private IMovieApiClient _movieApiClient;
        private IMapper _mapper;
        private ILogger _logger;

        public MovieController(IMovieApiClient apiClient, IMapper mapper, ILogger logger)
        {
            _movieApiClient = apiClient;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            var movieResponse = await _movieApiClient.GetAllMoviesAsync(PaginationQuery.CreateQuery(1, 10));

            var mappedMovies = MapResponsesToDTOs(movieResponse);

            _logger.LogInfo("Returning the list view with the list of movie DTOs");
            return View(mappedMovies);
        }
         
        [HttpGet]
        public ActionResult Create()
        {
            _logger.LogInfo("Returning the create view");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(MovieDTO movie)
        {
            var createMovieRequest = _mapper.Map<CreateMovieRequest>(movie);
            _logger.LogInfo("Mapping the MovieDTO to a CreateMovieRequest");

            await _movieApiClient.CreateMovieAsync(createMovieRequest);

            _logger.LogInfo("Redirecting to the List view");
            return RedirectToAction("List");
        }

        public async Task<ActionResult> Details(int id)
        {
            var movieResponse = await _movieApiClient.GetMovieAsync(id);

            var movie = _mapper.Map<MovieDTO>(movieResponse);
            _logger.LogInfo("Mapping the movie response to the movie DTO...");

            _logger.LogInfo($"Returning the details view with movie ID {id}");
            return View(movie);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var movie = await _movieApiClient.GetMovieAsync(id);

            var mappedMovie = _mapper.Map<MovieDTO>(movie);
            _logger.LogInfo("Mapping the movie domain object to MovieDTO");

            _logger.LogInfo($"Returning the edit view with movie ID {id}");
            return View(mappedMovie);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(MovieDTO editedMovie)
        {
            var editMovieRequest = _mapper.Map<EditMovieRequest>(editedMovie);
            _logger.LogInfo("Mapping the MovieDTO to EditMovieRequest");

            await _movieApiClient.EditMovieAsync(editMovieRequest);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var movieResponse = await _movieApiClient.GetMovieAsync(id);

            var mappedMovie = _mapper.Map<MovieDTO>(movieResponse);
            _logger.LogInfo("Mapping MovieResponse to MovieDTO");

            _logger.LogInfo($"Returning the delete view with the mapped movie (ID: {id})");
            return View(mappedMovie);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(MovieDTO movie)
        {
            await _movieApiClient.DeleteMovieAsync(movie.Id);

            _logger.LogInfo("Returning to the list view");
            return RedirectToAction("List");
        }

        private IEnumerable<MovieDTO> MapResponsesToDTOs(IEnumerable<MovieResponse> movieResponses)
        {
            var movieDTOs = new List<MovieDTO>();

            foreach (var movieResponse in movieResponses)
            {
                var mappedMovie = _mapper.Map<MovieDTO>(movieResponse);
                _logger.LogInfo($"Mapping the movie response (ID: {movieResponse.Id}) to a movie DTO");

                movieDTOs.Add(mappedMovie);
            }

            return movieDTOs;
        }
    }
}