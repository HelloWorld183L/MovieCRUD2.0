using Microsoft.AspNet.Identity.Owin;
using MovieCRUD.Web.ViewModels;
using MovieCRUD.Web;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using MovieCRUD.Authentication.Clients;
using MovieCRUD.Authentication.Requests;
using MovieCRUD.Authentication.Responses;
using MovieCRUD.Authentication.V1.Requests;
using MovieCRUD.Authentication.Notifications.Messages;

namespace MovieCRUD.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private const bool rememberBrowser = true;
        private const bool isPersistent = true;
        private IAuthApiClient _apiClient;
        private RequestTokenResponse _accessToken;
        private IMapper _mapper;

        public AccountController(IAuthApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel loginModel, string returnUrl)
        {
            if (!ModelState.IsValid) return View(loginModel);

            var accessTokenRequest = _mapper.Map<AccessTokenRequest>(loginModel);

            _accessToken = await _apiClient.RequestToken(accessTokenRequest);

            return RedirectToAction("List", "Movie");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid) return View(registerModel);

            var registerRequest = _mapper.Map<RegisterRequest>(registerModel);

            await _apiClient.RegisterUserAsync(registerRequest);

            return View(registerModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            await _apiClient.LogoutAsync();
            return RedirectToAction("List", "Movie");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel verifyCodeModel)
        {
            if (!ModelState.IsValid) return View(verifyCodeModel);

            var request = _mapper.Map<TwoFactorSignInRequest>(verifyCodeModel);

            var twoFactorSignInResult = await _apiClient.TwoFactorSignInAsync(request);
            switch (twoFactorSignInResult)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(verifyCodeModel.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code. ");
                    return View(verifyCodeModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid) return View();

            var changePasswordRequest = _mapper.Map<ChangePasswordRequest>(resetPasswordModel);

            await _apiClient.ChangePasswordAsync(changePasswordRequest);

            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _apiClient.GetVerifiedUserIdAsync();
            if (userId == null) return View("Error");

            var userFactors = await _apiClient.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel sendCodeModel)
        {
            if (!ModelState.IsValid) return View();

            var result = await _apiClient.SendTwoFactorAsync(sendCodeModel.SelectedProvider);
            if (!result.Succeeded) return View("Error");

            return RedirectToAction("VerifyCode", new { Provider = sendCodeModel.SelectedProvider, ReturnUrl = sendCodeModel.ReturnUrl, RememberMe = sendCodeModel.RememberMe });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _apiClient.GetExternalLoginInfoAsync();
            if (loginInfo == null) return RedirectToAction("Login");

            var externalSignInRequest = new ExternalSignInRequest()
            {
                LoginInfo = loginInfo,
                IsPersistent = false
            };

            var externalSignInResult = await _apiClient.ExternalSignInAsync(externalSignInRequest);
            switch (externalSignInResult)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Manage");
            if (!ModelState.IsValid) return View(model);

            var loginInfo = await _apiClient.GetExternalLoginInfoAsync();
            if (loginInfo == null) return View("ExternalLoginFailure");

            ViewBag.ReturnUrl = returnUrl;
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (!ModelState.IsValid) return View(forgotPasswordModel);

            var user = await _apiClient.FindByNameAsync(forgotPasswordModel.Email);
            if (user == null || ! await _apiClient.IsEmailConfirmedAsync(user.Id.ToString())) return View("ForgotPasswordConfirmation");

            var resetToken = await _apiClient.GeneratePasswordResetTokenAsync(user.Id.ToString());
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, resetToken });

            var email = new EmailMessage("Reset Password", $"Please reset your password by clicking <a href=\"{callbackUrl}\">here</a>");

            await _apiClient.SendEmailAsync(user.Id.ToString(), email);

            return View(forgotPasswordModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return View("Error");

            var request = new ConfirmEmailRequest()
            {
                Code = code,
                UserId = userId
            };

            var result = await _apiClient.ConfirmEmail(request);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        #region Helper methods
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}