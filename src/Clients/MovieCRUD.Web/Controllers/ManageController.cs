using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MovieCRUD.Authentication.Clients;
using MovieCRUD.Authentication.Notifications.Interfaces;
using MovieCRUD.Authentication.Notifications.Messages;
using MovieCRUD.Authentication.Requests;
using MovieCRUD.Authentication.V1.Requests;
using MovieCRUD.Controllers;
using MovieCRUD.Enums;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Web.ViewModels;

namespace MovieCRUD.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IAuthApiClient _apiClient;
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;

        public ManageController()
        {
        }

        public ManageController(IAuthApiClient client)
        {
            _apiClient = client;
        }

        [HttpGet]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            switch (message)
            {
                case ManageMessageId.ChangePasswordSuccess:
                    ViewBag.StatusMessage = "Your password has been changed.";
                    break;
                case ManageMessageId.SetPasswordSuccess:
                    ViewBag.StatusMessage = "Your password has been set.";
                    break;
                case ManageMessageId.SetTwoFactorSuccess:
                    ViewBag.StatusMessage = "Your two-factor authentication provider has been set.";
                    break;
                case ManageMessageId.Error:
                    ViewBag.StatusMessage = "An error has occured.";
                    break;
                case ManageMessageId.AddPhoneSuccess:
                    ViewBag.StatusMessage = "Your phone number was added.";
                    break;
                case ManageMessageId.RemovePhoneSuccess:
                    ViewBag.StatusMessage = "Your phone number was removed.";
                    break;
                default:
                    ViewBag.StatusMessage = "";
                    break;
            }
            var userLogins = await _apiClient.GetLoginsAsync(User.Identity.GetUserId());

            var model = new IndexViewModel
            {
                HasPassword = await HasPassword(),
                PhoneNumber = await _apiClient.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactorEnabled = await _apiClient.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = userLogins.ToList(),
                BrowserRemembered = AuthenticationManager.TwoFactorBrowserRemembered(User.Identity.GetUserId())
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var removeLoginRequest = new RemoveLoginRequest(loginProvider, providerKey);

            var result = await _apiClient.RemoveLoginAsync(removeLoginRequest);
            if (result.Succeeded)
            {
                var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    var signInRequest = new SignInRequest()
                    {
                        User = user,
                        IsPersistent = false,
                        RememberBrowser = false
                    };
                    await _apiClient.SignInAsync(signInRequest);
                }
            }

            ManageMessageId? message = result.Succeeded ? ManageMessageId.RemoveLoginSuccess : ManageMessageId.Error;
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        [HttpGet]
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPhoneNumber(AddPhoneNumberViewModel addPhoneNumberModel)
        {
            if (!ModelState.IsValid) return View(addPhoneNumberModel);

            // Generate the token and send it
            var request = new GenerateChangePhoneNumberTokenRequest()
            {
                PhoneNumber = addPhoneNumberModel.Number,
                UserId = User.Identity.GetUserId()
            };

            var code = _apiClient.GenerateChangePhoneNumberTokenAsync(request);
            if (_smsService != null)
            {
                var message = new SmsMessage
                {
                    Destination = addPhoneNumberModel.Number,
                    Body = "Your security code is: " + code
                };
                _smsService.SendMessage(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = addPhoneNumberModel.Number });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            var setTwoFactorRequest = new SetTwoFactorEnabledRequest()
            {
                UserId = User.Identity.GetUserId(),
                IsEnabled = true
            };

            await _apiClient.SetTwoFactorEnabledAsync(setTwoFactorRequest);
            var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                var signInRequest = new SignInRequest()
                {
                    User = user,
                    IsPersistent = false,
                    RememberBrowser = false
                };
                await _apiClient.SignInAsync(signInRequest);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            var setTwoFactorRequest = new SetTwoFactorEnabledRequest()
            {
                UserId = User.Identity.GetUserId(),
                IsEnabled = false
            };
            await _apiClient.SetTwoFactorEnabledAsync(setTwoFactorRequest);
            var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                var signInRequest = new SignInRequest()
                {
                    User = user,
                    IsPersistent = false,
                    RememberBrowser = false
                };
                await _apiClient.SignInAsync(signInRequest);
            }

            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult VerifyPhoneNumber(string phoneNumber)
        {
            var request = new GenerateChangePhoneNumberTokenRequest()
            {
                PhoneNumber = phoneNumber,
                UserId = User.Identity.GetUserId()
            };
            var code = _apiClient.GenerateChangePhoneNumberTokenAsync(request);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") :
                View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel verifyPhoneNumberModel)
        {
            if (!ModelState.IsValid) return View(verifyPhoneNumberModel);

            var request = new ChangePhoneNumberRequest()
            {
                UserId = User.Identity.GetUserId(),
                PhoneNumber = verifyPhoneNumberModel.PhoneNumber,
                Code = verifyPhoneNumberModel.Code
            };

            var result = await _apiClient.ChangePhoneNumberAsync(request);
            if (result.Succeeded)
            {
                var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    var signInRequest = new SignInRequest()
                    {
                        User = user,
                        IsPersistent = false,
                        RememberBrowser = false
                    };
                    await _apiClient.SignInAsync(signInRequest);
                }

                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(verifyPhoneNumberModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var request = new SetPhoneNumberRequest()
            {
                UserId = User.Identity.GetUserId(),
                NewPhoneNumber = null
            };

            var result = await _apiClient.SetPhoneNumberAsync(request);
            if (!result.Succeeded) return RedirectToAction("Index", new { Message = ManageMessageId.Error });

            var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                var signInRequest = new SignInRequest()
                {
                    User = user,
                    IsPersistent = false,
                    RememberBrowser = false
                };
                await _apiClient.SignInAsync(signInRequest);
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordModel)
        {
            if (!ModelState.IsValid) return View(changePasswordModel);

            var changePasswordRequest = _mapper.Map<ChangePasswordRequest>(changePasswordModel);

            var result = await _apiClient.ChangePasswordAsync(changePasswordRequest);

            if (!result.Succeeded)
            {
                ControllerExtensions.Instance.AddErrors(result);
                return View(changePasswordModel);
            }

            var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                var signInRequest = new SignInRequest()
                {
                    User = user,
                    IsPersistent = false,
                    RememberBrowser = false
                };
                await _apiClient.SignInAsync(signInRequest);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
        }

        [HttpGet]
        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel setPasswordModel)
        {
            if (!ModelState.IsValid) return View(setPasswordModel);

            var request = new AddPasswordRequest()
            {
                UserId = User.Identity.GetUserId(),
                NewPassword = setPasswordModel.NewPassword
            };

            var addPasswordResult = await _apiClient.AddPasswordAsync(request);
            if (addPasswordResult.Succeeded)
            {
                var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    var signInRequest = new SignInRequest()
                    {
                        User = user,
                        IsPersistent = false,
                        RememberBrowser = false
                    };
                    await _apiClient.SignInAsync(signInRequest);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
            }
            return View(setPasswordModel);
        }

        [HttpGet]
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                  message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
            if (user == null) return View("Error");

            var userLogins = await _apiClient.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().
                Where(auth => userLogins.All(loginInfo => auth.AuthenticationType != loginInfo.LoginProvider)).ToList();

            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count() > 1;

            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins.ToList(),
                OtherLogins = otherLogins
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            const string XsrfKey = "XsrfId";

            var loginInfo = AuthenticationManager.GetExternalLoginInfo(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null) return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });

            var request = new AddLoginRequest()
            {
                UserId = User.Identity.GetUserId(),
                LoginInfo = loginInfo.Login
            };
            var addLoginResult = await _apiClient.AddLoginAsync(request);

            return addLoginResult.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

#region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task<bool> HasPassword()
        {
            var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private async Task<bool> HasPhoneNumber()
        {
            var user = await _apiClient.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }
#endregion
    }
}