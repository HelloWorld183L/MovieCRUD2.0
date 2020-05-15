using AutoMapper;
using Moq;
using MovieCRUD.Movies;
using MovieCRUD.Controllers;
using MovieCRUD.DTOs;
using MovieCRUD.Tests.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MovieCRUD.Movies.Clients;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.SharedKernel;
using MovieCRUD.Movies.Requests;
using MovieCRUD.Movies.Responses;

namespace MovieCRUD.Tests
{
    [TestFixture]
    public class MovieControllerTest
    {
        private Mock<IMovieApiClient> _mockApiClient;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger> _mockLogger;
        private MovieController systemUnderTest;
        private const MockBehavior defaultMockBehavior = MockBehavior.Strict;
        private PaginationQuery _defaultQuery;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockApiClient = new Mock<IMovieApiClient>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger>();
            _defaultQuery = PaginationQuery.CreateQuery(1, 10);
        }

        [Test]
        public async Task List_ReturnsViewWithMovies()
        {
            var movieResponses = GetMovieResponses();

            _mockApiClient.Setup(x => x.GetAllMoviesAsync(It.IsAny<PaginationQuery>(), null))
                .Returns(Task.FromResult(movieResponses))
                .Verifiable();

            var mappedMovies = MapResponsesToDTOs(movieResponses);

            _mockMapper.Setup(x => x.Map<IEnumerable<MovieDTO>>(It.IsAny<IEnumerable<MovieResponse>>()))
                .Returns(mappedMovies);

            systemUnderTest = new MovieController(_mockApiClient.Object, _mockMapper.Object, _mockLogger.Object);

            var viewResult = await systemUnderTest.List() as ViewResult;

            _mockApiClient.Verify(x => x.GetAllMoviesAsync(It.IsAny<PaginationQuery>(), null), Times.Once());
        }

