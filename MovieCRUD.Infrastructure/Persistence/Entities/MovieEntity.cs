using MovieCRUD.Infrastructure.Enums;
using MovieCRUD.Domain.Interfaces;

namespace MovieCRUD.Infrastructure.Entities
{
    public class MovieEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public Rating Rating { get; set; }
    }
}