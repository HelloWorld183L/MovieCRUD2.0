namespace MovieCRUD.Domain.Authentication
{
    public class UserLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public int UserId { get; set; }
    }
}
