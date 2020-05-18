using AutoMapper;
using MovieCRUD.Domain;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Infrastructure.Persistence.Entities;
using MovieCRUD.Infrastructure.Tests.Persistence.Repositories.TestData;
using MovieCRUD.SharedKernel;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovieCRUD.Infrastructure.Tests.Persistence.Repositories
{
    public class MovieRepositoryTests
    {
        protected IMapper _mapper;
        protected IEnumerable<MovieEntity> _entitySet;

        [SetUp]
        public void SetUp()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddMaps(Assembly.GetExecutingAssembly());
            });
            _mapper = new Mapper(mapperConfiguration);
            _entitySet = GetMovies();
        }

        [Test]
        [TestCaseSource(typeof(MovieRepositoryTestData), "PaginationFilterParameters")]
        public void GetAll_ReturnsMovies(PaginationFilter filter)
        {
            var skip = (filter.PageNumber - 1) * filter.PageSize;

            var queryable = _entitySet.AsQueryable();
            var entities = queryable.OrderBy(e => e.Id).Skip(skip).Take(filter.PageSize);

            var mappedEntities = _mapper.Map<IEnumerable<Movie>>(entities);

            Assert.IsNotEmpty(mappedEntities);
        }

        [Test]
        [TestCaseSource(typeof(MovieRepositoryTestData), "PaginationFilterParameters")]
        public void GetAll_PaginationFilterApplied(PaginationFilter filter)
        {
            var skip = (filter.PageNumber - 1) * filter.PageSize;

            var queryable = _entitySet.AsQueryable();
            var movies = queryable.OrderBy(e => e.Id).Skip(skip).Take(filter.PageSize);

            var firstMovie = movies.ElementAt(0);

            Assert.True(firstMovie.Id > skip);
        }

        [Test]
        [TestCaseSource(typeof(MovieRepositoryTestData), "PaginationFilterAndGenreFilterParameters")]
        public void GetAll_GenreFilterApplied(PaginationFilter filter, GetAllByGenreFilter genreFilter)
        {
            var skip = (filter.PageNumber - 1) * filter.PageSize;

            var queryable = _entitySet.AsQueryable();
            var pagedMovies = queryable.OrderBy(e => e.Id).Skip(skip).Take(filter.PageSize);

            var filteredEntities = AddGenreFilterOnQuery(genreFilter, pagedMovies);

            foreach (var entity in filteredEntities)
            {
                Assert.True(entity.Genre == genreFilter.Genre);
            }
        }

        private static IEnumerable<MovieEntity> AddGenreFilterOnQuery(GetAllByGenreFilter filter, IEnumerable<MovieEntity> enumerable)
        {
            if (!string.IsNullOrEmpty(filter?.Genre))
            {
                enumerable = from entity in enumerable
                             where entity.Genre == filter.Genre
                             select entity;
            }
            return enumerable;
        }

        private IEnumerable<MovieEntity> GetMovies()
        {
            return new List<MovieEntity>
            {
                new MovieEntity
                {
                    Id = 1,
                    Name = "Sharknado",
                    Genre = "Comedy",
                    Rating = Rating.Bad
                },
                new MovieEntity
                {
                    Id = 2,
                    Name = "Captain America: Winter Soldier",
                    Genre = "Superhero",
                    Rating = Rating.Terrible
                },
                new MovieEntity
                {
                    Id = 3,
                    Name = "The Matrix",
                    Genre = "Action",
                    Rating = Rating.Masterpiece
                },
                new MovieEntity
                {
                    Id = 4,
                    Name = "The Matrix 2",
                    Genre = "Action",
                    Rating = Rating.Masterpiece
                },
                new MovieEntity
                {
                    Id = 5,
                    Name = "The Matrix 3",
                    Genre = "Action",
                    Rating = Rating.Mediocore
                },
                new MovieEntity
                {
                    Id = 6,
                    Name = "Sharknado 2",
                    Genre = "Comedy",
                    Rating = Rating.Good
                },
            };
        }
    }
}
