using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieCRUD.Authentication.V1.Requests
{
    public class AddPasswordRequest
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
    }
}