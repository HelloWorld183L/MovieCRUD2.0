using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MovieCRUD.Domain.Authentication;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.Persistence.Services
{
    public class UserRepository : Repository<User, UserEntity>, IUserRepository
    {
        private UserManager<UserEntity> _userManager;
        private SignInManager<UserEntity, string> _signInManager;

        public UserRepository(ApplicationDbContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger) 
        {
            _userManager = new UserManager<UserEntity>(new UserStore<UserEntity>(_context));
        }

        public async Task<User> GetUserAsync(string userName, string password)
        {
            var userEntity = await _userManager.FindAsync(userName, password);

            var mappedUser = _mapper.Map<User>(userEntity);

            return mappedUser;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var userEntity = await _userManager.FindByIdAsync(userId);

            var mappedUser = _mapper.Map<User>(userEntity);

            return mappedUser;
        }

        public async Task<IdentityResult> RegisterUserAsync(User user)
        {
            var identityUser = new UserEntity
            {
                UserName = user.Username,
            };

            var createUserResult = await _userManager.CreateAsync(identityUser, user.Password);

            return createUserResult;
        }

        public async Task<IdentityResult> RemovePasswordAsync(string userId)
        {
            var removePasswordResult = await _userManager.RemovePasswordAsync(userId);

            return removePasswordResult;
        }

        public async Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo loginInfo)
        {
            var removeLoginResult = await _userManager.RemoveLoginAsync(userId, loginInfo);

            return removeLoginResult;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword) 
        {
            var changePasswordResult = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);

            return changePasswordResult;
        }

        public async Task<IdentityResult> AddPasswordAsync(string userId, string newPassword)
        {
            var addPasswordResult = await _userManager.AddPasswordAsync(userId, newPassword);

            return addPasswordResult;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo)
        {
            var addLoginResult = await _userManager.AddLoginAsync(userId, loginInfo);

            return addLoginResult;
        }

        public async Task<User> GetExternalUserAsync(UserLoginInfo loginInfo)
        {
            var userEntity = await _userManager.FindAsync(loginInfo);

            var mappedUser = _mapper.Map<User>(userEntity);

            return mappedUser;
        }

        public async Task<string> GenerateChangePhoneNumberToken(string userId, string phoneNumber)
        {
            var changePhoneNumberToken = await _userManager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);

            return changePhoneNumberToken;
        }

        public async Task<IdentityResult> SetTwoFactorEnabled(string userId, bool isEnabled)
        {
            var setTwoFactorResult = await _userManager.SetTwoFactorEnabledAsync(userId, isEnabled);

            return setTwoFactorResult;
        }

        public async Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token)
        {
            var changeNumberResult = await _userManager.ChangePhoneNumberAsync(userId, phoneNumber, token);

            return changeNumberResult;
        }

        public async Task<IdentityResult> SetPhoneNumberAsync(string userId, string newNumber)
        {
            var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(userId, newNumber);

            return setPhoneNumberResult;
        }

        public async Task<string> GetPhoneNumberAsync(string userId)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(userId);

            return phoneNumber;
        }

        public async Task<bool> GetTwoFactorEnabledAsync(string userId)
        {
            var isEnabled = await _userManager.GetTwoFactorEnabledAsync(userId);

            return isEnabled;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        {
            var logins = await _userManager.GetLoginsAsync(userId);

            return logins;
        }

        public async Task SignIn(User user, bool isPersistent, bool rememberBrowser)
        {
            var userEntity = _mapper.Map<UserEntity>(user);

            await _signInManager.SignInAsync(userEntity, isPersistent, rememberBrowser);
        }
    }
}
