using System.Linq.Dynamic.Core;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Domain.RepositoryContracts;
using DotHotel.Core.Enums;
using DotHotel.Core.Querying;
using DotHotel.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace DotHotel.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        protected readonly ApplicationDbContext _db;

        public ReservationRepository(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<List<Reservation>> GetReservationsAsync(QueryOptions<Reservation>? queryOptions)
        {
            var reservationsQuery = _db.Reservations
                .Include(a => a.Apartment)
                    .ThenInclude(a => a.ApartmentImages)
                .Include(a => a.ReservationHolder)
                .AsQueryable();

            if (queryOptions?.Filter != null)
            {
                reservationsQuery = reservationsQuery.Where(queryOptions.Filter);
            }

            if (queryOptions?.Sort != null)
            {
                reservationsQuery = reservationsQuery.OrderBy(queryOptions.Sort);
            }

            if (queryOptions?.PageNumber > 0 && queryOptions?.PageSize > 0)
            {
                reservationsQuery = reservationsQuery
                    .Skip((queryOptions.PageNumber - 1) * queryOptions.PageSize)
                    .Take(queryOptions.PageSize);
            }

            return await reservationsQuery.ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(Guid reservationId)
        {
            return await _db.Reservations
                .Include(x => x.Apartment)
                    .ThenInclude(a => a.ApartmentImages)
                .Include(x => x.ReservationHolder)
                .FirstOrDefaultAsync(a => a.Id == reservationId);
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            await _db.Reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();

            return reservation;
        }

        public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
        {
            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            var existingReservation = _db.Reservations.Find(reservation.Id);
            if (existingReservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID {reservation.Id} not found.");
            }

            existingReservation.CheckInDate = reservation.CheckInDate;
            existingReservation.CheckOutDate = reservation.CheckOutDate;
            existingReservation.GuestCount = reservation.GuestCount;
            existingReservation.TotalPriceUsd = reservation.TotalPriceUsd;
            existingReservation.ReservationStatus = reservation.ReservationStatus;
            existingReservation.PaymentMethod = reservation.PaymentMethod;
            existingReservation.ReservationHolderId = reservation.ReservationHolderId;
            existingReservation.ApartmentId = reservation.ApartmentId;

            await _db.SaveChangesAsync();

            return existingReservation;
        }

        public async Task<bool> HasOverlappingReservationsAsync(Guid apartmentId, DateTime checkInDate, DateTime checkOutDate)
        {
            return await _db.Reservations
                .Where(r => r.ApartmentId == apartmentId &&
                           r.ReservationStatus != ReservationStatus.Cancelled &&
                           r.CheckInDate < checkOutDate &&
                           r.CheckOutDate > checkInDate)
                .AnyAsync();
        }
    }
}
