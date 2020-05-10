using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MovieCRUD.Controllers;
using MovieCRUD.Enums;
using MovieCRUD.Infrastructure.Notifications.Messages;
using MovieCRUD.Infrastructure.Persistence.Interfaces;
using MovieCRUD.Web.ViewModels;

namespace MovieCRUD.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IUserRepository _userRepo;

        public ManageController()
        {
        }

        public ManageController(IUserRepository repo)
        {
            _userRepo = repo;
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
            var userLogins = await _userRepo.GetLoginsAsync(User.Identity.GetUserId());

            var model = new IndexViewModel
            {
                HasPassword = await HasPassword(),
                PhoneNumber = await _userRepo.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactorEnabled = await _userRepo.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = userLogins.ToList(),
                BrowserRemembered = AuthenticationManager.TwoFactorBrowserRemembered(User.Identity.GetUserId())
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var result = await _userRepo.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
                if (user != null) await _userRepo.SignIn(user, isPersistent: false, rememberBrowser: false);
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
        public ActionResult AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Generate the token and send it
            var code = _userRepo.GenerateChangePhoneNumberToken(User.Identity.GetUserId(), model.Number);
            if (_userRepo.SmsService != null)
            {
                var message = new SmsMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                _userRepo.SmsService.SendMessage(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await _userRepo.SetTwoFactorEnabled(User.Identity.GetUserId(), true);
            var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await _userRepo.SignIn(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await _userRepo.SetTwoFactorEnabled(User.Identity.GetUserId(), false);
            var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null) await _userRepo.SignIn(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult VerifyPhoneNumber(string phoneNumber)
        {
            var code = _userRepo.GenerateChangePhoneNumberToken(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") :
                View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _userRepo.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
                if (user != null) await _userRepo.SignIn(user, isPersistent: false, rememberBrowser: false);

                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await _userRepo.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded) return RedirectToAction("Index", new { Message = ManageMessageId.Error });

            var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await _userRepo.SignIn(user, isPersistent: false, rememberBrowser: false);
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
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _userRepo.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await _userRepo.SignIn(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            ControllerExtensions.Instance.AddErrors(result);
            return View(model);
        }

        [HttpGet]
        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var addPasswordResult = await _userRepo.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            if (addPasswordResult.Succeeded)
            {
                var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await _userRepo.SignIn(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
            if (user == null) return View("Error");

            var userLogins = await _userRepo.GetLoginsAsync(User.Identity.GetUserId());
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

            var addLoginResult = await _userRepo.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);

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
            var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private async Task<bool> HasPhoneNumber()
        {
            var user = await _userRepo.GetUserByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }
#endregion
    }
}