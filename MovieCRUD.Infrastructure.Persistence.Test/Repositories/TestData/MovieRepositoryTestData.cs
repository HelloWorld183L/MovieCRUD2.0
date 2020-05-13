using MovieCRUD.Domain.Movies;
using MovieCRUD.SharedKernel;
using NUnit.Framework;
using System.Collections;

namespace MovieCRUD.Infrastructure.Tests.Persistence.Repositories.TestData
{
    public class MovieRepositoryTestData
    {
        public static IEnumerable PaginationFilterParameters
        {
            get
            {
                var paginationFilter1 = new PaginationFilter(1, 5);

                var paginationFilter2 = new PaginationFilter(2, 2);

                yield return new TestCaseData(paginationFilter1);
                yield return new TestCaseData(paginationFilter2);
            }
        }

        public static IEnumerable PaginationFilterAndGenreFilterParameters
        {
            get
            {
                var paginationFilter1 = new PaginationFilter(1, 5);

                var paginationFilter2 = new PaginationFilter(2, 2);

                var genreFilter1 = new GetAllByGenreFilter("Superhero");

                var genreFilter2 = new GetAllByGenreFilter("Action");

                yield return new TestCaseData(paginationFilter1, genreFilter1);
                yield return new TestCaseData(paginationFilter2, genreFilter2);
            }
        }
    }
}
