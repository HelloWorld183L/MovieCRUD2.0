using Microsoft.AspNet.Identity.EntityFramework;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Infrastructure.Persistence
{
    public class UserEntity : IdentityUser, IEntity
    {
        public int Id { get; set; }
    }
}