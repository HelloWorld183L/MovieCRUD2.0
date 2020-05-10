using MovieCRUD.Contracts.ViewModels;
using System.Collections.Generic;

namespace MovieCRUD.Contracts.V1.Responses
{
    public class ManageInfoResponse
    {
        public string LocalLoginProvider { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }
        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }
}