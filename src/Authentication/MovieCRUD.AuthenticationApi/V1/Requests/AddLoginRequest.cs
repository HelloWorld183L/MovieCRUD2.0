using Microsoft.AspNet.Identity;

namespace MovieCRUD.Authentication.V1.Requests
{
    public class AddLoginRequest
    {
        public string UserId { get; set; }
        public UserLoginInfo LoginInfo { get; set; }
    }
}