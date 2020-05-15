using MovieCRUD.SharedKernel;

namespace MovieCRUD.Desktop.Models.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public Rating Rating { get; set; }
    }
}
