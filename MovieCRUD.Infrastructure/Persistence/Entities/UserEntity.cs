using Microsoft.AspNet.Identity.EntityFramework;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Infrastructure
{
    public class UserEntity : IdentityUser, IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}