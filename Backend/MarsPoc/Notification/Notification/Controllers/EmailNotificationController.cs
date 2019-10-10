using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationWebApi.Email;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Options;
using NotificationWebApi.Configuration;
using NotificationWebApi.NotificationHubs;
using Microsoft.Azure.NotificationHubs;
using System.Net;
using Common.Notification;

namespace NotificationWebApi.Controllers
{
    [Route("api/notifications/email")]
    [ApiController]
    public class EmailNotificationController : ControllerBase
    {
        private EmailNotificationProxy _emailNotificationProxy;

        public EmailNotificationController(IOptions<NotificationHubConfiguration> standardNotificationHubConfiguration)
        {
            _emailNotificationProxy = new EmailNotificationProxy(standardNotificationHubConfiguration.Value);
        }

        [HttpPost("send")]
        public EmailNotificationResponsecs SendEmail(Common.Notification.SendMailRequest emailModel)
        {
            EmailNotificationResponsecs emailDeliveryResult = _emailNotificationProxy.SendEmail(emailModel);
            return emailDeliveryResult;
        }
    }
}