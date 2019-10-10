using Microsoft.AspNetCore.SignalR;
using Notification.Interface;

namespace Notification.NotificationHubs
{
    public class WebNotificationHub : Hub<ITypedHubClient>
    {
    }
}
