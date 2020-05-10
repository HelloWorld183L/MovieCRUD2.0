using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Contracts.V1.Requests.Authentication;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Contracts.V1.Responses.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MovieCRUD.Infrastructure.Network.v1.Interfaces
{
    public interface IAuthApiClient
    {
        Task RegisterUserAsync(RegisterRequest registerModel);
        Task RegisterUserExternalAsync(RegisterExternalRequest registerModel);
        Task<IEnumerable<ExternalLoginResponse>> GetExternalLoginsAsync(string returnUrl, bool generateState = false);
        Task<ExternalLoginResponse> GetExternalLoginAsync(string provider, string error = null);
        Task RemoveLoginAsync(RemoveLoginRequest removeLoginModel);
        Task AddExternalLoginAsync(AddExternalLoginRequest addExternalLoginModel);
        Task SetPasswordAsync(SetPasswordRequest setPasswordModel);
        Task ChangePasswordAsync(ChangePasswordRequest changePasswordModel);
        Task<ManageInfoResponse> GetManageInfoAsync(string returnUrl, bool generateState = false);
        Task LogoutAsync();
        Task<UserInfoResponse> GetUserInfoAsync();
        Task<RequestTokenResponse> RequestToken(AccessTokenRequest accessTokenRequest);
    }
}
