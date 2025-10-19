using DotHotel.Core.DispatcherContracts;
using DotHotel.Core.Domain.Events.DomainEvents;
using DotHotel.Core.Domain.Events.DomainEventsHandlers;

namespace DotHotel.Infrastructure.Dispatchers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IEnumerable<IDomainEventHandler> _handlers;

        public DomainEventDispatcher(IEnumerable<IDomainEventHandler> handlers)
        {
            _handlers = handlers?.ToList() ?? throw new ArgumentNullException(nameof(handlers), "Handlers collection cannot be null.");
        }

        public async Task DispatchAsync(IDomainEvent domainEvent)
        {
            var eventType = domainEvent.GetType();

            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(eventType))
                {
                    await handler.HandleAsync(domainEvent);
                }
            }
        }

    }
}
