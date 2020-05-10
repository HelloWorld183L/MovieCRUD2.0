using MovieCRUD.Api;

namespace MovieCRUD.Contracts.V1.Responses
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public Rating Rating { get; set; }
    }
}
