using MovieCRUD.SharedKernel;

namespace MovieCRUD.Infrastructure.Tests.Network.v1.Tests
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public Rating Rating { get; set; }
    }
}
