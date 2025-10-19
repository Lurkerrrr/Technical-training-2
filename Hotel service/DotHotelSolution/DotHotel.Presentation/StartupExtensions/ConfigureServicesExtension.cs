using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.InfrastructureContracts.Identity;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.QueryBuilders;
using DotHotel.Core.Querying;
using DotHotel.Core.ServiceContracts;
using DotHotel.Core.Services;
using DotHotel.Infrastructure.Services.Identity;

namespace DotHotel.Presentation.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IQueryBuilder<ApartmentSearchOptions, Apartment>, ApartmentQueryBuilder>();
            services.AddScoped<IQueryBuilder<ReservationSearchOptions, Reservation>, ReservationQueryBuilder>();

            services.AddScoped<IApartmentService, ApartmentService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IPricingService, PricingService>();

            services.AddScoped<IUserRoleService, UserRoleService>();

            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
