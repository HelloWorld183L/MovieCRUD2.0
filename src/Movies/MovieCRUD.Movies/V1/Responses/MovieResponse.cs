using MovieCRUD.SharedKernel;

namespace MovieCRUD.Movies.Responses
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public Rating Rating { get; set; }
    }
}