        [Test]
        public void Create_ReturnsCreateView()
        {
            systemUnderTest = new MovieController(_mockApiClient.Object, _mockMapper.Object, _mockLogger.Object);

            var viewResult = systemUnderTest.Create() as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "MovieDTOParameters2")]
        public async Task Create_MovieIsCreated(MovieDTO movie)
        {
            var mockMovieRequest = new CreateMovieRequest
            {
                Name = movie.Name,
                Genre = movie.Genre,
                Rating = (Rating)movie.Rating
            };

            _mockMapper.Setup(x => x.Map<CreateMovieRequest>(It.IsAny<MovieDTO>()))
                .Returns(mockMovieRequest);

            var createMovieRequest = _mockMapper.Object.Map<CreateMovieRequest>(movie);

            _mockApiClient.Setup(x => x.CreateMovieAsync(It.IsAny<CreateMovieRequest>()))
                .Returns(Task.FromResult(new MovieResponse()))
                .Verifiable();

            systemUnderTest = new MovieController(_mockApiClient.Object, _mockMapper.Object, _mockLogger.Object);

            await systemUnderTest.Create(movie);

            _mockApiClient.VerifyAll();
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "MovieDTOParameters2")]
        public async Task Create_RedirectsToAction(MovieDTO movie)
        {
            var mockMovieRequest = new CreateMovieRequest
            {
                Name = movie.Name,
                Genre = movie.Genre,
                Rating = (Rating)movie.Rating
            };

            _mockMapper.Setup(x => x.Map<CreateMovieRequest>(It.IsAny<MovieDTO>()))
                .Returns(mockMovieRequest);

            _mockApiClient.Setup(x => x.CreateMovieAsync(It.IsAny<CreateMovieRequest>()))
                .Returns(Task.FromResult(new MovieResponse()));

            systemUnderTest = new MovieController(_mockApiClient.Object, _mockMapper.Object, _mockLogger.Object);

            var viewResult = await systemUnderTest.Create(movie);

            Assert.IsNotNull(viewResult);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(6)]
        public async Task Details_MovieIsRetrieved(int id)
        {
            var movieResponse = new MovieResponse()
            {
                Id = id,
                Name = "Fast and Furious 8",
                Genre = "Action",
                Rating = Rating.Good
            };

            _mockApiClient.Setup(x => x.GetMovieAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(movieResponse))
                .Verifiable();

            var expectedMovie = new MovieDTO()
            {
                Id = movieResponse.Id,
                Name = movieResponse.Name,
                Genre = movieResponse.Genre,
                Rating = movieResponse.Rating
            };

            _mockMapper.Setup(x => x.Map<MovieDTO>(movieResponse))
                .Returns(expectedMovie)
                .Verifiable();

            systemUnderTest = new MovieController(_mockApiClient.Object, _mockMapper.Object, _mockLogger.Object);

            var result = await systemUnderTest.Details(id) as ViewResult;

            _mockApiClient.Verify(x => x.GetMovieAsync(id), Times.Once());
            _mockMapper.Verify(x => x.Map<MovieDTO>(movieResponse), Times.Once());

            Assert.IsNotNull(result.ViewData.Model);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(6)]
        public async Task Details_ReturnsDetailsViewWithMovie(int id)
        {
            var movieResponse = new MovieResponse()
            {
                Id = id,
                Name = "Fast and Furious 8",
                Genre = "Action",
                Rating = Rating.Good
            };

            _mockApiClient.Setup(x => x.GetMovieAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(movieResponse));

            var expectedMovie = new MovieDTO()
            {
                Id = movieResponse.Id,
                Name = movieResponse.Name,
                Genre = movieResponse.Genre,
                Rating = Rating.Good
            };

            _mockMapper.Setup(x => x.Map<MovieDTO>(movieResponse))
                .Returns(expectedMovie)
                .Verifiable();

            systemUnderTest = new MovieController(_mockApiClient.Object, _mockMapper.Object, _mockLogger.Object);

            var viewResult = await systemUnderTest.Details(id) as ViewResult;

            _mockApiClient.Verify(x => x.GetMovieAsync(id), Times.Once());

            Assert.IsNotNull(viewResult);
        }

        [Test]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(6)]
        public async Task Edit_ReturnsEditView(int id)
        {
            systemUnderTest = new MovieController(_mockApiClient.Object, _mockMapper.Object, _mockLogger.Object);

            var viewResult = await systemUnderTest.Edit(id) as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "MovieDTOParameters2")]
        public async Task Edit_MovieIsEdited(MovieDTO editedMovie)
        {
            var expectedMovie = new EditMovieRequest
            {
                Id = editedMovie.Id,
                Name = editedMovie.Name,
                Genre = editedMovie.Genre,
                Rating = (Rating)editedMovie.Rating
            };

            _mockMapper.Setup(x => x.Map<EditMovieRequest>(It.IsAny<MovieDTO>()))
                .Returns(expectedMovie);

            var editMovieRequest = _mockMapper.Object.Map<EditMovieRequest>(editedMovie);

            _mockApiClient.Setup(x => x.EditMovieAsync(It.IsAny<EditMovieRequest>()))
                .Returns(Task.FromResult(editedMovie))
                .Verifiable();

            await _mockApiClient.Object.EditMovieAsync(editMovieRequest);

            _mockApiClient.VerifyAll();
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "MovieResponseParameters2")]
        public void MapMovieResponsesToDTOs_ReturnsMappedMovies(IEnumerable<MovieResponse> movieResponses)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<MovieDTO>>(It.IsAny<IEnumerable<MovieResponse>>()))
                .Returns(GetMovieDTOs())
                .Verifiable();

            var mappedResponse = _mockMapper.Object.Map<IEnumerable<MovieDTO>>(movieResponses);

            _mockMapper.VerifyAll();
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "MovieDTOParameters2")]
        public void MapMovieDTOToCreateMovieRequest_MappingIsSuccessful(MovieDTO movie)
        {
            var expectedMovieRequest = new CreateMovieRequest()
            {
                Name = movie.Name,
                Genre = movie.Genre,
                Rating = (Rating)movie.Rating
            };

            _mockMapper.Setup(x => x.Map<CreateMovieRequest>(movie))
                .Returns(expectedMovieRequest)
                .Verifiable();

            var createMovieRequest = _mockMapper.Object.Map<CreateMovieRequest>(movie);

            _mockMapper.Verify(x => x.Map<CreateMovieRequest>(movie), Times.Once());
        }

        private IEnumerable<MovieResponse> GetMovieResponses()
        {
            var movieResponse1 = new MovieResponse
            {
                Id = 1,
                Name = "Harry Potter: Serpent Chamber",
                Genre = "Mystery",
                Rating = Rating.Terrible
            };

            var movieResponse2 = new MovieResponse
            {
                Id = 2,
                Name = "HELLO WORLD",
                Genre = "Sci-fi",
                Rating = Rating.Masterpiece
            };

            yield return movieResponse1;
            yield return movieResponse2;
        }

        private IEnumerable<MovieDTO> GetMovieDTOs()
        {
            var movieDto1 = new MovieDTO()
            {
                Id = 1,
                Name = "Harry Potter: The Philosopher's Stone",
                Genre = "Mystery",
                Rating = Rating.Good
            };

            var movieDto2 = new MovieDTO()
            {
                Id = 2,
                Name = "Harry Potter: Deathly Hallows",
                Genre = "Mystery/Action",
                Rating = Rating.Masterpiece
            };

            yield return movieDto1;
            yield return movieDto2;
        }

        private IEnumerable<MovieDTO> MapResponsesToDTOs(IEnumerable<MovieResponse> movieResponses)
        {
            var mappedMovies = new List<MovieDTO>();
            foreach (var expectedMovie in movieResponses)
            {
                var movieDto = new MovieDTO()
                {
                    Id = expectedMovie.Id,
                    Name = expectedMovie.Name,
                    Genre = expectedMovie.Genre,
                    Rating = Rating.Good
                };
                mappedMovies.Add(movieDto);
            }
            return mappedMovies.AsEnumerable();
        }
    }
}