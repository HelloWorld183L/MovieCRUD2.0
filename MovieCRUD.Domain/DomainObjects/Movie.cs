using MovieCRUD.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Domain
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