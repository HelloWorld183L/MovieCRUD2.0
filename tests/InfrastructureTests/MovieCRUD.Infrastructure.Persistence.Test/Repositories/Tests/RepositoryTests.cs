using AutoMapper;
using MovieCRUD.SharedKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MovieCRUD.Infrastructure.Tests.Persistence.Repositories
{
    public class RepositoryTests<TDomainModel, TEntity>
        where TDomainModel : IAggregate
        where TEntity : class, IEntity
    {
        protected IEnumerable<TEntity> _entitySet;
        protected ApplicationDbContext _context;
        protected IMapper _mapper;
        protected string _entityTypeName;

        [SetUp]
        public void SetUp()
        {
            _entityTypeName = typeof(TEntity).Name;

            _entitySet = GetEntities();

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddMaps(Assembly.GetExecutingAssembly());
            });
            _mapper = new Mapper(mapperConfiguration);
        }

        [Test]
        public void GetAll_ReturnsEntities(PaginationFilter filter)
        {

        }

        [Test]
        public void GetAll_PaginationFilterAppliedSuccess(PaginationFilter filter)
        {

        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void Get_ReturnsEntity(int id)
        {

        }

        [Test]
        public void Create_EntityIsCreated(TDomainModel model)
        {

        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void Delete_EntityIsDeleted(int id)
        {

        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void Delete_InvalidIdDoesntDeleteEntity(int id)
        {

        }

        [Test]
        public void Edit_EntityIsEdited(TDomainModel newModel)
        {

        }

        [Test]
        public void DomainModelToEntityMappingIsSuccess(TDomainModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
        }

        [Test]
        public void EntityToDomainModelMappingIsSuccess(TEntity entity)
        {
            var domainModel = _mapper.Map<TDomainModel>(entity);
        }

        private IEnumerable<TEntity> GetEntities()
        {
            return null;
        }
    }
}
