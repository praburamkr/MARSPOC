using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.Interface
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string JSONMessage, string userid);
    }
}
