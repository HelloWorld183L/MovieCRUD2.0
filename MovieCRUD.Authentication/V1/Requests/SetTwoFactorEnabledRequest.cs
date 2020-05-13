using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieCRUD.Authentication.V1.Requests
{
    public class SetTwoFactorEnabledRequest
    {
        public string UserId { get; set; }
        public bool IsEnabled { get; set; }
    }
}