using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GL.MARS.CommunicationBlocks.EventBus.Concepts
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
