using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MovieCRUD.Web.ViewModels;
using MovieCRUD.Web;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieCRUD.Infrastructure.Network.v1.Interfaces;
using AutoMapper;
using System.Web.Services.Description;
using MovieCRUD.Contracts.V1.Responses.Authentication;
using MovieCRUD.Contracts.V1.Requests.Authentication;
using System.Threading.Tasks;
using MovieCRUD.Contracts.V1.Requests;
using System.Web.UI.WebControls;

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

        public AccountController() { }

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
        public ActionResult VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = SignInManager.TwoFactorSignIn(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code. ");
                    return View(model);
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
        public ActionResult SendCode(string returnUrl, bool rememberMe)
        {
            var userId = SignInManager.GetVerifiedUserId();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = UserManager.GetValidTwoFactorProviders(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!SignInManager.SendTwoFactorCode(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = AuthenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = SignInManager.ExternalSignIn(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Manage");
            if (!ModelState.IsValid) return View(model);

            var loginInfo = AuthenticationManager.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                return View("ExternalLoginFailure");
            }

            ViewBag.ReturnUrl = returnUrl;
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (!ModelState.IsValid) return View(forgotPasswordModel);

            var user = UserManager.FindByName(forgotPasswordModel.Email);
            if (user == null || !UserManager.IsEmailConfirmed(user.Id))
            {
                return View("ForgotPasswordConfirmation");
            }
            var resetToken = UserManager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, resetToken });
            UserManager.SendEmail(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = UserManager.ConfirmEmail(userId, code);
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