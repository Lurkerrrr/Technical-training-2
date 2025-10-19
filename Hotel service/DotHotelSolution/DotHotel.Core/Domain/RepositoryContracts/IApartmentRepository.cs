using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.Domain.RepositoryContracts
{
    public interface IApartmentRepository
    {
        Task<List<Apartment>> GetApartmentsAsync(QueryOptions<Apartment>? queryOptions);

        Task<Apartment?> GetApartmentByIdAsync(Guid apartmentId);

        Task<Apartment> AddApartmentAsync(Apartment apartment);

        Task<Apartment> UpdateApartmentAsync(Apartment apartment);

        Task<bool> DeleteApartmentAsync(Guid apartmentId);

        Task<bool> ExistsAndActiveAsync(Guid apartmentId);

        Task<int?> GetApartmentCapacityAsync(Guid apartmentId);
    }
}
