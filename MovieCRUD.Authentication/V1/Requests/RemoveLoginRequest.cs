using System.ComponentModel.DataAnnotations;

namespace MovieCRUD.Authentication.Requests
{
    public class RemoveLoginRequest
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }

        public RemoveLoginRequest(string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }
    }
}