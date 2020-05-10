using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Contracts.V1.Requests
{
    public class RemoveLoginRequest
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }
}