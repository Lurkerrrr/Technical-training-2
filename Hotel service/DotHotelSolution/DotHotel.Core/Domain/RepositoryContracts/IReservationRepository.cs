using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Querying;

namespace DotHotel.Core.Domain.RepositoryContracts
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetReservationsAsync(QueryOptions<Reservation>? queryOptions);

        Task<Reservation?> GetReservationByIdAsync(Guid apartmentId);

        Task<Reservation> CreateReservationAsync(Reservation reservation);

        Task<Reservation> UpdateReservationAsync(Reservation reservation);

        Task<bool> HasOverlappingReservationsAsync(Guid apartmentId, DateTime checkInDate, DateTime checkOutDate);
    }
}
