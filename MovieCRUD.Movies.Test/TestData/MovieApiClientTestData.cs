using MovieCRUD.Movies.Requests;
using MovieCRUD.Movies.Responses;
using MovieCRUD.SharedKernel;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace MovieCRUD.Infrastructure.Tests.Network.v1.TestData
{
    public class MovieApiClientTestData
    {
        public static IEnumerable CreateMovieParameterTestData
        {
            get
            {
                var createMovieRequest1 = new CreateMovieRequest()
                {
                    Name = "The Shawshank Redemption",
                    Genre = "Drama/Prison",
                    Rating = Rating.Masterpiece
                };

                var createMovieRequest2 = new CreateMovieRequest()
                {
                    Name = "JoJo's Bizarre Adventure: Stardust Crusaders",
                    Genre = "Shonen",
                    Rating = Rating.Terrible
                };

                yield return new TestCaseData(createMovieRequest1);
                yield return new TestCaseData(createMovieRequest2);
            }
        }

        public static IEnumerable EditMovieParameterTestData
        {
            get
            {
                var editMovieRequest1 = new EditMovieRequest()
                {
                    Name = "Star Trek",
                    Genre = "Sci-fi",
                    Rating = Rating.Good
                };

                var editMovieRequest2 = new EditMovieRequest()
                {
                    Name = "Kung fu Panda",
                    Genre = "Action",
                    Rating = Rating.Bad
                };

                yield return new TestCaseData(editMovieRequest1);
                yield return new TestCaseData(editMovieRequest2);
            }
        }

        public static IEnumerable GetAllMoviesParameters
        {
            get
            {
                var movies = new List<MovieResponse>()
                {
                    new MovieResponse()
                    {
                        Id = 1,
                        Name = "Avengers",
                        Genre = "Superhero",
                        Rating = Rating.Good
                    },
                    new MovieResponse()
                    {
                        Id = 2,
                        Name = "Ace Ventura",
                        Genre = "Comedy",
                        Rating = Rating.Masterpiece
                    },
                    new MovieResponse()
                    {
                        Id = 3,
                        Name = "Spy Kids 1",
                        Genre = "Action",
                        Rating = Rating.Mediocore
                    }
                };
                yield return new TestCaseData(movies);
            }
        }

        public static IEnumerable GetMovieParameterTestData
        {
            get
            {
                var movie1 = new MovieResponse()
                {
                    Id = 1,
                    Name = "Avengers",
                    Genre = "Superhero",
                    Rating = Rating.Good
                };

                var movie2 = new MovieResponse()
                {
                    Id = 2,
                    Name = "Ace Ventura",
                    Genre = "Comedy",
                    Rating = Rating.Masterpiece
                };

                yield return new TestCaseData(movie1);
                yield return new TestCaseData(movie2);
            }
        }
    }
}