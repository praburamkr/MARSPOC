using Microsoft.Azure.ServiceBus;
using System;

namespace GL.MARS.CommunicationBlocks.AZServiceBus
{
    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}
