using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MovieCRUD.Authentication.Notifications.Messages;
using MovieCRUD.Authentication.Requests;
using MovieCRUD.Authentication.Responses;
using MovieCRUD.Authentication.V1.Requests;
using MovieCRUD.Domain.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCRUD.Authentication.Clients
{
    public interface IAuthApiClient
    {
        Task<IdentityResult> RegisterUserAsync(RegisterRequest registerModel);
        Task<IdentityResult> RegisterUserExternalAsync(RegisterExternalRequest registerModel);
        Task<IEnumerable<ExternalLoginResponse>> GetExternalLoginsAsync(string returnUrl, bool generateState = false);
        Task<ExternalLoginResponse> GetExternalLoginAsync(string provider, string error = null);
        Task<IdentityResult> RemoveLoginAsync(RemoveLoginRequest removeLoginModel);
        Task<IdentityResult> AddExternalLoginAsync(AddExternalLoginRequest addExternalLoginModel);
        Task<IdentityResult> SetPasswordAsync(SetPasswordRequest setPasswordModel);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordRequest changePasswordModel);
        Task<ManageInfoResponse> GetManageInfoAsync(string returnUrl, bool generateState = false);
        Task<IdentityResult> LogoutAsync();
        Task<UserInfoResponse> GetUserInfoAsync();
        Task<RequestTokenResponse> RequestToken(AccessTokenRequest accessTokenRequest);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        Task<bool> GetTwoFactorEnabledAsync(string userId);
        Task<User> GetUserByIdAsync(string userId);
        Task<string> GetPhoneNumberAsync(string userId);
        Task<IdentityResult> SignInAsync(SignInRequest request);
        Task<IdentityResult> AddLoginAsync(AddLoginRequest request);
        Task<IdentityResult> GenerateChangePhoneNumberTokenAsync(GenerateChangePhoneNumberTokenRequest request);
        Task<IdentityResult> SetTwoFactorEnabledAsync(SetTwoFactorEnabledRequest request);
        Task<IdentityResult> AddPasswordAsync(AddPasswordRequest request);
        Task<IdentityResult> SetPhoneNumberAsync(SetPhoneNumberRequest request);
        Task<IdentityResult> ChangePhoneNumberAsync(ChangePhoneNumberRequest request);
        Task<SignInStatus> TwoFactorSignInAsync(TwoFactorSignInRequest signInRequest);
        Task<IdentityResult> ConfirmEmail(ConfirmEmailRequest request);
        Task<IdentityResult> SendEmailAsync(string userId, EmailMessage message);
        Task<User> FindByNameAsync(string email);
        Task<IdentityResult> GeneratePasswordResetTokenAsync(string userId);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInStatus> ExternalSignInAsync(ExternalSignInRequest request);
        Task<IdentityResult> SendTwoFactorAsync(string selectedProvider);
        Task<string> GetVerifiedUserIdAsync();
        Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId);
    }
}
