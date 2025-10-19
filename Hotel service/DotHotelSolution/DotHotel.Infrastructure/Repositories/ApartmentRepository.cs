using System.Linq.Dynamic.Core;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Domain.RepositoryContracts;
using DotHotel.Core.Querying;
using DotHotel.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace DotHotel.Infrastructure.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        protected readonly ApplicationDbContext _db;

        public ApartmentRepository(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<List<Apartment>> GetApartmentsAsync(QueryOptions<Apartment>? queryOptions)
        {
            var apartmentsQuery = _db.Apartments
                .Include(a => a.Reservations)
                .Include(a => a.ApartmentImages)
                .AsQueryable();

            if (queryOptions?.Filter != null)
            {
                apartmentsQuery = apartmentsQuery.Where(queryOptions.Filter);
            }

            if (queryOptions?.Sort != null)
            {
                apartmentsQuery = apartmentsQuery.OrderBy(queryOptions.Sort);
            }

            if (queryOptions?.PageNumber > 0 && queryOptions?.PageSize > 0)
            {
                apartmentsQuery = apartmentsQuery
                    .Skip((queryOptions.PageNumber - 1) * queryOptions.PageSize)
                    .Take(queryOptions.PageSize);
            }

            return await apartmentsQuery.ToListAsync();
        }

        public async Task<Apartment?> GetApartmentByIdAsync(Guid apartmentId)
        {
            return await _db.Apartments
                .Include(a => a.ApartmentImages)
                .FirstOrDefaultAsync(a => a.Id == apartmentId);
        }

        public async Task<Apartment> AddApartmentAsync(Apartment apartment)
        {
            if (apartment == null)
            {
                throw new ArgumentNullException(nameof(apartment));
            }

            await _db.Apartments.AddAsync(apartment);
            await _db.SaveChangesAsync();

            return apartment;
        }

        public async Task<Apartment> UpdateApartmentAsync(Apartment apartment)
        {
            if (apartment == null)
            {
                throw new ArgumentNullException(nameof(apartment));
            }

            var existingApartment = await _db.Apartments.Include(x => x.ApartmentImages).FirstOrDefaultAsync(a => a.Id == apartment.Id);
            if (existingApartment == null)
            {
                throw new KeyNotFoundException($"Apartment with ID {apartment.Id} not found.");
            }

            existingApartment.Name = apartment.Name;
            existingApartment.Description = apartment.Description;
            existingApartment.Rating = apartment.Rating;
            existingApartment.Capacity = apartment.Capacity;
            existingApartment.PricePerNightPerPersonUsd = apartment.PricePerNightPerPersonUsd;
            existingApartment.IsActive = apartment.IsActive;
            existingApartment.ApartmentImages = apartment.ApartmentImages;

            await _db.SaveChangesAsync();

            return existingApartment;
        }

        public async Task<bool> DeleteApartmentAsync(Guid apartmentId)
        {
            var apartment = _db.Apartments.Find(apartmentId);
            if (apartment == null)
            {
                throw new KeyNotFoundException($"Apartment with ID {apartmentId} not found.");
            }

            _db.Apartments.Remove(apartment);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAndActiveAsync(Guid apartmentId)
        {
            return await _db.Apartments
                .Where(a => a.Id == apartmentId && a.IsActive)
                .AnyAsync();
        }

        public async Task<int?> GetApartmentCapacityAsync(Guid apartmentId)
        {
            return await _db.Apartments
                .Where(a => a.Id == apartmentId)
                .Select(a => a.Capacity)
                .FirstOrDefaultAsync();
        }
    }
}
