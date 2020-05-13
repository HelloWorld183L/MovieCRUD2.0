using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieCRUD.Authentication.V1.Requests
{
    public class GenerateChangePhoneNumberTokenRequest
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
    }
}