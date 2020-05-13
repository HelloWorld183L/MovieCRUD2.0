using MovieCRUD.SharedKernel;
using System.Collections.Generic;

namespace MovieCRUD.Infrastructure.Persistence.Interfaces
{
    public interface IRepository<IAggregate>
    {
        IEnumerable<IAggregate> GetAll(PaginationFilter paginationFilter);
        IAggregate Get(int id);
        void Create(IAggregate aggregate);
        void Edit(IAggregate aggregate);
        void Delete(int id);
    }
}
