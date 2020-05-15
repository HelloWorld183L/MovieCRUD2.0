using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MovieCRUD.Authentication.Notifications.Messages;
using MovieCRUD.Authentication.Requests;
using MovieCRUD.Authentication.Responses;
using MovieCRUD.Authentication.V1;
using MovieCRUD.Authentication.V1.Requests;
using MovieCRUD.Authentication.V1.Responses;
using MovieCRUD.Domain.Authentication;
using MovieCRUD.Infrastructure.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MovieCRUD.Authentication.Clients
{
    public class AuthApiClient : IAuthApiClient
    {
        private static HttpClient _restClient;
        private readonly ILogger _logger;
        private const int _portNumber = 80;

        public AuthApiClient(ILogger logger)
        {
            _restClient = new HttpClient();
            _restClient.BaseAddress = new Uri($"http://localhost:{_portNumber}/");
            _restClient.DefaultRequestHeaders.Accept.Clear();
            _restClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _logger = logger;
        }

        public async Task<IdentityResult> AddExternalLoginAsync(AddExternalLoginRequest addExternalLoginRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(addExternalLoginRequest);
            _logger.LogInfo("Serialized external login model");

            var externalLoginData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.AddExternalLogin, externalLoginData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo("Sent HTTP POST request to add an external login");

            return await GetResult(response);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(changePasswordRequest);
            _logger.LogInfo("Serialized change password model");

            var passwordData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.ChangePassword, passwordData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo("Sent HTTP POST request to change the password");

            return await GetResult(response);
        }

        public async Task<ExternalLoginResponse> GetExternalLoginAsync(string provider, string error = null)
        {
            var response = await _restClient.GetAsync(AccountRoutes.GetExternalLogin);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {AccountRoutes.GetExternalLogin}");

            var externalLogin = await response.Content.ReadAsAsync<ExternalLoginResponse>();

            return externalLogin;
        }

        public async Task<IEnumerable<ExternalLoginResponse>> GetExternalLoginsAsync(string returnUrl, bool generateState = false)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GetExternalLogins}?returnUrl={returnUrl}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {AccountRoutes.GetExternalLogins}");

            var externalLogins = await response.Content.ReadAsAsync<IEnumerable<ExternalLoginResponse>>();

            return externalLogins;
        }

        public async Task<ManageInfoResponse> GetManageInfoAsync(string returnUrl, bool generateState = false)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GetManageInfo}?returnUrl={returnUrl}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {AccountRoutes.GetManageInfo}");

            var manageInfo = await response.Content.ReadAsAsync<ManageInfoResponse>();

            return manageInfo;
        }

        public async Task<UserInfoResponse> GetUserInfoAsync()
        {
            var response = await _restClient.GetAsync(AccountRoutes.GetUserInfo);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {AccountRoutes.GetUserInfo}");

            var userInfo = await response.Content.ReadAsAsync<UserInfoResponse>();

            return userInfo;
        }

        public async Task<IdentityResult> LogoutAsync()
        {
            var response = await _restClient.GetAsync(AccountRoutes.Logout);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP GET request to {AccountRoutes.Logout}");

            return await GetResult(response);
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterRequest registerRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(registerRequest);

            var registerData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.RegisterUser, registerData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> RegisterUserExternalAsync(RegisterExternalRequest registerRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(registerRequest);

            var registerExternalData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.RegisterUserExternal, registerExternalData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> RemoveLoginAsync(RemoveLoginRequest removeLoginRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(removeLoginRequest);

            var removeLoginData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.RemoveLogin, removeLoginData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> SetPasswordAsync(SetPasswordRequest setPasswordRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(setPasswordRequest);

            var setPasswordData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.SetPassword, setPasswordData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<RequestTokenResponse> RequestToken(AccessTokenRequest accessTokenRequest)
        {
            var requestBody = $"grant_type=password&username={accessTokenRequest.Username}&password={accessTokenRequest.Password}";

            var requestContent = new StringContent(requestBody, Encoding.UTF8, "x-www-form-urlencoded");

            var response = await _restClient.PostAsync(AccountRoutes.RequestToken, requestContent);
            response.EnsureSuccessStatusCode();

            var responseText = await response.Content.ReadAsStringAsync();

            var requestTokenResponse = JsonConvert.DeserializeObject<RequestTokenResponse>(responseText);

            return requestTokenResponse;
        }
             
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GetLogins}/{userId}");
            response.EnsureSuccessStatusCode();

            var userLogins = await response.Content.ReadAsAsync<IList<UserLoginInfo>>();

            return userLogins;
        }

        public async Task<bool> GetTwoFactorEnabledAsync(string userId)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GetTwoFactorEnabled}/{userId}");
            response.EnsureSuccessStatusCode();

            var isEnabled = await response.Content.ReadAsAsync<bool>();

            return isEnabled;

        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GetUserById}/{userId}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadAsAsync<User>();

            return user;
        }

        public async Task<string> GetPhoneNumberAsync(string userId)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GetPhoneNumber}/{userId}");
            response.EnsureSuccessStatusCode();

            var phoneNumber = await response.Content.ReadAsStringAsync();

            return phoneNumber;
        }

        public async Task<IdentityResult> SignInAsync(SignInRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.SignIn, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> AddLoginAsync(AddLoginRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.AddLogin, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> GenerateChangePhoneNumberTokenAsync(GenerateChangePhoneNumberTokenRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.GenerateChangePhoneNumberToken, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> SetTwoFactorEnabledAsync(SetTwoFactorEnabledRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.SetTwoFactorEnabled, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> AddPasswordAsync(AddPasswordRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.AddPassword, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> SetPhoneNumberAsync(SetPhoneNumberRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.SetPhoneNumber, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> ChangePhoneNumberAsync(ChangePhoneNumberRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.ChangePhoneNumber, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        private async Task<IdentityResult> GetResult(HttpResponseMessage response)
        {
            var identityResult = await response.Content.ReadAsAsync<IdentityResult>();

            return identityResult;
        }

        public async Task<SignInStatus> TwoFactorSignInAsync(TwoFactorSignInRequest signInRequest)
        {
            var serializedRequest = JsonConvert.SerializeObject(signInRequest);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.TwoFactorSignIn, requestData);
            response.EnsureSuccessStatusCode();

            var signInStatus = await response.Content.ReadAsAsync<SignInStatus>();

            return signInStatus;
        }

        public async Task<IdentityResult> ConfirmEmail(ConfirmEmailRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.ConfirmEmail, requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<IdentityResult> SendEmailAsync(string userId, EmailMessage message)
        {
            var serializedRequest = JsonConvert.SerializeObject(message);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync($"{AccountRoutes.SendEmail}/{userId}", requestData);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<User> FindByNameAsync(string email)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.FindByName}/{email}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadAsAsync<User>();

            return user;
        }

        public async Task<IdentityResult> GeneratePasswordResetTokenAsync(string userId)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GeneratePasswordResetToken}/{userId}");
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.IsEmailConfirmed}/{userId}");
            response.EnsureSuccessStatusCode();

            var isEmailConfirmed = await response.Content.ReadAsAsync<bool>();

            return isEmailConfirmed;
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            var response = await _restClient.GetAsync(AccountRoutes.GetExternalLoginInfo);
            response.EnsureSuccessStatusCode();

            var externalLoginInfo = await response.Content.ReadAsAsync<ExternalLoginInfo>();

            return externalLoginInfo;
        }

        public async Task<SignInStatus> ExternalSignInAsync(ExternalSignInRequest request)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);

            var requestData = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.ExternalSignIn, requestData);
            response.EnsureSuccessStatusCode();

            var signInStatus = await response.Content.ReadAsAsync<SignInStatus>();

            return signInStatus;
        }

        public async Task<IdentityResult> SendTwoFactorAsync(string selectedProvider)
        {
            var content = new StringContent(selectedProvider, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(AccountRoutes.SendTwoFactor, content);
            response.EnsureSuccessStatusCode();

            return await GetResult(response);
        }

        public async Task<string> GetVerifiedUserIdAsync()
        {
            var response = await _restClient.GetAsync(AccountRoutes.GetVerifiedUserId);
            response.EnsureSuccessStatusCode();

            var userId = await response.Content.ReadAsStringAsync();

            return userId;
        }

        public async Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId)
        {
            var response = await _restClient.GetAsync($"{AccountRoutes.GetValidTwoFactorProviderAsync}/{userId}");
            response.EnsureSuccessStatusCode();

            var twoFactorProviders = await response.Content.ReadAsAsync<IList<string>>();

            return twoFactorProviders;
        }
    }
}
