using MovieCRUD.Domain.Authentication;
using MovieCRUD.Domain.Authentication.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByNameAsync(string userName);
        Task<User> GetUserByExternalLoginInfoAsync(UserLoginData loginInfo);
        Task<IList<Claim>> GetClaimsAsync(int userId);
        Task<IList<UserLoginData>> GetLoginsAsync(int userId);
        Task GetPasswordHashAsync(int userId);
    }
}
