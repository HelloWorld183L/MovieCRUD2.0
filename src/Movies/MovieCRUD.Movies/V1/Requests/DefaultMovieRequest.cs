using MovieCRUD.Movies;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Movies.Requests
{
    public abstract class DefaultMovieRequest : IMovieRequest
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public Rating Rating { get; set; }
    }
}