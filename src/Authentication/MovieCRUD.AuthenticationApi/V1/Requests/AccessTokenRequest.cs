using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Authentication.Requests
{
    public class AccessTokenRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
