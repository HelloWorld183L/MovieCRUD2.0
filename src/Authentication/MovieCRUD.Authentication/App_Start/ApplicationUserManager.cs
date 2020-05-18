using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MovieCRUD.Authentication.Models.IdentityModels;
using MovieCRUD.Authentication.Stores;
using MovieCRUD.Infrastructure.Persistence.Interfaces;

namespace MovieCRUD.Authentication
{
    public class ApplicationUserManager : UserManager<UserDTO, int>
    {
        private static IMapper _mapper;
        private static IUserRepository _userRepo;

        public ApplicationUserManager(IUserStore<UserDTO, int> userStore, IMapper mapper, IUserRepository userRepository) : base(userStore) 
        {
            _mapper = mapper;
            _userRepo = userRepository;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> identityFactoryOptions, IOwinContext context)
        {
            var userStore = new UserStore(_userRepo, _mapper);
            var userManager = new ApplicationUserManager(userStore, _mapper, _userRepo);
            ConfigureValidation(userManager);

            var dataProtectionProvider = identityFactoryOptions.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                userManager.UserTokenProvider = new DataProtectorTokenProvider<UserDTO, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return userManager;
        }

        private static void ConfigureValidation(ApplicationUserManager userManager)
        {
            userManager.UserValidator = new UserValidator<UserDTO, int>(userManager)
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