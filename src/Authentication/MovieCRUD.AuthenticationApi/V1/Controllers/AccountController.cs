using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using MovieCRUD.Authentication.Models;
using MovieCRUD.Authentication.Models.IdentityModels;
using MovieCRUD.Authentication.Requests;
using MovieCRUD.Authentication.Responses;
using MovieCRUD.Authentication.Results;
using MovieCRUD.Authentication.V1;
using MovieCRUD.Authentication.ViewModels;

namespace MovieCRUD.Authentication.Controllers
{
    [Authorize]
    [RoutePrefix(AccountRoutes.Prefix)]
    public class AccountController : ApiController
    {
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }
        private const string LocalLoginProvider = "Local";
        private UserManager<UserDTO, int> _userManager;
        private IMapper _mapper;

        public AccountController(UserManager<UserDTO, int> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route(AccountRoutes.GetUserInfo)]
        public UserInfoResponse GetUserInfo()
        {
            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoResponse
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        [Route(AccountRoutes.Logout)]
        public IHttpActionResult Logout()
        {
            AuthenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        [Route(AccountRoutes.GetManageInfo)]
        public async Task<ManageInfoResponse> GetManageInfo(string returnUrl, bool generateState = false)
        {
            var user = await _userManager.FindByIdAsync(GetUserId());
            if (user == null) return null;

            var mappedUser = _mapper.Map<UserDTO>(user);

            var logins = new List<UserLoginInfoViewModel>();
            foreach (var linkedAccount in mappedUser.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (mappedUser.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoResponse
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = mappedUser.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        [Route(AccountRoutes.ChangePassword)]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordRequest changePasswordModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var changePasswordResult = await _userManager.ChangePasswordAsync(
                GetUserId(),
                changePasswordModel.OldPassword,
                changePasswordModel.NewPassword);
            if (!changePasswordResult.Succeeded) return GetErrorResult(changePasswordResult);

            return Ok();
        }

        [Route(AccountRoutes.SetPassword)]
        public async Task<IHttpActionResult> SetPassword(SetPasswordRequest passwordRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var setPasswordResult = await _userManager.AddPasswordAsync(GetUserId(), passwordRequest.NewPassword);
            if (!setPasswordResult.Succeeded) return GetErrorResult(setPasswordResult);

            return Ok();
        }

        [Route(AccountRoutes.AddExternalLogin)]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginRequest externalLoginRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var authTicket = AccessTokenFormat.Unprotect(externalLoginRequest.ExternalAccessToken);
            if (authTicket == null || authTicket.Identity == null || (authTicket.Properties != null
                && authTicket.Properties.ExpiresUtc.HasValue
                && authTicket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            var externalData = ExternalLoginData.FromIdentity(authTicket.Identity);
            if (externalData == null) return BadRequest("The external login is already associated with an account.");

            var addLoginResult = await _userManager.AddLoginAsync(GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));
            if (!addLoginResult.Succeeded) return GetErrorResult(addLoginResult);

            return Ok();
        }

        [Route(AccountRoutes.RemoveLogin)]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginRequest removeLoginRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            IdentityResult loginProviderResult;
            if (removeLoginRequest.LoginProvider == LocalLoginProvider)
            {
                loginProviderResult = await _userManager.RemovePasswordAsync(GetUserId());
            }
            else
            {
                loginProviderResult = await _userManager.RemoveLoginAsync(GetUserId(),
                    new UserLoginInfo(removeLoginRequest.LoginProvider, removeLoginRequest.ProviderKey));
            }

            if (!loginProviderResult.Succeeded) return GetErrorResult(loginProviderResult);

            return Ok();
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route(AccountRoutes.GetExternalLogin, Name = AccountRoutes.GetExternalLogin)]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null) return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            if (!User.Identity.IsAuthenticated) return new ChallengeResult(provider, this);

            var externalLoginData = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            if (externalLoginData == null) return InternalServerError();

            if (externalLoginData.LoginProvider != provider)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            if (externalLogin == null) return InternalServerError();

            var user = await _userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));
            var hasRegistered = user != null;

            var redirectUri = string.Empty;
            redirectUri = $"{redirectUri}#external_access_token={externalLogin.ExternalAccessToken}&" +
                              $"provider={externalLogin.LoginProvider}&" +
                              $"haslocalaccount={hasRegistered}&" +
                              $"external_user_name={externalLogin.UserName}";

            return Redirect(redirectUri);
        }

        [AllowAnonymous]
        [Route(AccountRoutes.GetExternalLogins)]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            var authDescriptions = AuthenticationManager.GetExternalAuthenticationTypes();
            var externalLoginViewModels = new List<ExternalLoginViewModel>();

            string state = null;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }

            foreach (var authDescription in authDescriptions)
            {
                var externalLoginViewModel = new ExternalLoginViewModel
                {
                    Name = authDescription.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = authDescription.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                externalLoginViewModels.Add(externalLoginViewModel);
            }

            return externalLoginViewModels;
        }

        [AllowAnonymous]
        [Route(AccountRoutes.RegisterUser)]
        public async Task<IHttpActionResult> RegisterUser(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = _mapper.Map<UserDTO>(registerRequest);
            var registerUserResult = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!registerUserResult.Succeeded) return GetErrorResult(registerUserResult);

            return Ok();
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route(AccountRoutes.RegisterUserExternal)]
        public async Task<IHttpActionResult> RegisterUserExternal(RegisterExternalRequest registerExternalRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return InternalServerError();

            var user = _mapper.Map<UserDTO>(registerExternalRequest);
            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded) return GetErrorResult(createUserResult);

            var addLoginResult = await _userManager.AddLoginAsync(user.Id, loginInfo.Login);
            if (!addLoginResult.Succeeded) return GetErrorResult(createUserResult);

            return Ok();
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult identityResult)
        {
            if (identityResult == null) return InternalServerError();

            if (!identityResult.Succeeded)
            {
                if (identityResult.Errors != null)
                {
                    foreach (string error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private int GetUserId()
        {
            var parsedId = int.Parse(User.Identity.GetUserId());
            return parsedId;
        }
        #endregion
    }
}
