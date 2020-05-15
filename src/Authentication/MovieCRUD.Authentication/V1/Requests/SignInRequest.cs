using MovieCRUD.Domain.Authentication;

namespace MovieCRUD.Authentication.V1.Requests
{
    public class SignInRequest
    {
        public User User { get; set; }
        public bool IsPersistent { get; set; }
        public bool RememberBrowser { get; set; }
    }
}