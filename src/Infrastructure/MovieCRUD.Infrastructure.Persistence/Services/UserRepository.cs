using AutoMapper;
using MovieCRUD.Domain.Authentication;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.Persistence.Services
{
    public class UserRepository : Repository<User, UserEntity>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger) { }

        public Task<IList<Claim>> GetClaimsAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<ExternalLogin>> GetLoginsAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task GetPasswordHashAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetUserByExternalLoginInfoAsync(ExternalLogin loginInfo)
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
