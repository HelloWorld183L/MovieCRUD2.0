namespace MovieCRUD.Authentication.V1.Requests
{
    public class SetPhoneNumberRequest
    {
        public string UserId { get; set; }
        public string NewPhoneNumber { get; set; }

        public SetPhoneNumberRequest(string userId, string phoneNumber)
        {
            NewPhoneNumber = phoneNumber;
            UserId = userId;
        }
    }
}