using MovieCRUD.Contracts.V1.ApiRoutes;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Contracts.V1.Requests.Authentication;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Contracts.V1.Responses.Authentication;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Network.v1.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MovieCRUD.Infrastructure.Network.v1
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

        public async Task AddExternalLoginAsync(AddExternalLoginRequest addExternalLoginRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(addExternalLoginRequest);
            _logger.LogInfo("Serialized external login model");

            var externalLoginData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(ApiRoutes.AccountRoutes.AddExternalLogin, externalLoginData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo("Sent HTTP POST request to add an external login");
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(changePasswordRequest);
            _logger.LogInfo("Serialized change password model");

            var passwordData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(ApiRoutes.AccountRoutes.ChangePassword, passwordData);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo("Sent HTTP POST request to change the password");
        }

        public async Task<ExternalLoginResponse> GetExternalLoginAsync(string provider, string error = null)
        {
            var response = await _restClient.GetAsync(ApiRoutes.AccountRoutes.GetExternalLogin);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {ApiRoutes.AccountRoutes.GetExternalLogin}");

            var externalLogin = await response.Content.ReadAsAsync<ExternalLoginResponse>();

            return externalLogin;
        }

        public async Task<IEnumerable<ExternalLoginResponse>> GetExternalLoginsAsync(string returnUrl, bool generateState = false)
        {
            var response = await _restClient.GetAsync($"{ApiRoutes.AccountRoutes.GetExternalLogins}?returnUrl={returnUrl}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {ApiRoutes.AccountRoutes.GetExternalLogins}");

            var externalLogins = await response.Content.ReadAsAsync<IEnumerable<ExternalLoginResponse>>();

            return externalLogins;
        }

        public async Task<ManageInfoResponse> GetManageInfoAsync(string returnUrl, bool generateState = false)
        {
            var response = await _restClient.GetAsync($"{ApiRoutes.AccountRoutes.GetManageInfo}?returnUrl={returnUrl}");
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {ApiRoutes.AccountRoutes.GetManageInfo}");

            var manageInfo = await response.Content.ReadAsAsync<ManageInfoResponse>();

            return manageInfo;
        }

        public async Task<UserInfoResponse> GetUserInfoAsync()
        {
            var response = await _restClient.GetAsync(ApiRoutes.AccountRoutes.GetUserInfo);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sent HTTP GET request to {ApiRoutes.AccountRoutes.GetUserInfo}");

            var userInfo = await response.Content.ReadAsAsync<UserInfoResponse>();

            return userInfo;
        }

        public async Task LogoutAsync()
        {
            var response = await _restClient.GetAsync(ApiRoutes.AccountRoutes.Logout);
            response.EnsureSuccessStatusCode();
            _logger.LogInfo($"Sending HTTP GET request to {ApiRoutes.AccountRoutes.Logout}");
        }

        public async Task RegisterUserAsync(RegisterRequest registerRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(registerRequest);

            var registerData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(ApiRoutes.AccountRoutes.RegisterUser, registerData);
            response.EnsureSuccessStatusCode();
        }

        public async Task RegisterUserExternalAsync(RegisterExternalRequest registerRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(registerRequest);

            var registerExternalData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(ApiRoutes.AccountRoutes.RegisterUserExternal, registerExternalData);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveLoginAsync(RemoveLoginRequest removeLoginRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(removeLoginRequest);

            var removeLoginData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(ApiRoutes.AccountRoutes.RemoveLogin, removeLoginData);
            response.EnsureSuccessStatusCode();
        }

        public async Task SetPasswordAsync(SetPasswordRequest setPasswordRequest)
        {
            var serializedModel = JsonConvert.SerializeObject(setPasswordRequest);

            var setPasswordData = new StringContent(serializedModel, Encoding.UTF8, "application/json");

            var response = await _restClient.PostAsync(ApiRoutes.AccountRoutes.SetPassword, setPasswordData);
            response.EnsureSuccessStatusCode();
        }

        public async Task<RequestTokenResponse> RequestToken(AccessTokenRequest accessTokenRequest)
        {
            var requestBody = $"grant_type=password&username={accessTokenRequest.Username}&password={accessTokenRequest.Password}";

            var requestContent = new StringContent(requestBody, Encoding.UTF8, "x-www-form-urlencoded");

            var response = await _restClient.PostAsync(ApiRoutes.AccountRoutes.RequestToken, requestContent);
            response.EnsureSuccessStatusCode();

            var responseText = await response.Content.ReadAsStringAsync();

            var requestTokenResponse = JsonConvert.DeserializeObject<RequestTokenResponse>(responseText);

            return requestTokenResponse;
        }
    }
}
