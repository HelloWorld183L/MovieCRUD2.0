using MovieCRUD.Domain.Interfaces;

namespace MovieCRUD.Domain.DomainObjects
{
    public class User : IAggregate
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
