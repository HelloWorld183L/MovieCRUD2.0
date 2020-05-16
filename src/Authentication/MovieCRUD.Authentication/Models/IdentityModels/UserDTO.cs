using Microsoft.AspNet.Identity;
using MovieCRUD.Domain.Authentication.ValueObjects;
using System.Collections.Generic;

namespace MovieCRUD.Authentication.Models.IdentityModels
{
    public class UserDTO : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<UserLoginData> Logins { get; set; }
    }
}