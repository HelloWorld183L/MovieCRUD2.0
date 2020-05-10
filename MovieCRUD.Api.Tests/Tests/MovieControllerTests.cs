using Autofac.Extras.Moq;
using AutoMapper;
using Moq;
using MovieCRUD.Api.Tests.TestData;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Contracts.V1.Requests.Queries;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Domain;
using MovieCRUD.Domain.Filters;
using MovieCRUD.Infrastructure.Enums;
using MovieCRUD.Infrastructure.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovieCRUD.Api.Tests
{
    public class MovieControllerTests
    {
        private Mock<IMapper> _mockMapper;
        private Mock<IMovieRepository> _mockRepository;
        private const int defaultPageSize = 10;
        private const int defaultPageIndex = 1;

        [SetUp]
        public void Setup()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IMovieRepository>();
        }

        [Test]
        [TestCase("Superhero")]
        [TestCase("Action", 1, 2)]
        [TestCase("Mystery", 2, 3)]
        public void GetMoviesByGenre_RetrievalSuccessful(string genre, int pageIndex = defaultPageIndex, int pageSize = defaultPageSize)
        {
            var paginationFilter = new PaginationFilter() { PageSize = pageSize, PageNumber = pageIndex };

            var genreFilter = new GetAllByGenreFilter() { Genre = genre };

            var expectedMovies = GetExpectedMovies(genre, paginationFilter);

            _mockRepository.Setup(x => x.GetAll(It.IsAny<PaginationFilter>(), It.IsAny<GetAllByGenreFilter>()))
                .Returns(expectedMovies)
                .Verifiable();

            var actualMovies = _mockRepository.Object.GetAll(paginationFilter, genreFilter);

            Assert.AreEqual(expectedMovies, actualMovies);
            _mockRepository.Verify(x => x.GetAll(paginationFilter, genreFilter), Times.Once());
        }

        [Test]
        [TestCase("Superhero")]
        [TestCase("Action")]
        [TestCase("Mystery")]
        public void GetMoviesByGenre_GenreIsMappedSuccessful(string genre)
        {
            var expectedGenreFilter = new GetAllByGenreFilter() { Genre = genre };

            _mockMapper.Setup(x => x.Map<GetAllByGenreFilter>(It.IsAny<GetAllByGenreQuery>()))
                .Returns(expectedGenreFilter);

            var genreQuery = new GetAllByGenreQuery(genre);
            var genreFilter = _mockMapper.Object.Map<GetAllByGenreFilter>(genreQuery);

            Assert.AreEqual(expectedGenreFilter, genreFilter);
        }

        [Test]
        [TestCase(1, 5)]
        [TestCase(2, 10)]
        public void GetMovies_ResponseIsMappedSuccessful(int pageIndex = defaultPageIndex, int pageSize = defaultPageSize)
        {
            var movie = new Movie()
            {
                Id = 1,
                Name = "Shawshank Redemption",
                Genre = "Prison drama",
                Rating = Domain.Rating.Good
            };

            var expectedMovie = new MovieResponse()
            {
                Id = movie.Id,
                Name = movie.Name,
                Genre = movie.Genre,
                Rating = (Rating)movie.Rating
            };

            _mockMapper.Setup(x => x.Map<MovieResponse>(It.IsAny<Movie>()))
                .Returns(expectedMovie);

            var actualMovie = _mockMapper.Object.Map<MovieResponse>(movie);

            Assert.AreEqual(expectedMovie, actualMovie);
        }

        [Test]
        [TestCase(1, 5)]
        [TestCase(2, 10)]
        public void GetMovies_RetrievalSuccessful(int pageIndex = defaultPageIndex, int pageSize = defaultPageSize)
        {
            var paginationFilter = new PaginationFilter() { PageNumber = pageIndex, PageSize = pageSize };
            
            _mockRepository.Setup(x => x.GetAll(paginationFilter, null))
                .Returns(GetExpectedMovies(null, paginationFilter));

            var expectedMovies = _mockRepository.Object.GetAll(paginationFilter);

            var actualMovies = GetExpectedMovies(null, paginationFilter);

            for (int i = 0; i < expectedMovies.Count(); i++)
            {
                var expectedMovie = expectedMovies.ElementAt(i);
                var actualMovie = actualMovies.ElementAt(i);

                Assert.AreEqual(expectedMovie.Id, actualMovie.Id);
            }
        }

        [Test]
        [TestCase(1, 5)]
        public void GetMovies_QueryToFilterMappingSuccessful(int pageIndex, int pageSize)
        {
            var paginationQuery = PaginationQuery.CreateQuery(pageIndex, pageSize);

            var expectedFilter = new PaginationFilter()
            {
                PageSize = pageSize,
                PageNumber = pageIndex
            };

            _mockMapper.Setup(x => x.Map<PaginationFilter>(paginationQuery))
                .Returns(expectedFilter);

            var actualFilter = _mockMapper.Object.Map<PaginationFilter>(paginationQuery);

            Assert.AreEqual(expectedFilter, actualFilter);
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void GetMovie_RetrievalSuccessful(int movieId)
        {
            var expectedMovie = GetMovie(movieId);

            _mockRepository.Setup(x => x.Get(It.IsAny<int>()))
                .Returns(expectedMovie);

            var movie = _mockRepository.Object.Get(movieId);

            Assert.IsNotNull(movie);
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "GetMovieParameterTestData")]
        public void GetMovie_DomainObjectToResponseMappingIsSuccessful(Movie movie)
        {
            var expectedMovie = new MovieResponse()
            {
                Id = movie.Id,
                Name = movie.Name,
                Genre = movie.Genre,
                Rating = (Rating)movie.Rating
            };

            _mockMapper.Setup(x => x.Map<MovieResponse>(It.IsAny<Movie>()))
                .Returns(expectedMovie);

            var actualMovie = _mockMapper.Object.Map<MovieResponse>(movie);

            Assert.AreEqual(expectedMovie, actualMovie);
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "CreateMovieParameterTestData")]
        public void CreateMovie_MovieIsCreated(CreateMovieRequest movieRequest)
        {
            var movie = new Movie()
            {
                Id = 0,
                Name = movieRequest.Name,
                Genre = movieRequest.Genre,
                Rating = (Domain.Rating)movieRequest.Rating
            };

            _mockRepository.Setup(x => x.Create(It.IsAny<Movie>()));

            _mockRepository.Object.Create(movie);

            _mockRepository.Verify(x => x.Create(movie), Times.Once());
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "CreateMovieParameterTestData")]
        public void CreateMovie_CreateMovieRequestToDomainObjectMappingIsSuccessful(CreateMovieRequest movieRequest)
        {
            var expectedMovie = new Movie()
            {
                Id = 0,
                Name = movieRequest.Name,
                Genre = movieRequest.Genre,
                Rating = (Domain.Rating)movieRequest.Rating
            };

            _mockMapper.Setup(x => x.Map<Movie>(It.IsAny<CreateMovieRequest>()))
                .Returns(expectedMovie);

            var actualMovie = _mockMapper.Object.Map<Movie>(movieRequest);

            Assert.AreEqual(expectedMovie, actualMovie);
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "EditMovieParameterTestData")]
        public void EditMovie_MovieIsEdited(EditMovieRequest editMovieRequest)
        {
            var editedMovie = new Movie()
            {
                Id = editMovieRequest.Id,
                Name = editMovieRequest.Name,
                Genre = editMovieRequest.Genre,
                Rating = (Domain.Rating)editMovieRequest.Rating
            };

            _mockRepository.Setup(x => x.Edit(editedMovie));

            var originalMovie = _mockRepository.Object.Get(editedMovie.Id);

            _mockRepository.Object.Edit(editedMovie);

            _mockRepository.Verify(x => x.Edit(editedMovie), Times.Exactly(1));

            Assert.AreNotEqual(editedMovie, originalMovie);
        }

        [Test]
        [TestCaseSource(typeof(MovieControllerTestData), "EditMovieParameterTestData")]
        public void EditMovie_EditMovieRequestToDomainObjectMappingIsSuccessful(EditMovieRequest movieRequest)
        {
            var expectedMovie = new Movie()
            {
                Id = movieRequest.Id,
                Name = movieRequest.Name,
                Genre = movieRequest.Genre,
                Rating = (Domain.Rating)movieRequest.Rating
            };

            _mockMapper.Setup(x => x.Map<Movie>(It.IsAny<EditMovieRequest>()))
                .Returns(expectedMovie);

            var actualMovie = _mockMapper.Object.Map<Movie>(movieRequest);

            Assert.AreEqual(expectedMovie, actualMovie);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void DeleteMovie_MovieIsDeleted(int movieId)
        {
            _mockRepository.Setup(x => x.Delete(It.IsAny<int>()))
                .Verifiable();

            _mockRepository.Object.Delete(movieId);

            var movieToRetrieve = _mockRepository.Object.Get(movieId);

            _mockRepository.Verify(x => x.Delete(movieId), Times.Once());
            Assert.IsNull(movieToRetrieve);
        }

        private IEnumerable<Movie> GetExpectedMovies(string genre, PaginationFilter paginationFilter)
        {
            var expectedMovies = new List<Movie>()
            {
                new Movie()
                {
                    Id = 1,
                    Name = "Iron man 1",
                    Genre = "Superhero",
                    Rating = Domain.Rating.Good
                },
                new Movie()
                {
                    Id = 2,
                    Name = "Iron man 2",
                    Genre = "Superhero",
                    Rating = Domain.Rating.Bad
                },
                new Movie()
                {
                    Id = 3,
                    Name = "Shawshank Redemption",
                    Genre = "Drama",
                    Rating = Domain.Rating.Masterpiece
                },
                 new Movie()
                {
                    Id = 4,
                    Name = "Friends: The Movie",
                    Genre = "Sitcom",
                    Rating = Domain.Rating.Terrible
                },
                  new Movie()
                {
                    Id = 5,
                    Name = "One Punch Man: The Live-Action Movie",
                    Genre = "Shonen",
                    Rating = Domain.Rating.Mediocore
                },
                new Movie()
                {
                    Id = 6,
                    Name = "Hello world",
                    Genre = "Adventure",
                    Rating = Domain.Rating.Good
                },
                new Movie()
                {
                    Id = 7,
                    Name = "Batman: The Brave and The Bold",
                    Genre = "Action",
                    Rating = Domain.Rating.Masterpiece
                }
            };

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            if (genre != null)
            {
                return expectedMovies.Where(movie => movie.Genre == genre).Skip(skip).Take(paginationFilter.PageSize);
            }

            return expectedMovies.OrderBy(x => x.Id).Skip(skip).Take(paginationFilter.PageSize);
        }

        private Movie GetMovie(int movieId)
        {
            var paginationFilter = new PaginationFilter() { PageNumber = 1, PageSize = 10 };

            var movie = GetExpectedMovies(null, paginationFilter).FirstOrDefault(x => x.Id == movieId);
            return movie;
        }

        private IEnumerable<MovieResponse> MapMoviesToResponses(IEnumerable<Movie> movies)
        {
            var movieResponses = new List<MovieResponse>();

            foreach (var movie in movies)
            {
                var movieResponse = new MovieResponse()
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Genre = movie.Genre,
                    Rating = (Rating)movie.Rating
                };
                movieResponses.Add(movieResponse);
            }

            return movieResponses.AsEnumerable();
        }
    }
}