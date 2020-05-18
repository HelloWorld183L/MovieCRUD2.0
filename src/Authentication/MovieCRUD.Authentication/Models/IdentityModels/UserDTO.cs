using Microsoft.AspNet.Identity;
using MovieCRUD.Domain.Authentication.ValueObjects;
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
        public ICollection<UserLoginData> Logins { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserDTO, int> manager, string authenticationType)
        {
            var claimsIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return claimsIdentity;
        }
    }
}