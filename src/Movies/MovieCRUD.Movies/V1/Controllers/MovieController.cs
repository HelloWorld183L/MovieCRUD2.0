using AutoMapper;
using MovieCRUD.Infrastructure.Logging;
using System.Collections.Generic;
using System.Web.Http;
using MovieCRUD.Movies.Requests;
using MovieCRUD.Movies.Responses;
using MovieCRUD.Movies.V1;
using MovieCRUD.SharedKernel;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Requests.Queries;
using MovieCRUD.Infrastructure.Persistence.Interfaces;

namespace MovieCRUD.Movies.V1.Controllers
{
    [Authorize]
    public class MovieController : ApiController
    {
        private IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private const int defaultPageSize = 10;
        private const int defaultPageIndex = 1;

        public MovieController(IMovieRepository movieRepository, IMapper mapper, ILogger logger)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route(MovieRoutes.GetAll)]
        public IEnumerable<MovieResponse> GetMovies(string genre, int pageIndex = defaultPageIndex, int pageSize = defaultPageSize)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(PaginationQuery.CreateQuery(pageIndex, pageSize));
            var genreFilter = _mapper.Map<GetAllByGenreFilter>(new GetAllByGenreQuery(genre));

            var movies = _movieRepository.GetAll(paginationFilter, genreFilter);
            _logger.LogInfo("Getting all movies from a movie repository");

            var mappedMovies = _mapper.Map<IEnumerable<MovieResponse>>(movies);
            _logger.LogInfo("Mapping movie responses to movie domain objects");

            return mappedMovies;
        }

        [HttpGet]
        [Route(MovieRoutes.GetAll)]
        public IEnumerable<MovieResponse> GetMovies(int pageIndex = defaultPageIndex, int pageSize = defaultPageSize)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(PaginationQuery.CreateQuery(pageIndex, pageSize));

            var movies = _movieRepository.GetAll(paginationFilter);
            _logger.LogInfo("Getting all movies from a movie repository");

            var mappedMovies = _mapper.Map<IEnumerable<MovieResponse>>(movies);
            _logger.LogInfo("Mapping movie responses to movie domain objects");

            return mappedMovies;
        }

        [HttpGet]
        [Route(MovieRoutes.Get)]
        public MovieResponse GetMovie([FromUri]int movieId)
        {
            var movie = _movieRepository.Get(movieId);

            var mappedMovie = _mapper.Map<MovieResponse>(movie);
            _logger.LogInfo("Mapping Movie domain object to MovieResponse");

            return mappedMovie;
        }

        [HttpPost]
        [Route(MovieRoutes.Post)]
        public void CreateMovie(CreateMovieRequest movie)
        {
            var mappedMovie = _mapper.Map<Movie>(movie);
            _logger.LogInfo("Mapping the CreateMovieRequest to a Movie domain object");

            _movieRepository.Create(mappedMovie);
        }

        [HttpPut]
        [Route(MovieRoutes.Put)]
        public void EditMovie([FromBody]EditMovieRequest newMovie)
        {
            var mappedMovie = _mapper.Map<Movie>(newMovie);
            _logger.LogInfo("Mapping the EditMovieRequest to a Movie domain object");

            _movieRepository.Edit(mappedMovie);
        }

        [HttpDelete]
        [Route(MovieRoutes.Delete)]
        public void DeleteMovie([FromUri]int movieId)
        {
            _movieRepository.Delete(movieId);
        }
    }
}