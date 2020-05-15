using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieCRUD.Authentication.V1.Requests
{
    public class TwoFactorSignInRequest
    {
        public string Provider { get; set; }
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberBrowser { get; set; }
        public bool RememberMe { get; set; }
    }
}