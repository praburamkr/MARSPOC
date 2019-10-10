using System.Threading.Tasks;
using GL.MARS.CommunicationBlocks.EventBus.Events;

namespace GL.MARS.CommunicationBlocks.EventBus.Concepts
{
    /// <summary>
    /// All Events handled by the event handlers must inherit from this interface.
    /// </summary>
    /// <typeparam name="TIntegrationEvent"></typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
         where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
        // additional custom methods...
    }
}
