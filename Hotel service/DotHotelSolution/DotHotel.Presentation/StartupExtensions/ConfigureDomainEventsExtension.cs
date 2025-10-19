using DotHotel.Core.DispatcherContracts;
using DotHotel.Core.Domain.Events.DomainEventsHandlers;
using DotHotel.Infrastructure.Dispatchers;
using DotHotel.Infrastructure.Events.DomainEventHandlers.ReservationEventsHandlers.EmailHandlers;

namespace DotHotel.Presentation.StartupExtensions
{
    public static class ConfigureDomainEventsExtension
    {
        public static IServiceCollection ConfigureDomainEvents(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventHandler, ReservationCreatedEmailHandler>();
            services.AddScoped<IDomainEventHandler, ReservationConfirmedEmailHandler>();
            services.AddScoped<IDomainEventHandler, ReservationCanceledEmailHandler>();
            services.AddScoped<IDomainEventHandler, ReservationCheckedInEmailHandler>();
            services.AddScoped<IDomainEventHandler, ReservationCheckedOutEmailHandler>();

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            return services;
        }
    }
}
