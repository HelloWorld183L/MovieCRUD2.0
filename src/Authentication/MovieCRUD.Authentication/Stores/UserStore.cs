using AutoMapper;
using Microsoft.AspNet.Identity;
using MovieCRUD.Authentication.Models.IdentityModels;
using MovieCRUD.Domain.Authentication;
using MovieCRUD.Infrastructure.Persistence;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace MovieCRUD.Authentication.Stores
{
    public class UserStore : IUserLoginStore<UserDTO, int>,
        IUserClaimStore<UserDTO, int>,
        IUserRoleStore<UserDTO, int>,
        IUserPasswordStore<UserDTO, int>,
        IUserSecurityStampStore<UserDTO, int>,
        IUserStore<UserDTO, int>,
        IDisposable
    {
        private IUserRepository _userRepo;
        private IMapper _mapper;

        public UserStore(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public Task AddClaimAsync(UserDTO user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(UserDTO user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(UserDTO user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var mappedUser = _mapper.Map<User>(user);
            _userRepo.Create(mappedUser);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var mappedUser = _mapper.Map<User>(user);
            _userRepo.Delete(mappedUser.Id);

            return Task.CompletedTask;
        }

        public async Task<UserDTO> FindAsync(UserLoginInfo loginInfo)
        {
            if (loginInfo == null)
                throw new ArgumentNullException("loginInfo");

            var user = await _userRepo.GetByExternalLoginInfoAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            var mappedUser = _mapper.Map<UserDTO>(user);

            return mappedUser;
        }

        public Task<UserDTO> FindByIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("userId", userId, "User IDs should NEVER be 0 or below.");

            var user = _userRepo.Get(userId);
            var mappedUser = _mapper.Map<UserDTO>(user);

            return Task.FromResult(mappedUser);
        }

        public async Task<UserDTO> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");

            var user = await _userRepo.GetUserByNameAsync(userName);
            var mappedUser = _mapper.Map<UserDTO>(user);

            return mappedUser;
        }

        public async Task<IList<Claim>> GetClaimsAsync(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var claims = await _userRepo.GetClaimsAsync(user.Id);

            return claims;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var userLogins = await _userRepo.GetLoginsAsync(user.Id);

            return userLogins;
        }

        public Task<string> GetPasswordHashAsync(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(UserDTO user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(UserDTO user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(UserDTO user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(UserDTO user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(UserDTO user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(UserDTO user, string stamp)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void Dispose(){}
    }
}