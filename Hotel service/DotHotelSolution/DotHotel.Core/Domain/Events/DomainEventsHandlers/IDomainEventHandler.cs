using DotHotel.Core.Domain.Events.DomainEvents;

namespace DotHotel.Core.Domain.Events.DomainEventsHandlers
{
    public interface IDomainEventHandler
    {
        bool CanHandle(Type eventType);

        Task HandleAsync(IDomainEvent domainEvent);
    }
}
