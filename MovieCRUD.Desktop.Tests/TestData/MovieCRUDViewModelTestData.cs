using MovieCRUD.Contracts.V1.Responses;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MovieCRUD.Desktop.Tests.TestData
{
    public class MovieCRUDViewModelTestData
    {
        public static IEnumerable MovieResponseParameters
        {
            get
            {
                var movieResponses = new List<MovieResponse>
                {
                    new MovieResponse()
                    {
                        Id = 1,
                        Name = "Harry Potter: The Philosopher's Stone",
                        Genre = "Mystery",
                        Rating = Api.Rating.Good
                    },

                    new MovieResponse()
                    {
                        Id = 2,
                        Name = "Harry Potter: Deathly Hallows",
                        Genre = "Mystery/Action",
                        Rating = Api.Rating.Good
                    }
                };
                yield return new TestCaseData(movieResponses.AsEnumerable());
            }
        }
    }
}
