using DotHotel.Presentation.BackgroundServices;

namespace DotHotel.Presentation.StartupExtensions
{
    public static class ConfigureBackgroundServicesExtension
    {
        public static IServiceCollection ConfigureBackgroundServices(this IServiceCollection services)
        {
            services.AddSingleton<DomainEventBackgroundService>();
            services.AddHostedService(provider => provider.GetRequiredService<DomainEventBackgroundService>());

            return services;
        }
    }
}
