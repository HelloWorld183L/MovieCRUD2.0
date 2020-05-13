using AutoMapper;
using Moq;
using MovieCRUD.Desktop.Models.DTOs;
using MovieCRUD.Desktop.Tests.TestData;
using MovieCRUD.Desktop.ViewModels;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Movies.Clients;
using MovieCRUD.Movies.Responses;
using NUnit.Framework;
using System.Collections.Generic;

namespace MovieCRUD.Desktop.Tests.Tests
{
    public class MovieCRUDViewModelTest
    {
        private Mock<ILogger> _logger;
        private Mock<IMovieApiClient> _mockApiClient;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger>();
            _mockMapper = new Mock<IMapper>();
            _mockApiClient = new Mock<IMovieApiClient>();
        }

        [Test]
        [TestCaseSource(typeof(MovieCRUDViewModelTestData), "MovieResponseParameters")]
        public void MapResponsesToDtos_IsSuccessful(IEnumerable<MovieResponse> movieResponses)
        {
            foreach (var movieResponse in movieResponses)
            {
                var expectedMovieDto = new MovieDTO()
                {
                    Id = movieResponse.Id,
                    Name = movieResponse.Name,
                    Genre = movieResponse.Genre,
                    Rating = movieResponse.Rating
                };

                _mockMapper.Setup(x => x.Map<MovieDTO>(movieResponse))
                    .Returns(expectedMovieDto);

                var actualMovieDto = _mockMapper.Object.Map<MovieDTO>(movieResponse);

                Assert.AreNotEqual(actualMovieDto, movieResponses);
                Assert.IsNotNull(actualMovieDto);
            }
        }
    }
}
