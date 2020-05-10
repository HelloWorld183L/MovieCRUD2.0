using MovieCRUD.Api;

namespace MovieCRUD.Contracts.V1.Requests
{
    public abstract class DefaultMovieRequest : IMovieRequest
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public Rating Rating { get; set; }
    }
}