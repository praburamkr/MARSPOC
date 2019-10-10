using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationWebApi.Configuration
{
    public class EmailNotificationConfiguration
    {
        public string SMTPAddress { get; set; }
        public string EmailPortNumber { get; set; }
        public string SendFromEmail { get; set; }
        public string SendFromPassword { get; set; }
        public string EmailAttachmetnPath { get; set; }
    }
}
