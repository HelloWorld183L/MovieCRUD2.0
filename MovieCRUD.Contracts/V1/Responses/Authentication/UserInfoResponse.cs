namespace MovieCRUD.Contracts.V1.Responses
{
    public class UserInfoResponse
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }
}