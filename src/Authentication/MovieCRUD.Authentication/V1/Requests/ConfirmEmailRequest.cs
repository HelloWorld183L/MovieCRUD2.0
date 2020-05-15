namespace MovieCRUD.Authentication.V1.Requests
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}