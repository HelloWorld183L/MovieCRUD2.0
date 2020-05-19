using Microsoft.AspNet.Identity;
using MovieCRUD.Domain.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieCRUD.Authentication.Models.IdentityModels
{
    public class UserDTO : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<ExternalLogin> Logins { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserDTO, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}