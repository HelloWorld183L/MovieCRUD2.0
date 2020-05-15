using MovieCRUD.Desktop.Models.DTOs;
using Stylet;

namespace MovieCRUD.Desktop.ViewModels
{
    public class MovieDetailsViewModel : Screen
    {
        private MovieDTO _movie;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Rating { get; set; }

        public MovieDetailsViewModel(MovieDTO movie)
        {
            _movie = movie;
            Id = _movie.Id;
            Name = _movie.Name;
            Genre = _movie.Genre;
            Rating = _movie.Rating.ToString();
        }
    }
}
