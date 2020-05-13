using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using MovieCRUD.Domain.Movies;
using MovieCRUD.SharedKernel;
using MovieCRUD.Infrastructure.Persistence.Entities;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Infrastructure.Logging;

namespace MovieCRUD.Infrastructure.Persistence.Services
{
    public class MovieRepository : Repository<Movie, MovieEntity>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger) { }

        public IEnumerable<Movie> GetAll(PaginationFilter paginationFilter, GetAllByGenreFilter filter = null)
        {
            var queryable = _entitySet.AsQueryable();
            IQueryable<MovieEntity> entities = _entitySet;

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            entities = queryable.OrderBy(e => e.Id).Skip(skip).Take(paginationFilter.PageSize);
            _logger.LogInfo("Applied the pagination filter successfully");

            var filteredEntities = AddGenreFilterOnQuery(filter, entities);
            _logger.LogInfo("Added genre filter onto the pagedEntities");

            var mappedEntities = _mapper.Map<IEnumerable<Movie>>(filteredEntities);
            _logger.LogInfo($"Mapped entities (Type: {filteredEntities.GetType().Name}) to domain models (Type: {mappedEntities.GetType().Name})");

            _logger.LogInfo("Returning movie entities to be displayed");
            return mappedEntities;
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
    }
}
