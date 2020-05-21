namespace MovieCRUD.Authentication.V1.Requests
{
    public class ChangePhoneNumberRequest
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}