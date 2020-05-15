using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCRUD.Desktop.ViewModels
{
    public class LoginViewModel : BaseAuthenticationViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            this.DisplayName = "Login";
        }
    }
}
