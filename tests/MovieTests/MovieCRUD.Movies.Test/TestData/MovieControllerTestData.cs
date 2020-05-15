using MovieCRUD.Domain;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Requests;
using MovieCRUD.SharedKernel;
using NUnit.Framework;
using System.Collections;

namespace MovieCRUD.Movies.Tests.TestData
{
    public class MovieControllerTestData
    {
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
                    Rating = Rating.Masterpiece
                };

                yield return new TestCaseData(createMovieRequest1);
                yield return new TestCaseData(createMovieRequest2);
            }
        }

        public static IEnumerable GetMovieParameterTestData
        {
            get
            {
                var movie1 = new Movie()
                {
                    Id = 1,
                    Name = "Legend of Zelda",
                    Genre = "Adventure",
                    Rating = Rating.Good
                };

                var movie2 = new Movie()
                {
                    Id = 2,
                    Name = "Shawn the Sheep",
                    Genre = "Adventure",
                    Rating = Rating.Terrible
                };

                yield return new TestCaseData(movie1);
                yield return new TestCaseData(movie2);
            }
        }
    }
}
