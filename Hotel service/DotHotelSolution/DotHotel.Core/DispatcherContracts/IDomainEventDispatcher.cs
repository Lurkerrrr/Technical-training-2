using DotHotel.Core.Domain.Events.DomainEvents;

namespace DotHotel.Core.DispatcherContracts
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent);
    }
}
