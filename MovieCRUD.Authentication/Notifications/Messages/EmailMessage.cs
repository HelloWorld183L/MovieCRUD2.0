using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieCRUD.Authentication.Notifications.Messages
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailMessage(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
    }
}