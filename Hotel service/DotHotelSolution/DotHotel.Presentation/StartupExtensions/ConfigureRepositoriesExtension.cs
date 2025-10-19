using DotHotel.Core.Domain.RepositoryContracts;
using DotHotel.Infrastructure.Repositories;

namespace DotHotel.Presentation.StartupExtensions
{
    public static class ConfigureRepositoriesExtension
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            return services;
        }
    }
}
