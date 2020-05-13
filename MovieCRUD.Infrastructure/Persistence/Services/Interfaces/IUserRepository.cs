using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MovieCRUD.Domain.Authentication;
using MovieCRUD.Infrastructure.Interfaces;
using MovieCRUD.Infrastructure.Notifications.Services;
using MovieCRUD.Infrastructure.Notifications.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCRUD.Infrastructure.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        ISmsService SmsService { get; }
        IEmailService EmailService { get; }
        Task<IdentityResult> RegisterUserAsync(User user);
        Task<User> GetUserAsync(string userName, string password);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetExternalUserAsync(UserLoginInfo loginInfo);
        Task<IdentityResult> RemovePasswordAsync(string userId);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo loginInfo);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo loginInfo);
        Task<IdentityResult> AddPasswordAsync(string userId, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task<string> GenerateChangePhoneNumberToken(string userId, string phoneNumber);
        Task<IdentityResult> SetTwoFactorEnabled(string userId, bool isEnabled);
        Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token);
        Task<string> GetPhoneNumberAsync(string userId);
        Task<IdentityResult> SetPhoneNumberAsync(string userId, string newNumber);
        Task<bool> GetTwoFactorEnabledAsync(string userId);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        Task SignIn(User user, bool isPersistent, bool rememberBrowser);
    }
}
