using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MovieCRUD.Authentication.Models.IdentityModels;
using MovieCRUD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCRUD.Authentication
{
    public class ApplicationUserManager : UserManager<UserDTO>
    {
        public ApplicationUserManager(IUserStore<UserDTO> userStore)
            : base(userStore)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> identityFactoryOptions, IOwinContext context)
        {
            var userStore = new UserStore<UserDTO>(context.Get<ApplicationDbContext>());

            var userManager = new ApplicationUserManager(userStore);

            ConfigureValidation(userManager);

            var dataProtectionProvider = identityFactoryOptions.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                userManager.UserTokenProvider = new DataProtectorTokenProvider<UserDTO>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return userManager;
        }

        private static void ConfigureValidation(ApplicationUserManager userManager)
        {
            userManager.UserValidator = new UserValidator<UserDTO>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
        }
    }
}
