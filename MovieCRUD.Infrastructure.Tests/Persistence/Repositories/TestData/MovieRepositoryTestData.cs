using MovieCRUD.Domain.Filters;
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
                var paginationFilter1 = new PaginationFilter();
                paginationFilter1.PageNumber = 1;
                paginationFilter1.PageSize = 5;

                var paginationFilter2 = new PaginationFilter();
                paginationFilter2.PageNumber = 2;
                paginationFilter2.PageSize = 2;

                yield return new TestCaseData(paginationFilter1);
                yield return new TestCaseData(paginationFilter2);
            }
        }

        public static IEnumerable PaginationFilterAndGenreFilterParameters
        {
            get
            {
                var paginationFilter1 = new PaginationFilter
                {
                    PageNumber = 1,
                    PageSize = 5
                };

                var paginationFilter2 = new PaginationFilter
                {
                    PageNumber = 2,
                    PageSize = 2
                };

                var genreFilter1 = new GetAllByGenreFilter
                {
                    Genre = "Superhero"
                };

                var genreFilter2 = new GetAllByGenreFilter
                {
                    Genre = "Action"
                };

                yield return new TestCaseData(paginationFilter1, genreFilter1);
                yield return new TestCaseData(paginationFilter2, genreFilter2);
            }
        }
    }
}
