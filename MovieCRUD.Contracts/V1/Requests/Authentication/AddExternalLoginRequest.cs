using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Contracts.V1.Requests
{
    public class AddExternalLoginRequest
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
}