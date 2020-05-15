using NUnit.Framework;
using System.Threading.Tasks;
using MovieCRUD.Infrastructure.Tests.Network.v1.TestData;
using Moq;
using System.Collections.Generic;
using MovieCRUD.Movies.Clients;
using MovieCRUD.SharedKernel;
using MovieCRUD.Movies.Requests;
using MovieCRUD.Movies.Responses;

namespace MovieCRUD.Infrastructure.Tests.Network.v1
{
    public class MovieApiClientTests
    {
        private Mock<IMovieApiClient> _mockApiClient;
        private PaginationQuery _defaultQuery;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockApiClient = new Mock<IMovieApiClient>();
            var paginationQuery = new PaginationQuery() { PageNumber = 1, PageSize = 5 };
        }

        [Test]
        [TestCaseSource(typeof(MovieApiClientTestData), "CreateMovieParameterTestData")]
        public async Task CreateMovieAsync_IsSuccessful(CreateMovieRequest movie)
        {
            _mockApiClient.Setup(x => x.CreateMovieAsync(It.IsAny<CreateMovieRequest>()))
                .Verifiable();

            await _mockApiClient.Object.CreateMovieAsync(movie);

            _mockApiClient.Verify(x => x.CreateMovieAsync(movie), Times.Once());
        }

        [Test]
        [TestCaseSource(typeof(MovieApiClientTestData), "GetMovieParameterTestData")]
        public async Task GetMovieAsync_ReturnsMovie(MovieResponse movie)
        {
            _mockApiClient.Setup(x => x.GetMovieAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(movie))
                .Verifiable();

            await _mockApiClient.Object.GetMovieAsync(movie.Id);

            _mockApiClient.Verify(x => x.GetMovieAsync(movie.Id), Times.Once());
        }

        [Test]
        [TestCaseSource(typeof(MovieApiClientTestData), "GetAllMoviesParameters")]
        public async Task GetAllMoviesAsync_IsSuccessful(IEnumerable<MovieResponse> expectedMovies)
        {
            _mockApiClient.Setup(x => x.GetAllMoviesAsync(_defaultQuery, null))
                .Returns(Task.FromResult(expectedMovies))
                .Verifiable();

            await _mockApiClient.Object.GetAllMoviesAsync(_defaultQuery);

            _mockApiClient.Verify(x => x.GetAllMoviesAsync(_defaultQuery, null), Times.Once());
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public async Task DeleteMovieAsync_IsSuccessful(int movieId)
        {
            _mockApiClient.Setup(x => x.DeleteMovieAsync(It.IsAny<int>()))
                .Verifiable();

            await _mockApiClient.Object.DeleteMovieAsync(movieId);

            _mockApiClient.Verify(x => x.DeleteMovieAsync(movieId), Times.Once());
        }

        [Test]
        [TestCaseSource(typeof(MovieApiClientTestData), "EditMovieParameterTestData")]
        public async Task EditMovieAsync_IsSuccessful(EditMovieRequest editedMovie)
        {
            _mockApiClient.Setup(x => x.EditMovieAsync(It.IsAny<EditMovieRequest>()))
                .Verifiable();

            await _mockApiClient.Object.EditMovieAsync(editedMovie);

            _mockApiClient.Verify(x => x.EditMovieAsync(editedMovie), Times.Once());
        }
    }
}
