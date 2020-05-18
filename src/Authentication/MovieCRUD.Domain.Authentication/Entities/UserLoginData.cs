namespace MovieCRUD.Domain.Authentication
{
    public class UserLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        // Foreign key
        public int UserId { get; set; }
    }
}
