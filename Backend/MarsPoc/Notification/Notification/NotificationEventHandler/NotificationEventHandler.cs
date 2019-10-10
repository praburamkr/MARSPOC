using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GL.MARS.CommunicationBlocks.EventBus.Concepts;
using GL.MARS.Models.NotificationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Notification.Repositories;
using NotificationWebApi.Configuration;
using NotificationWebApi.NotificationHubs;

namespace NotificationWebApi
{
    public class NotificationEventHandler : IIntegrationEventHandler<PushNotificationIntegrationEvent>
    {
        NotificationHubProxy _notificationHubProxy;
        private readonly UserRepository userContext;
        public NotificationEventHandler(Microsoft.Extensions.Options.IOptions<NotificationHubConfiguration> standardNotificationHubConfiguration, UserRepository userContext)
        {
            _notificationHubProxy = new NotificationHubProxy(standardNotificationHubConfiguration.Value, userContext);
        }

        public Task Handle(PushNotificationIntegrationEvent @event)
        {
            SendNotification(@event);
            return Task.CompletedTask;
        }
        
        private async void SendNotification(PushNotificationIntegrationEvent newNotification)
        {
            HubResponse<NotificationOutcome> pushDeliveryResult = await _notificationHubProxy.SendNotification(newNotification);
        }
    }
}
