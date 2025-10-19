using DotHotel.Core.Domain.Events.DomainEvents;

namespace DotHotel.Core.Domain.Events.DomainEventsHandlers
{
    public abstract class DomainEventHandler<TSpecificEvent> : IDomainEventHandler where TSpecificEvent : IDomainEvent
    {
        public bool CanHandle(Type domainEvent)
        {
            return domainEvent == typeof(TSpecificEvent) || domainEvent.IsSubclassOf(typeof(TSpecificEvent));
        }

        public async Task HandleAsync(IDomainEvent domainEvent)
        {
            if (domainEvent is TSpecificEvent specificEvent)
            {
                await HandleAsync(specificEvent);
            }
            else
            {
                throw new InvalidOperationException($"Cannot handle event of type {domainEvent.GetType().Name} with handler for {typeof(TSpecificEvent).Name}.");
            }
        }

        public abstract Task HandleAsync(TSpecificEvent domainEvent);
    }
}
