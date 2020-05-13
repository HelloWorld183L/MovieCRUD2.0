using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Authentication.Requests
{
    public class AddExternalLoginRequest
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
}