using System.Threading.Channels;
using DotHotel.Core.DispatcherContracts;
using DotHotel.Core.Domain.Events.DomainEvents;

namespace DotHotel.Presentation.BackgroundServices
{
    public class DomainEventBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Channel<IDomainEvent> _eventChannel;

        public DomainEventBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _eventChannel = Channel.CreateUnbounded<IDomainEvent>();
        }

        public async Task EnqueueEventAsync(IDomainEvent domainEvent)
        {
            await _eventChannel.Writer.WriteAsync(domainEvent);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var domainEvent in _eventChannel.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = _serviceProvider.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();

                try
                {
                    await dispatcher.DispatchAsync(domainEvent);
                }
                catch { }
            }
        }
    }
}
