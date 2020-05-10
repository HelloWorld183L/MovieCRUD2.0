using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.DTOs;
using MovieCRUD.Enums;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MovieCRUD.Tests.TestData
{
    public class MovieControllerTestData
    {
        public static IEnumerable MovieResponseParameters
        {
            get
            {
                var movieResponse1 = new MovieResponse()
                {
                    Id = 1,
                    Name = "Harry Potter: The Philosopher's Stone",
                    Genre = "Mystery",
                    Rating = Api.Rating.Good
                };

                var movieResponse2 = new MovieResponse()
                {
                    Id = 2,
                    Name = "Harry Potter: Deathly Hallows",
                    Genre = "Mystery/Action",
                    Rating = Api.Rating.Masterpiece
                };

                yield return new TestCaseData(movieResponse1);
                yield return new TestCaseData(movieResponse2);
            }
        }

        public static IEnumerable MovieResponseParameters2
        {
            get
            {
                var movieResponses = new List<MovieResponse>();

                var movieResponse1 = new MovieResponse()
                {
                    Id = 1,
                    Name = "Harry Potter: The Philosopher's Stone",
                    Genre = "Mystery",
                    Rating = Api.Rating.Good
                };

                var movieResponse2 = new MovieResponse()
                {
                    Id = 2,
                    Name = "Harry Potter: Deathly Hallows",
                    Genre = "Mystery/Action",
                    Rating = Api.Rating.Masterpiece
                };

                movieResponses.Add(movieResponse1);
                movieResponses.Add(movieResponse2);

                yield return new TestCaseData(movieResponses.AsEnumerable());
            }
        }

        public static IEnumerable<MovieDTO> MovieDTOParameters1
        {
            get
            {
                var movieDto1 = new MovieDTO()
                {
                    Id = 1,
                    Name = "Harry Potter: The Philosopher's Stone",
                    Genre = "Mystery",
                    Rating = Rating.Good
                };

                var movieDto2 = new MovieDTO()
                {
                    Id = 2,
                    Name = "Harry Potter: Deathly Hallows",
                    Genre = "Mystery/Action",
                    Rating = Rating.Good
                };

                yield return movieDto1;
                yield return movieDto2;
            }
        }

        public static IEnumerable MovieDTOParameters2
        {
            get
            {
                var movieDto1 = new MovieDTO()
                {
                    Id = 1,
                    Name = "Harry Potter: The Philosopher's Stone",
                    Genre = "Mystery",
                    Rating = Rating.Good
                };

                var movieDto2 = new MovieDTO()
                {
                    Id = 2,
                    Name = "Harry Potter: Deathly Hallows",
                    Genre = "Mystery/Action",
                    Rating = Rating.Good
                };

                yield return new TestCaseData(movieDto1);
                yield return new TestCaseData(movieDto2);
            }
        }
    }
}
