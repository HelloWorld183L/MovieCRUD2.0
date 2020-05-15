using MovieCRUD.Domain.Movies;
using MovieCRUD.SharedKernel;
using System.Collections.Generic;

namespace MovieCRUD.Infrastructure.Persistence.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        IEnumerable<Movie> GetAll(PaginationFilter paginationFilter, GetAllByGenreFilter filter = null);
    }
}