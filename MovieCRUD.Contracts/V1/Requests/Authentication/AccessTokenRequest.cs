using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Contracts.V1.Requests.Authentication
{
    public class AccessTokenRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
