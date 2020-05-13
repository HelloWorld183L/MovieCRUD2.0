using MovieCRUD.SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Domain.Movies
{
    public class Movie : IAggregate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public Rating Rating { get; set; }
    }
}