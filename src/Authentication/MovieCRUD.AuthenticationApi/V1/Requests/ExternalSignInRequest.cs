using Microsoft.AspNet.Identity.Owin;

namespace MovieCRUD.Authentication.V1.Requests
{
    public class ExternalSignInRequest
    {
        public ExternalLoginInfo LoginInfo { get; set; }
        public bool IsPersistent { get; set; }
    }
}