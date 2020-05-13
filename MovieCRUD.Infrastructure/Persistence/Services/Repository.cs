using AutoMapper;
using MovieCRUD.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Infrastructure.Services
{
    public class Repository<TDomainModel, TEntity> : IRepository<TDomainModel> 
        where TDomainModel : IAggregate
        where TEntity : class, IEntity
    {
        protected readonly DbSet<TEntity> _entitySet;
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly ICollection<TEntity> _cachedEntities;
        protected readonly string _entityTypeName;

        public Repository(ApplicationDbContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _entitySet = context.Set<TEntity>();
            _logger = logger;
            _entityTypeName = typeof(TEntity).Name;
            _cachedEntities = new List<TEntity>();
        }

        public IEnumerable<TDomainModel> GetAll(PaginationFilter paginationFilter)
        {
            if (paginationFilter == null) return null;

            var queryable = _entitySet.AsQueryable();
            IEnumerable<TEntity> pagedEntities = _entitySet;

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            pagedEntities = queryable.OrderBy(x => x.Id == x.Id).Skip(skip).Take(paginationFilter.PageSize);
            _logger.LogInfo("Applied the pagination filter successfully");

            var mappedEntities = _mapper.Map<IEnumerable<TDomainModel>>(pagedEntities);
            _logger.LogInfo($"Mapped entities (Type: {_entityTypeName}) to domain models (Type: {mappedEntities.GetType().Name})");

            _logger.LogInfo("Returning movie entities to be displayed");
            return mappedEntities;
        }

        public TDomainModel Get(int id)
        {
            if (id <= 0) return default;

            TEntity entity = _cachedEntities.FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                entity = _entitySet.FirstOrDefault(e => e.Id == id);
                if (entity == null) return default;

                _logger.LogInfo($"Retrieved an entity (Type: {_entityTypeName}) from the entity set");

                _cachedEntities.Add(entity);
                _logger.LogInfo($"Added an entity (Type: {_entityTypeName}) to the entity cache");
            }

            _logger.LogInfo($"Found an entity (Type: {_entityTypeName}) with an ID: {id}");

            var mappedEntity = _mapper.Map<TDomainModel>(entity);
            _logger.LogInfo($"Mapped an entity (Type: {_entityTypeName}) to a domain model (Type: {mappedEntity.GetType().Name})");

            return mappedEntity;
        }

        public void Create(TDomainModel movie)
        {
            var entity = _mapper.Map<TEntity>(movie);
            _logger.LogInfo($"Mapped a domain model (Type: {movie.GetType().Name}) to a entity (Type: {_entityTypeName})");

            _entitySet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            if (id <= 0) return;

            var entity = _cachedEntities.FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                entity = _entitySet.FirstOrDefault(e => e.Id == id);
                _logger.LogInfo($"Retrieved an entity (Type: {_entityTypeName}) from the entity set");

                _cachedEntities.Add(entity);
                _logger.LogInfo($"Added an entity (Type: {_entityTypeName}) to the entity cache");
            }
            _entitySet.Remove(entity);
            _context.SaveChanges();
        }

        public void Edit(TDomainModel editedMovie)
        {
            var mappedMovie = _mapper.Map<TEntity>(editedMovie);
            _logger.LogInfo($"Mapped a domain model (Type: {editedMovie.GetType().Name}) to a entity (Type: {mappedMovie.GetType().Name})");

            _context.Entry(mappedMovie).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
