﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MovieCRUD.Authentication.Models;
using MovieCRUD.Authentication.Models.IdentityModels;
using MovieCRUD.Authentication.Providers;
using MovieCRUD.Authentication.Results;
using MovieCRUD.Contracts.V1.ApiRoutes;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Domain.DomainObjects;
using MovieCRUD.Infrastructure.Persistence.Interfaces;

namespace MovieCRUD.Authentication.Controllers
{
    [Authorize]
    [RoutePrefix(ApiRoutes.AccountRoutes.Prefix)]
    public class AccountController : ApiController
    {
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }
        private const string LocalLoginProvider = "Local";
        private IUserRepository _userRepo;
        private IMapper _mapper;

        public AccountController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepo = userRepository;
            _mapper = mapper;
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route(ApiRoutes.AccountRoutes.GetUserInfo)]
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

        [Route(ApiRoutes.AccountRoutes.Logout)]
        public IHttpActionResult Logout()
        {
            AuthenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        [Route(ApiRoutes.AccountRoutes.GetManageInfo)]
        public async Task<ManageInfoResponse> GetManageInfo(string returnUrl, bool generateState = false)
        {
            var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());

            var mappedUser = _mapper.Map<UserDTO>(user);

            if (user == null) return null;

            var logins = new List<ViewModels.UserLoginInfoViewModel>();

            foreach (var linkedAccount in user.Logins)
            {
                logins.Add(new ViewModels.UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new ViewModels.UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoResponse
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = (IEnumerable<Contracts.ViewModels.UserLoginInfoViewModel>)logins,
                ExternalLoginProviders = (IEnumerable<Contracts.ViewModels.ExternalLoginViewModel>)GetExternalLogins(returnUrl, generateState)
            };
        }

        [Route(ApiRoutes.AccountRoutes.ChangePassword)]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordRequest changePasswordModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var changePasswordResult = await _userRepo.ChangePasswordAsync(
                User.Identity.GetUserId(),
                changePasswordModel.OldPassword,
                changePasswordModel.NewPassword);
            
            if (!changePasswordResult.Succeeded)
            {
                return GetErrorResult(changePasswordResult);
            }

            return Ok();
        }

        [Route(ApiRoutes.AccountRoutes.SetPassword)]
        public async Task<IHttpActionResult> SetPassword(SetPasswordRequest passwordRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var setPasswordResult = await _userRepo.AddPasswordAsync(User.Identity.GetUserId(), passwordRequest.NewPassword);

            if (!setPasswordResult.Succeeded)
            {
                return GetErrorResult(setPasswordResult);
            }

            return Ok();
        }

        [Route(ApiRoutes.AccountRoutes.AddExternalLogin)]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginRequest externalLoginModel)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var authTicket = AccessTokenFormat.Unprotect(externalLoginModel.ExternalAccessToken);

            if (authTicket == null || authTicket.Identity == null || (authTicket.Properties != null
                && authTicket.Properties.ExpiresUtc.HasValue
                && authTicket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            var externalData = ExternalLoginData.FromIdentity(authTicket.Identity);

            if (externalData == null) { return BadRequest("The external login is already associated with an account."); }

            var addLoginResult = await _userRepo.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!addLoginResult.Succeeded) return GetErrorResult(addLoginResult);

            return Ok();
        }

        [Route(ApiRoutes.AccountRoutes.RemoveLogin)]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginRequest removeLoginRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            IdentityResult loginProviderResult;

            if (removeLoginRequest.LoginProvider == LocalLoginProvider)
            {
                loginProviderResult = await _userRepo.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                loginProviderResult = await _userRepo.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(removeLoginRequest.LoginProvider, removeLoginRequest.ProviderKey));
            }

            if (!loginProviderResult.Succeeded) return GetErrorResult(loginProviderResult);

            return Ok();
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route(ApiRoutes.AccountRoutes.GetExternalLogin, Name = ApiRoutes.AccountRoutes.GetExternalLogin)]
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

            var user = await _userRepo.GetExternalUserAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            var hasRegistered = user != null;

            var redirectUri = string.Empty;
            redirectUri = $"{redirectUri}#external_access_token={externalLogin.ExternalAccessToken}&" +
                              $"provider={externalLogin.LoginProvider}&" +
                              $"haslocalaccount={hasRegistered}&" +
                              $"external_user_name={externalLogin.UserName}";

            return Redirect(redirectUri);
        }

        [AllowAnonymous]
        [Route(ApiRoutes.AccountRoutes.GetExternalLogins)]
        public IEnumerable<ViewModels.ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            var authDescriptions = AuthenticationManager.GetExternalAuthenticationTypes();
            var externalLoginViewModels = new List<ViewModels.ExternalLoginViewModel>();

            string state = null;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }

            foreach (var authDescription in authDescriptions)
            {
                var externalLoginViewModel = new ViewModels.ExternalLoginViewModel
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
        [Route(ApiRoutes.AccountRoutes.RegisterUser)]
        public async Task<IHttpActionResult> RegisterUser(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User { Username = registerRequest.Email, Password = registerRequest.Password };

            var registerUserResult = await _userRepo.RegisterUserAsync(user);

            if (!registerUserResult.Succeeded) return GetErrorResult(registerUserResult);

            return Ok();
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route(ApiRoutes.AccountRoutes.RegisterUserExternal)]
        public async Task<IHttpActionResult> RegisterUserExternal(RegisterExternalRequest registerExternalRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) 
            {
                return InternalServerError();
            }

            var user = _mapper.Map<User>(registerExternalRequest);

            var createUserResult = await _userRepo.RegisterUserAsync(user);
            if (!createUserResult.Succeeded) return GetErrorResult(createUserResult);

            var addLoginResult = await _userRepo.AddLoginAsync(user.Id.ToString(), loginInfo.Login);
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
        #endregion
    }
}
