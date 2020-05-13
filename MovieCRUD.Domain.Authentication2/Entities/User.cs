using MovieCRUD.SharedKernel;

namespace MovieCRUD.Domain.Authentication
{
    public class User : IAggregate
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
    }
}
