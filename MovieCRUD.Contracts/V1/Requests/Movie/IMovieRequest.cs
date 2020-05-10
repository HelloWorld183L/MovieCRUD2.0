using MovieCRUD.Api;

namespace MovieCRUD.Contracts.V1.Requests
{
    public interface IMovieRequest
    {
        string Name { get; set; }
        string Genre { get; set; }
        Rating Rating { get; set; }
    }
}
