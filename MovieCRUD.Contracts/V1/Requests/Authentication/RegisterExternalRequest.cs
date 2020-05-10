using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Contracts.V1.Requests
{
    public class RegisterExternalRequest
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}