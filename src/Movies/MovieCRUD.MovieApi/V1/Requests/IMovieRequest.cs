using MovieCRUD.Movies;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Movies.Requests
{
    public interface IMovieRequest
    {
        string Name { get; set; }
        string Genre { get; set; }
        Rating Rating { get; set; }
    }
}
