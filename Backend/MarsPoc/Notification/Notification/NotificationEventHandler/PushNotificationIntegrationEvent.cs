using System;
using System.Collections.Generic;
using System.Text;
using GL.MARS.CommunicationBlocks.EventBus.Events;

namespace GL.MARS.Models.NotificationEvents
{
    public class PushNotificationIntegrationEvent : IntegrationEvent
    {
        //Tags is User Name
        public string[] Tags { get; set; }
        public string[] EventId { get; set; } //"Patient"
        public Dictionary<string, string> Data { get; set; }
    }
}
