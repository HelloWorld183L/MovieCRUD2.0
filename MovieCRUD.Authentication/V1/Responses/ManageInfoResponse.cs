using MovieCRUD.Authentication.ViewModels;
using System.Collections.Generic;

namespace MovieCRUD.Authentication.Responses
{
    public class ManageInfoResponse
    {
        public string LocalLoginProvider { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }
        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }
}