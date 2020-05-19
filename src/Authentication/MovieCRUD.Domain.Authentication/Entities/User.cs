using MovieCRUD.SharedKernel;
using System.Collections.Generic;

namespace MovieCRUD.Domain.Authentication
{
    public class User : IAggregate
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<ExternalLogin> Logins { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
