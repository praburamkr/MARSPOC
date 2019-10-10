using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationWebApi.Configuration
{
    public class NotificationHubConfiguration
    {
        public string ConnectionString { get; set; }
        public string HubName { get; set; }
        public string SMTPAddress { get; set; }
        public string EmailPortNumber { get; set; }
        public string SendFromEmail { get; set; }
        public string SendFromPassword { get; set; }
        public string EmailAttachmetnPath { get; set; }
    }
}
