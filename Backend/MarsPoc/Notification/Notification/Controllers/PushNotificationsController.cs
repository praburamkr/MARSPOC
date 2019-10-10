using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using Notification.Interface;
using Notification.Models;
using Notification.NotificationHubs;
using Notification.Repositories;
using NotificationWebApi.Configuration;
using NotificationWebApi.NotificationHubs;
using Common.Notification;
using Common.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace NotificationWebApi.Controllers
{
    /// 
    /// <summary>
    /// Anonymous access is only for testing purposes
    /// Remember to enable authentication
    /// </summary>

    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/notifications")]
    [ApiController]
    public class PushNotificationsController : ControllerBase
    {
        private NotificationHubProxy _notificationHubProxy;
        private IHubContext<WebNotificationHub, ITypedHubClient> _hubSignalRContext;
        private readonly UserRepository userContext;
        private readonly IExceptionHandler exceptionHandler;

        public PushNotificationsController(
            IOptions<NotificationHubConfiguration> standardNotificationHubConfiguration,
            IHubContext<WebNotificationHub, ITypedHubClient> hubSignalRContext,
            UserRepository userContext,
            IExceptionHandler exceptionHandler)
        {
            _notificationHubProxy = new NotificationHubProxy(standardNotificationHubConfiguration.Value, userContext);
            _hubSignalRContext = hubSignalRContext;
            this.userContext = userContext;
            this.exceptionHandler = exceptionHandler;
        }

        [HttpGet("Test")]
        public string TestNotificationController()
        {
            return "Welcome to Push Notification Service !!" + DateTime.Now.ToString();
        }

        [HttpGet("register")]
        public async Task<object> CreateRegistrationId()
        {
            string regID = await _notificationHubProxy.CreateRegistrationId();
            var obj = new { registrationID = regID };
            return obj;
        }

        [HttpPut("enable")]
        public async Task<IActionResult> RegisterForPushNotifications(DeviceRegistration deviceUpdate)
        {
            HubResponse registrationResult = await _notificationHubProxy.RegisterForPushNotifications(deviceUpdate);

            if (registrationResult.CompletedWithSuccess)
                return Ok();

            return StatusCode((int)HttpStatusCode.ExpectationFailed);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] Common.Notification.Notification newNotification)
        {
            HubResponse<NotificationOutcome> pushDeliveryResult = await _notificationHubProxy.SendNotification(newNotification, _hubSignalRContext);

            if (pushDeliveryResult.CompletedWithSuccess)
                return Ok();

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpPost("userinfo")]
        public async Task<IActionResult> StoreUserInfo([FromBody]UserModel userInfo)
        {
            return await this.exceptionHandler.SendResponse(this, this.userContext.CreateOrUpdateAsync(userInfo));
        }
    }
}