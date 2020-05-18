using AutoMapper;
using MovieCRUD.Domain.Authentication;
using MovieCRUD.Domain.Authentication.ValueObjects;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Infrastructure.Persistence.Services
{
    public class UserRepository : Repository<User, UserEntity>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger) { }

        public async Task<IList<Claim>> GetClaimsAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IList<UserLoginData>> GetLoginsAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetPasswordHashAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUserByExternalLoginInfoAsync(UserLoginData loginInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            var userEntity = await _entitySet.FirstOrDefaultAsync(x => x.UserName == userName);

            var mappedUser = _mapper.Map<User>(userEntity);

            return mappedUser;
        }
    }
}
