using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Authentication.Requests
{
    public class RegisterExternalRequest
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}